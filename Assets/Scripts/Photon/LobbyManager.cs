using System.Collections.Generic;

using Photon.Pun;
using Photon.Realtime;

using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace CodingStrategy.Photon
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        // 갱신된 방 리스트를 저장해 둘 변수
        public static List<RoomInfo> cachedRoomList = new List<RoomInfo>();
        public RawImage RandomImage;
        public TextMeshProUGUI RandomDescription;
        public RawImage StandardImage;
        public TextMeshProUGUI StandardDescription;
        public TextMeshProUGUI LobbyNickname;

        public string RoomID;

        public GameObject roomPrefab;
        public Transform contentRoomList;
        public GameObject Loading;

        private void Awake()
        {
            LobbyNickname.text = PhotonNetwork.LocalPlayer.NickName;
            Loading.SetActive(false);
        }

        public void OnRandomRoomButtonClick()
        {
            StandardImage.gameObject.SetActive(false);
            StandardDescription.gameObject.SetActive(false);
            RandomImage.gameObject.SetActive(true);
            RandomDescription.gameObject.SetActive(true);

            RoomID = null;
        }

        public override void OnConnectedToMaster()
        {
            TypedLobby lobby = new TypedLobby("coding-strategy", LobbyType.SqlLobby);
            PhotonNetwork.JoinLobby(lobby);
        }

        public void OnJoinedRoomButtonClick()
        {
            if (PhotonNetwork.NetworkingClient.State != ClientState.JoinedLobby)
            {
                return;
            }

            if (string.IsNullOrEmpty(RoomID))
            {
                PhotonNetwork.JoinRandomOrCreateRoom(
                    sqlLobbyFilter: "C0='coding-strategy' AND C1 < 4 AND C2=0",
                    roomOptions: new RoomOptions
                    {
                        MaxPlayers = 4,
                        IsVisible = true,
                        PublishUserId = true,
                        CleanupCacheOnLeave = false,
                        CustomRoomProperties = new Hashtable
                        {
                            { "C0", "coding-strategy" },
                            { "C1", 0 },
                            { "C2", 0 }
                        },
                        CustomRoomPropertiesForLobby = new[] { "C0", "C1", "C2" },
                        BroadcastPropsChangeToAll = true
                    });
            }
            else
            {
                PhotonNetwork.JoinRoom(RoomID);
            }
        }

        public override void OnJoinedLobby()
        {
            Debug.LogFormat("Connected to Master {0}", PhotonNetwork.CurrentLobby);
            InvokeRepeating(nameof(QueryRoomList), 1f, 2f);
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Successfully joined room");

            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.FetchServerTimestamp();
                PhotonNetwork.CurrentRoom.SetCustomProperties(
                    new Hashtable
                    {
                        { "C1", PhotonNetwork.CurrentRoom.PlayerCount },
                        { "Timer", PhotonNetwork.ServerTimestamp }
                    });
            }
            else
            {
                int startTime = (int) PhotonNetwork.CurrentRoom.CustomProperties["Timer"];
                int delay = unchecked(PhotonNetwork.ServerTimestamp - startTime);

                Debug.LogFormat("Server delay is {0}ms", delay);
            }

            PhotonNetwork.LocalPlayer.SetCustomProperties(
                new Hashtable
                {
                    { "isReady", PhotonNetwork.IsMasterClient ? 1 : 0 }
                });

            SceneManager.LoadScene("GameRoom");
        }

        public void ActivateRotationUI()
        {
            Loading.SetActive(true);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            RoomOptions roomOptions = new RoomOptions
            {
                MaxPlayers = 4
            };
            PhotonNetwork.CreateRoom(null, roomOptions);
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            cachedRoomList = roomList;
            UpdateRoomListUI();
        }

        private void QueryRoomList()
        {
            if (!PhotonNetwork.InLobby)
            {
                return;
            }

            Debug.Log("Request to update room list");
            PhotonNetwork.GetCustomRoomList(PhotonNetwork.CurrentLobby, "C0='coding-strategy' AND C1 < 4 AND C2=0");
        }

        public void UpdateRoomListUI()
        {
            foreach (Transform child in contentRoomList)
            {
                Destroy(child.gameObject);
            }

            foreach (RoomInfo room in cachedRoomList)
            {
                GameObject roomObj = Instantiate(roomPrefab, contentRoomList);

                TextMeshProUGUI roomNameText = roomObj.transform.Find("RoomName").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI userCountText = roomObj.transform.Find("UserCount").GetComponent<TextMeshProUGUI>();

                roomNameText.text = room.Name;
                userCountText.text = $"{room.PlayerCount} / {room.MaxPlayers}";

                roomObj.GetComponent<Button>().onClick.AddListener(() => OnRoomClick(room.Name));
            }
        }

        public void OnRoomClick(string roomId)
        {
            RandomImage.gameObject.SetActive(false);
            RandomDescription.gameObject.SetActive(false);
            StandardImage.gameObject.SetActive(true);
            StandardDescription.gameObject.SetActive(true);
            RoomID = roomId;
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log("Player entered room");
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log("Player left room");
        }
    }
}
