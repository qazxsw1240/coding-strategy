using System.Collections;
using System.Text;

using CodingStrategy.Entities;
using CodingStrategy.Network;
using CodingStrategy.UI.GameScene;
using CodingStrategy.Utility;

using Photon.Pun;
using Photon.Realtime;

using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;

using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace CodingStrategy.Photon
{
    public class RoomManager : MonoBehaviourPunCallbacks
    {
        public TextMeshProUGUI[] playerNicknames; // 플레이어 닉네임 텍스트 배열
        public TextMeshProUGUI[] playerReady; // 플레이어 닉네임 텍스트 배열
        public TextMeshProUGUI[] Master;

        public bool[] ready = new bool[3];

        public GameObject startButton; // 시작 버튼
        public GameObject readyButton; // 레디

        public ChatManager ChatManager;

        public readonly Color ReadyGreen = new Vector4(51, 164, 49, 255) / 255;

        // public InGameStatusSynchronizer statusSynchronizer;

        private void Start()
        {
            GameInitializer.Initialize();

            StartCoroutine(AwaitJoiningRoom());

            CommandDetailEvent commandDetailEvent = FindObjectOfType<CommandDetailEvent>();
            commandDetailEvent.OnCommandClickEvent.AddListener(
                id =>
                {
                    ICommand command = PhotonPlayerCommandCache.GetCachedCommands()[id];
                    commandDetailEvent.setCommandDetail.SetCommandName(command.Info.Name);
                    commandDetailEvent.setCommandDetail.SetCommandAttackRange(command.Info.EnhancedLevel);
                    commandDetailEvent.setCommandDetail.SetCommandDescription(command.Info.Explanation);
                });
        }

        private void Update()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                startButton.SetActive(true);
                readyButton.SetActive(false);
            }
            else
            {
                startButton.SetActive(false);
                readyButton.SetActive(true);
            }
        }

        private IEnumerator AwaitJoiningRoom()
        {
            yield return new WaitUntil(() => PhotonNetwork.NetworkingClient.State == ClientState.Joined);

            ResetPlayerArrays();
            UpdatePlayerNicknames();
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(
                new Hashtable
                {
                    { "isReady", PhotonNetwork.IsMasterClient ? 1 : 0 }
                });

            if (PhotonNetwork.IsMasterClient)
            {
                return;
            }

            int startTime = (int) PhotonNetwork.CurrentRoom.CustomProperties["Timer"];
            int delay = unchecked(PhotonNetwork.ServerTimestamp - startTime);

            Debug.LogFormat("Server delay is {0}ms", delay);
        }

        public void UpdatePlayerNicknames()
        {
            foreach ((int i, Player player) in PhotonNetwork.PlayerList.ToIndexed())
            {
                if (player.NickName != null)
                {
                    playerNicknames[i].text = player.NickName;

                    int isReady = (int) (player.CustomProperties.ContainsKey("isReady")
                        ? player.CustomProperties["isReady"]
                        : 0);

                    if (isReady == 1)
                    {
                        playerReady[i].text = "준비 완료!";
                        playerReady[i].color = ReadyGreen;
                    }
                    else
                    {
                        playerReady[i].text = "준비 안함";
                        playerReady[i].color = Color.white;
                    }

                    //만약에 해당 플레이어가 마스터 클라이언트(방장)이라면 레디고 뭐고 방장인걸 표시해야겠죠?
                    if (player.IsMasterClient)
                    {
                        playerReady[i].text = "준비 완료!";
                        playerReady[i].color = Color.white;
                        playerReady[i].gameObject.SetActive(false); // playerReady[i] 비활성화
                        Master[i].gameObject.SetActive(true); // Master[i] 활성화
                    }
                }
                //만약에 PhotonNetwork.PlayerList[i].NickName의 닉네임이 Null이라면 그 자리는 지금 비었다는 뜻이니까
                //else를 치게 될 경우 (없음)과 "--"를 치게 될거에요.
                else
                {
                    playerNicknames[i].text = "(없음)";
                    playerReady[i].text = "--";
                    playerReady[i].color = Color.white;
                }
            }
        }

        private void ResetPlayerArrays()
        {
            // 모든 텍스트 필드를 초기 상태로 리셋합니다.
            for (int i = 0; i < playerNicknames.Length; i++)
            {
                playerNicknames[i].text = "(없음)";
                playerReady[i].text = "--";
                playerReady[i].color = Color.white;
                Master[i].gameObject.SetActive(false);
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            ChatManager.Announce(newPlayer.NickName + "가 입장하였습니다.");
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.SetCustomProperties(
                    new Hashtable
                    {
                        { "C1", PhotonNetwork.CurrentRoom.PlayerCount }
                    });
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            ChatManager.Announce(otherPlayer.NickName + "가 퇴장하였습니다.");
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.SetCustomProperties(
                    new Hashtable
                    {
                        { "C1", PhotonNetwork.CurrentRoom.PlayerCount }
                    });
            }

            ResetPlayerArrays();
            UpdatePlayerNicknames();
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (changedProps.ContainsKey("isReady"))
            {
                ResetPlayerArrays();
                UpdatePlayerNicknames();
            }
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            LogRoomProperties();

            if (propertiesThatChanged.TryGetValue("C1", out object value))
            {
                int intValue = (int) value;
                if (intValue == 5)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        StartCoroutine(StartCountdownCoroutine());
                    }
                }
            }
        }

        public void LogRoomProperties()
        {
            StringBuilder builder = new StringBuilder();
            foreach ((object key, object value) in PhotonNetwork.CurrentRoom.CustomProperties)
            {
                builder.AppendLine($"{key}: {value}");
            }

            Debug.Log(builder.ToString());
        }

        public void OnReadyButtonClick()
        {
            Hashtable props = new Hashtable
            {
                { "isReady", 1 }
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }

        public bool CheckAllPlayersReady()
        {
            return true;
        }

        public void OnStartButtonClicked()
        {
            foreach (TextMeshProUGUI readyCheck in playerReady)
            {
                if (readyCheck.text == "준비 안함")
                {
                    ChatManager.Announce("모든 플레이어가 준비 상태가 아닙니다.");
                    return;
                }

                if (PhotonNetwork.PlayerList.Length < 2)
                {
                    ChatManager.Announce("1명이서는 게임이 불가능합니다.");
                    return;
                }
            }

            OnGameStart();
        }

        public void LeavePlayer()
        {
            Debug.Log("방에서 나갑니다.");
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "isReady", 0 } });
            PhotonNetwork.LeaveRoom();
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("GameLobby");
        }

        public void OnGameStart()
        {
            Debug.Log("게임을 시작합니다.");

            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.SetCustomProperties(
                    new Hashtable
                    {
                        { "C1", 5 }
                    });
            }
        }

        private IEnumerator StartCountdownCoroutine()
        {
            for (int i = 3; i > 0; i--)
            {
                ChatManager.Announce($"{i}초 후 게임을 시작합니다.");
                yield return new WaitForSeconds(1f);
            }

            PhotonNetwork.LoadLevel("GameScene");
        }
    }
}
