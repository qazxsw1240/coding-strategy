using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using System.Text;
using CodingStrategy;
using CodingStrategy.Entities;
using CodingStrategy.Network;
using UnityEngine;
using Photon.Realtime;
using TMPro;
using CodingStrategy.Photon.Chat;
using CodingStrategy.UI.InGame;
using CodingStrategy.Utility;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

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

    private void Start()
    {
        GameInitializer.Initialize();

        StartCoroutine(AwaitJoiningRoom());

        CommandDetailEvent commandDetailEvent = FindObjectOfType<CommandDetailEvent>();
        commandDetailEvent.OnCommandClickEvent.AddListener(id =>
        {
            ICommand command = PhotonPlayerCommandCache.GetCachedCommands()[id];
            commandDetailEvent.setCommandDetail.SetCommandName(command.Info.Name);
            commandDetailEvent.setCommandDetail.SetCommandAttackRange(command.Info.EnhancedLevel);
            commandDetailEvent.setCommandDetail.SetCommandDescription(command.Info.Explanation);
        });
    }

    private IEnumerator AwaitJoiningRoom()
    {
        yield return new WaitUntil(() => PhotonNetwork.NetworkingClient.State == ClientState.Joined);

        ResetPlayerArrays();
        UpdatePlayerNicknames();
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable
        {
            { "isReady", PhotonNetwork.IsMasterClient ? 1 : 0 }
        });

        if (!PhotonNetwork.IsMasterClient)
        {
            int startTime = (int) PhotonNetwork.CurrentRoom.CustomProperties["Timer"];
            int delay = unchecked(PhotonNetwork.ServerTimestamp - startTime);

            Debug.LogFormat("Server delay is {0}ms", delay);
        }
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

    void Update()
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

    /* // 이거 전시때 못써먹을 기능이라 생각되기도 하고, 무리수인 기능이라 죽여버려달라는 요청이 있었습니다. 그냥 지워버려.
    public void LateUpdate()
    {
        // 모든 플레이어가 준비 상태인지 확인합니다.
        foreach (TextMeshProUGUI readyCheck in playerReady)
        {
           //한명이라도 준비 안함 상태라는 상태일 경우
            if (readyCheck.text == "준비 안함")
           {
                StopCoroutine(StartButtonCountdown());
                isCountdownStarted = false;
                return;
           }
            else //모두가 스타트 버튼을 누른 상태일 경우 StartButtonCountdown() 실행
            {
                if (PhotonNetwork.PlayerList.Length < 2)
                {
                    return;
                }

                if (!isCountdownStarted)
                {
                    ChatManager.Announce("모든 플레이어가 준비 완료되었습니다.");
                    isCountdownStarted = true; // 코루틴이 시작되었으므로 상태를 true로 설정합니다.
                }

                StartCoroutine(StartButtonCountdown());
                return;
            }
        }
    }
    */

    //유저들이 들어올 때 갱신해야겠죠?
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        ChatManager.Announce(newPlayer.NickName + "가 입장하였습니다.");
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable
            {
                { "C1", PhotonNetwork.CurrentRoom.PlayerCount }
            });
        }
    }

    //유저들이 나갈 때 갱신해야겠죠?
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ChatManager.Announce(otherPlayer.NickName + "가 퇴장하였습니다.");
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable
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

    //private IEnumerator StartButtonCountdown()
    //{
    //    //20초 기다렸다가...
    //    yield return new WaitForSeconds(20f);
    //
    //    // 방장을 강퇴하고 닉네임을 "없음"으로 변경
    //    //LeaveMasterPlayer();
    //    UpdatePlayerNicknames();
    //}

    public void OnReadyButtonClick()
    {
        Hashtable props = new Hashtable
        {
            { "isReady", 1 }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        // UpdatePlayerNicknames();
    }

    public bool CheckAllPlayersReady()
    {
        return true;
    }


    public void OnStartButtonClicked()
    {
        // 모든 플레이어가 준비 상태인지 확인합니다.
        foreach (TextMeshProUGUI readyCheck in playerReady)
        {
            if (readyCheck.text == "준비 안함")
            {
                ChatManager.Announce("모든 플레이어가 준비 상태가 아닙니다.");
                return;
            }
            else if (PhotonNetwork.PlayerList.Length < 2)
            {
                ChatManager.Announce("1명이서는 게임이 불가능합니다.");
                return;
            }
        }

        OnGameStart();
    }

    //public void LeaveMasterPlayer() 
    //{
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        Debug.Log("방장 잠수 강제 종료");
    //        PhotonNetwork.LeaveRoom();
    //        
    //        GameObject existingLobbyManager = GameObject.Find("LobbyManager");
    //        Destroy(existingLobbyManager);
    //        
    //        SceneManager.LoadScene("GameLobby");
    //    }
    //}

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
        // 카운트다운을 중단합니다.
        //StopCoroutine(StartButtonCountdown());

        // 게임을 시작합니다.
        Debug.Log("게임을 시작합니다.");
        // 게임을 시작하는 로직을 이곳 아래에 구현하세요.

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable
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
