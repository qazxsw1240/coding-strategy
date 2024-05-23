using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class RoomManager : MonoBehaviourPunCallbacks
{
    // 방에 있는 플레이어 목록
    public List<Photon.Realtime.Player> playersInRoom = new List<Photon.Realtime.Player>();

    // 방장
    private Photon.Realtime.Player roomMaster;

    // 각 플레이어의 닉네임을 표시할 TextMeshProUGUI 배열
    public TextMeshProUGUI[] playerNicknames;

    // 각 플레이어의 준비 상태를 표시할 TextMeshProUGUI 배열
    public TextMeshProUGUI[] playerReadyChecks;

    public GameObject[] roomManagerText;

    // 시작 버튼
    public Button startButton;
    public Button readyButton;

    public void Start()
    {
        // 시작 버튼을 초기 상태로 설정합니다 (비활성화)
        startButton.gameObject.SetActive(false);
        Debug.Log(PhotonNetwork.LocalPlayer.NickName);
        Debug.Log(playersInRoom.Count);
        OnPlayerEnteredRoom(PhotonNetwork.LocalPlayer);
    }

    public void Update()
    {
        Debug.Log(playersInRoom.Count);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("Player entered room");

        // 새로 들어온 플레이어를 리스트에 추가합니다
        playersInRoom.Add(newPlayer);

        Debug.Log(playersInRoom.Count);

        // 플레이어 목록을 갱신합니다.
        UpdatePlayerList();

        // 방장을 설정합니다.
        if (roomMaster == null)
        {
            roomMaster = newPlayer;
            // 방장의 경우 자동으로 레디 상태를 "준비"로 설정합니다.
            playerReadyChecks[playersInRoom.IndexOf(roomMaster)].text = "준비";
            // 방장만 시작 버튼을 활성화합니다.
            if (PhotonNetwork.LocalPlayer == roomMaster)
            {
                // 방장에게 할당된 readyState TextMeshPro를 비활성화합니다.
                playerReadyChecks[playersInRoom.IndexOf(roomMaster)].gameObject.SetActive(false);

                // RoomManagerText TextMeshPro를 활성화합니다.
                // 이 부분에서는 RoomManagerText의 인스턴스가 필요합니다.
                roomManagerText[playersInRoom.IndexOf(roomMaster)].gameObject.SetActive(true);
            }
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        // 방을 나간 플레이어를 리스트에서 제거합니다
        playersInRoom.Remove(otherPlayer);

        // 방장이 방을 나간 경우 다음 플레이어를 방장으로 지정합니다
        if (roomMaster == otherPlayer && playersInRoom.Count > 0)
        {
            // 그럼 다음으로 먼저 들어온 사람이 방장이 됩니다.
            roomMaster = playersInRoom[0];

            playerReadyChecks[playersInRoom.IndexOf(roomMaster)].text = "준비";

            if (PhotonNetwork.LocalPlayer == roomMaster)
            {
                // 방장에게 할당된 readyState TextMeshPro를 비활성화합니다.
                playerReadyChecks[playersInRoom.IndexOf(roomMaster)].gameObject.SetActive(false);

                // RoomManagerText TextMeshPro를 활성화합니다.
                // 이 부분에서는 RoomManagerText의 인스턴스가 필요합니다.
                roomManagerText[playersInRoom.IndexOf(roomMaster)].gameObject.SetActive(true);
            }
        }

        // 플레이어 목록을 갱신합니다.
        UpdatePlayerList();
    }

    public void UpdatePlayerList()
    {
        // 모든 플레이어의 정보를 초기화합니다
        for (int i = 0; i < playerNicknames.Length; i++)
        {
            playerNicknames[i].text = "없음";
            playerReadyChecks[i].text = "--";
        }

        // 현재 방에 있는 플레이어의 정보를 표시합니다
        for (int i = 0; i < playersInRoom.Count; i++)
        {
            playerNicknames[i].text = playersInRoom[i].NickName;
            playerReadyChecks[i].text = "대기";
        }

        // 방장에게는 'start button'을, 그 외의 참가자들에게는 'ready button'을 보여줍니다.
        if (PhotonNetwork.LocalPlayer == roomMaster)
        {
            startButton.gameObject.SetActive(true);
            readyButton.gameObject.SetActive(false); // ready button 프리팹을 비활성화합니다.
        }
        else
        {
            startButton.gameObject.SetActive(false);
            readyButton.gameObject.SetActive(true); // ready button 프리팹을 활성화합니다.
        }
    }

    public void OnReadyButtonClicked()
    {
        // 자신의 정보를 가져옵니다.
        Photon.Realtime.Player localPlayer = PhotonNetwork.LocalPlayer;

        // 현재 레디 상태를 가져옵니다.
        bool currentReadyStatus = (bool)localPlayer.CustomProperties["isReady"];

        // 레디 상태를 반전시킵니다.
        bool newReadyStatus = !currentReadyStatus;

        // 새로운 레디 상태를 CustomProperties에 저장합니다.
        localPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "isReady", newReadyStatus } });

        // UI를 업데이트합니다.
        string statusText = newReadyStatus ? "준비" : "대기";
        playerReadyChecks[playersInRoom.IndexOf(localPlayer)].text = statusText;
    }

    public void OnStartButtonClicked()
    {
        // 모든 플레이어가 준비 상태인지 확인합니다.
        foreach (TextMeshProUGUI readyCheck in playerReadyChecks)
        {
            if (readyCheck.text != "준비")
            {
                Debug.Log("모든 플레이어가 준비 상태가 아닙니다.");
                return;
            }
        }
        OnGameStart();
    }

    private IEnumerator KickMasterAfterCountdown()
    {
        yield return new WaitForSeconds(20);

        // 방장이 게임을 시작하지 않았다면 방장을 강퇴합니다.
        if (roomMaster == PhotonNetwork.LocalPlayer)
        {
            Debug.Log("게임을 시작하지 않아 방장이 강퇴당하였습니다.");
            PhotonNetwork.LeaveRoom();
        }
    }

    public void KickMasterAllOnReadyButtonClicked()
    {
        // 모든 플레이어가 준비 상태인지 확인합니다.
        foreach (TextMeshProUGUI readyCheck in playerReadyChecks)
        {
            if (readyCheck.text != "준비")
            {
                Debug.Log("모든 플레이어가 준비 상태가 아닙니다.");
                return;
            }
        }

        // 모든 플레이어가 준비 상태라면 20초의 카운트다운을 시작합니다.
        StartCoroutine(KickMasterAfterCountdown());
    }

    public void OnGameStart()
    {
        // 카운트다운을 중단합니다.
        StopCoroutine(KickMasterAfterCountdown());

        // 게임을 시작합니다.
        Debug.Log("게임을 시작합니다.");
        // 게임을 시작하는 로직을 이곳에 구현하세요.
    }
}
