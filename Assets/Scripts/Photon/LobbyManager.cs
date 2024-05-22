using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    //public GameObject RandomRoomDescribe;
    //public GameObject StandardRoomDescribe;
    //public TMP_Text RoomTitle;
    //public TMP_Text UserCounts;
    //public UnityEngine.UI.Button RandomEnterBtn;
    //
    //public void CreateRoom() => PhotonNetwork.CreateRoom(RoomTitle.text == "방 제목" ? "Room" + Random.Range(0, 100) : RoomTitle.text, new RoomOptions { MaxPlayers = 4 });
    //
    //public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();
    //
    //// 랜덤 참가 버튼을 눌렀을 때
    //public void RandomRoomEnter()
    //{
    //    StandardRoomDescribe.SetActive(false);
    //    RandomRoomDescribe.SetActive(true);
    //}
    //
    //// 생성되어 있는 방을 눌렀을 때
    //public void CreatedRoomEnter()
    //{
    //    RandomRoomDescribe.SetActive(false);
    //    StandardRoomDescribe.SetActive(true);
    //}
    //
    //// 게임시작 버튼을 눌렀을 때
    //public void EnterGame()
    //{
    //
    //    SceneManager.LoadScene("GameScene");
    //}
    //
    //
    //void Start()
    //{
    //    
    //}
    //
    //// Update is called once per frame
    //void Update()
    //{
    //
    //}

    public RawImage RandomImage;
    public TextMeshProUGUI RandomDescription;
    public RawImage StandardImage;
    public TextMeshProUGUI StandardDescription;
    public TextMeshProUGUI Nickname;
    public GameObject roomPrefab;
    public Transform contentRoomlist;



    //닉네임값 받아오기
    private void Start()
    {
        Nickname.text = PhotonNetwork.NickName;
        DontDestroyOnLoad(gameObject);
        PhotonNetwork.JoinLobby();
    }

    //버튼 클릭시 오브젝트 설명 변경. (standard 설명의 경우 false로 할당하여 room을 클릭했다가 random을 클릭할 경우를 고려하였음)
    public void OnRandomRoomButtonClick()
    {
        StandardImage.gameObject.SetActive(false);
        StandardDescription.gameObject.SetActive(false);
        RandomImage.gameObject.SetActive(true);
        RandomDescription.gameObject.SetActive(true);
    }


    // 버튼을 눌렀을 때, 실행되어있는 이미지가 random 이미지면 랜덤 방 참가
    // standard 이미지일 경우 해당 방의 ID를 받아와서 참가할 것입니다.
    public void OnJoinedRoomButtonClick()
    {
        if (RandomImage.gameObject.activeInHierarchy)
        {
            PhotonNetwork.JoinRandomRoom();
            SceneManager.LoadScene("GameRoom");
        }
        else if (StandardImage.gameObject.activeInHierarchy)
        {
            PhotonNetwork.JoinRoom(StandardDescription.text);
            SceneManager.LoadScene("GameRoom");
        }
    }

    //포톤은 고유한 ID를 방생성하면서 할당해줍니다. 그냥 생성만 해도 될 것 같습니다. 최대 인원 4명입니다.
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(null, roomOptions, null);
    }

    //OnRoomListUpdate 함수는 룸의 갯수가 변경될 때 마다 갱신해주는 함수입니다.
    //이 함수를 오버라이딩하여 작업하겠습니다.
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // 기존에 content 하위에 있는 모든 방 리스트 항목 제거
        foreach (Transform child in contentRoomlist)
        {
            Destroy(child.gameObject);
        }

        // 새로운 방 리스트로 항목 프리팹 생성합니다.
        foreach (RoomInfo room in roomList)
        {
            GameObject roomObj = Instantiate(roomPrefab, contentRoomlist);
            

            //프리팹 항목의 Roomname이라는 이름을 가진 textmeshPro 객체와 Usercount 라는 객체를 찾아옵니다.
            TextMeshProUGUI roomNameText = roomObj.transform.Find("RoomName").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI userCountText = roomObj.transform.Find("UserCount").GetComponent<TextMeshProUGUI>();

            //프리팹 항목의 Roomname에 방 고유 ID를 할당하고, Usercount는 이제 Usercount 대로 계산한 값을 보여주며 끝냅니다.
            roomNameText.text = room.Name;
            userCountText.text = $"{room.PlayerCount} / {room.MaxPlayers}";

            //그리고 해당 버튼 프리팹의 클릭 이벤트에 OnRoomClick 함수를 추가합니다.
            roomObj.GetComponent<Button>().onClick.AddListener(() => OnRoomClick(room.Name));
        }
    }


    //이 함수는 이미지를 변경할 것입니다.
    void OnRoomClick(string roomId)
    {
        RandomImage.gameObject.SetActive(false);
        RandomDescription.gameObject.SetActive(false);
        StandardImage.gameObject.SetActive(true);
        StandardDescription.gameObject.SetActive(true);
    }
}
