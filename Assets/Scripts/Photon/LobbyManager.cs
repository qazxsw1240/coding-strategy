using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


public class LobbyManager : MonoBehaviourPunCallbacks
{
    /*
    public Button joinRoomButton;
    public TextMeshProUGUI nicknameText;
    public RawImage randomImage;
    public GameObject roomEnterScroll; // 방 목록을 보여주는 스크롤 뷰
    public GameObject roomEntryPrefab; // 방 항목을 표현하는 프리팹
    public RoomInfo[] rooms;

    void Start()
    {
        // 닉네임 설정
        nicknameText.text = PhotonNetwork.NickName;

        // 버튼 이벤트 설정
        joinRoomButton.onClick.AddListener(OnJoinRoomButtonClick);
        randomRoomButton.onClick.AddListener(OnRandomRoomButtonClick);

        randomImage.gameObject.SetActive(false); // 초기 상태는 비활성화
    }

    public void OnJoinRoomButtonClick()
    {
        // 현재 활성화된 방에 입장
        foreach (RoomEntry entry in roomEnterScroll.GetComponentsInChildren<RoomEntry>())
        {
            // 만약 "Standard" 이미지가 활성화된 방이 있다면 해당 방에 입장합니다.
            if (entry.standardImage.isActiveAndEnabled)
            {
                PhotonNetwork.JoinRoom(entry.roomInfo.Name);
                break;
            }
        }
    }

    // 방 목록이 업데이트될 때 호출되는 콜백 
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        rooms = roomList.ToArray();

        // 방 목록 UI를 업데이트합니다.
        foreach (Transform child in roomEnterScroll.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var room in rooms)
        {
            GameObject roomEntryObject = Instantiate(roomEntryPrefab, roomEnterScroll.transform);
            RoomEntry roomEntry = roomEntryObject.GetComponent<RoomEntry>();
            roomEntry.SetRoomInfo(room);
        }
    }

    // 랜덤 방 참여에 실패했을 때 호출되는 콜백
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // 새로운 방을 생성합니다.
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4; // 방의 최대 참여자를 4명으로 설정
        PhotonNetwork.CreateRoom(null, roomOptions, TypedLobby.Default);
    }

    // 방에 입장했을 때 호출되는 콜백
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        // GameRoomScene으로 전환
        SceneManager.LoadScene("GameRoomScene");
    }

    // 다른 플레이어가 방을 나갔을 때 호출되는 콜백
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // 다른 플레이어가 방을 나갔을 때 그 방이 비어 있는지 확인하고, 비어 있다면 그 방을 파괴합니다.
        foreach (RoomEntry entry in roomEnterScroll.GetComponentsInChildren<RoomEntry>())
        {
            entry.CheckDestroyRoom();
        }
    }
    */
}