using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using CodingStrategy.PlayerStates;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI[] playerNicknames; // 플레이어 닉네임 텍스트 배열
    public TextMeshProUGUI[] playerReady; // 플레이어 닉네임 텍스트 배열
    public TextMeshProUGUI[] Master;
    
    public bool[] ready = new bool[3];

    public GameObject startButton; // 시작 버튼
    public GameObject readyButton; // 레디

    private void Start()
    {
        InvokeRepeating("UpdatePlayerNicknames", 1f, 1f);
    }

    public void UpdatePlayerNicknames()
    {
        //Debug.Log("리로드");
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            //playerNicknames의 0번째에 playerStates 클래스 내에 정의된 playersinRoom의 i번째의 유저의 닉네임을 가져옵니다.
            //근데 PlayerList[i]의 값이 Null이 아니야! >> 그러면 이제 그 위치의 닉네임 변경해주는겁니다.
            if (PhotonNetwork.PlayerList[i].NickName!=null)
            { 
                playerNicknames[i].text = PhotonNetwork.PlayerList[i].NickName;
                
                // 그리고 그 PlayerList의 customProperties의 Hashtage "isReady"의 값을 받아와서, 그게 true일 경우 "준비 완료!"를 띄울겁니다.
                bool isReady = PhotonNetwork.PlayerList[i].CustomProperties.ContainsKey("isReady")
                       && (bool)PhotonNetwork.PlayerList[i].CustomProperties["isReady"];

                if (isReady)
                {
                    playerReady[i].text = "준비 완료!";
                    playerReady[i].color = Color.green;
                }
                else
                {
                    playerReady[i].text = "준비 안함";
                }

                //만약에 해당 플레이어가 마스터 클라이언트(방장)이라면 레디고 뭐고 방장인걸 표시해야겠죠?
                if (PhotonNetwork.PlayerList[i].IsMasterClient)
                {
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
            }
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

    private void ResetPlayerArrays()
    {
        // 모든 텍스트 필드를 초기 상태로 리셋합니다.
        for (int i = 0; i < playerNicknames.Length; i++)
        {
            playerNicknames[i].text = "(없음)";
            playerReady[i].text = "--";
            playerReady[i].color = Color.black;
            Master[i].gameObject.SetActive(false);
        }
    }

    public void LateUpdate()
    {
        // 모든 플레이어가 준비 상태인지 확인합니다.
        foreach (TextMeshProUGUI readyCheck in playerReady)
        {
           //한명이라도 준비 안함 상태라는 상태일 경우
            if (readyCheck.text == "준비 안함")
           {
                StopCoroutine(StartButtonCountdown()); 
                return;
           }
            else //모두가 스타트 버튼을 누른 상태일 경우 StartButtonCountdown() 실행
            {
                StartCoroutine(StartButtonCountdown());
                return;
            }
        }
    }

    //유저들이 들어올 때 갱신해야겠죠?
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        UpdatePlayerNicknames();
    }

    //유저들이 나갈 때 갱신해야겠죠?
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        ResetPlayerArrays();
        UpdatePlayerNicknames();
    }

    private IEnumerator StartButtonCountdown()
    {
        //20초 기다렸다가...
        yield return new WaitForSeconds(20f);

        // 방장을 강퇴하고 닉네임을 "없음"으로 변경
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CloseConnection(PhotonNetwork.LocalPlayer);
            UpdatePlayerNicknames();
        }
    }
    
    public void OnReadyButtonClick()
    {
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
            {
                { "isReady", true }
            };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        UpdatePlayerNicknames();
    }
    

    public void OnStartButtonClicked()
    {
        // 모든 플레이어가 준비 상태인지 확인합니다.
        foreach (TextMeshProUGUI readyCheck in playerReady)
        {
            if (readyCheck.text == "준비 안함")
            {
                Debug.Log("모든 플레이어가 준비 상태가 아닙니다.");
                return;
            }
            else if (PhotonNetwork.PlayerList.Length < 2)
            {
                Debug.Log("1명이서는 게임이 불가능합니다.");
                return;
            }
        }
        OnGameStart();
    }

    public void OnGameStart()
    {
        // 카운트다운을 중단합니다.
        StopCoroutine(StartButtonCountdown());

        // 게임을 시작합니다.
        Debug.Log("게임을 시작합니다.");
        // 게임을 시작하는 로직을 이곳 아래에 구현하세요.
    }
}
