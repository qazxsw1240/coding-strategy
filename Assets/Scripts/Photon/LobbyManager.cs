using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public RawImage RandomImage;
    public TextMeshProUGUI RandomDescription;
    public RawImage StandardImage;
    public TextMeshProUGUI StandardDescription;
    public TextMeshProUGUI LobbyNickname;

    public string RoomID;

    public GameObject roomPrefab;
    public Transform contentRoomlist;

    // 갱신된 방 리스트를 저장해 둘 변수
    public static List<RoomInfo> cachedRoomList = new List<RoomInfo>();

    private void Awake()
    {
        LobbyNickname.text = PhotonNetwork.LocalPlayer.NickName;
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
            PhotonNetwork.JoinRandomOrCreateRoom(roomOptions: new RoomOptions
            {
                MaxPlayers = 4,
                IsVisible = true,
                PublishUserId = true,
                CleanupCacheOnLeave = false,
                CustomRoomProperties = new Hashtable
                {
                    { "C0", "coding-strategy" },
                    { "C1", 0 }
                },
                CustomRoomPropertiesForLobby = new string[] { "C0", "C1" },
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
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable
            {
                { "C1", PhotonNetwork.CurrentRoom.PlayerCount }
            });
        }

        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable
        {
            { "isReady", PhotonNetwork.IsMasterClient ? 1 : 0 }
        });

        SceneManager.LoadScene("GameRoom");
    }


    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 4
        };
        PhotonNetwork.CreateRoom(null, roomOptions, null);
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
        PhotonNetwork.GetCustomRoomList(PhotonNetwork.CurrentLobby, "C0='coding-strategy' AND C1 < 4");
    }

    public void UpdateRoomListUI()
    {
        foreach (Transform child in contentRoomlist)
        {
            Destroy(child.gameObject);
        }

        foreach (RoomInfo room in cachedRoomList)
        {
            GameObject roomObj = Instantiate(roomPrefab, contentRoomlist);

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
