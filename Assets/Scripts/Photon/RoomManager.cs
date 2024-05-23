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

    public PlayerStates playerStates;

    public GameObject startButton; // 시작 버튼
    public GameObject readyButton; // 레디 버튼

    private void Start()
    {
        GameObject playerInfo = GameObject.Find("PlayerInfo"); // 저 Lobby에서 destroy on load를 통해 갓 온 따끈따끈한 "PlayerInfo"라는 이름의 오브젝트 찾기
        playerStates = playerInfo.GetComponent<PlayerStates>(); // PlayerStates 컴포넌트 찾기
        
        UpdatePlayerNicknames();
        
    }

    public void UpdatePlayerNicknames()
    {
        for (int i = 0; i < playerStates.playersinRoom.Count; i++)
        {
            //playerNicknames의 0번째에 playerStates 클래스 내에 정의된 playersinRoom의 i번째의 유저의 닉네임을 가져옵니다.
            if(playerStates.playersinRoom[i].NickName!=null)
            { playerNicknames[i].text = PhotonNetwork.PlayerList[i].NickName; }
            
            
            //만약에 해당 플레이어가 마스터 클라이언트(방장)이라면
            if (playerStates.playersinRoom[i].IsMasterClient)
            {
                playerReady[i].gameObject.SetActive(false); // playerReady[i] 비활성화
                Master[i].gameObject.SetActive(true); // Master[i] 활성화
            }

            if (playerStates.ready[i] == true)
            {
                playerReady[i].text = "준비 완료!";
                playerReady[i].color = Color.green;
            }
            else if (playerStates.ready[i] == null)
            {
                playerReady[i].text = "없음";
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

    private void FixedUpdate()
    {
        //StartCoroutine(StartButtonCountdown());
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
        UpdatePlayerNicknames();
    }

    private IEnumerator StartButtonCountdown()
    {
        // 모든 플레이어가 준비 상태인지 확인
        foreach (TextMeshProUGUI playerStatus in playerReady)
        {
            if (playerStatus.text != "준비 완료!" && playerStatus.text != "없음") 
            {
                yield break; // 모든 플레이어가 준비 상태가 아니면 코루틴 종료
            }
        }

        //20초 기다렸다가...
        yield return new WaitForSeconds(20f);

        // 방장을 강퇴하고 닉네임을 "없음"으로 변경
        if (PhotonNetwork.IsMasterClient)
        {
            playerStates.playersinRoom.Remove(PhotonNetwork.LocalPlayer);
            UpdatePlayerNicknames();
        }
    }

    public void OnReadyButtonClick()
    {
        for (int i = 0; i < playerNicknames.Length; i++)
        {
            if (playerNicknames[i].text == PhotonNetwork.LocalPlayer.NickName)
            {
                playerReady[i].text = "준비 완료!";
                playerReady[i].color = Color.green;
                playerStates.ready[i] = true;
                break;
            }
        }
        UpdatePlayerNicknames();
    }

    public void OnStartButtonClicked()
    {
        // 모든 플레이어가 준비 상태인지 확인합니다.
        foreach (TextMeshProUGUI readyCheck in playerReady)
        {
            if (readyCheck.text != "준비 완료!" && readyCheck.text !="없음")
            {
                Debug.Log("모든 플레이어가 준비 상태가 아닙니다.");
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
