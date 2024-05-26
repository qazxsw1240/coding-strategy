using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Linq;
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

    public GameObject existingLoginManager;

    // 갱신된 방 리스트를 저장해 둘 변수
    public static List<RoomInfo> cachedRoomList = new List<RoomInfo>();

    private void Awake()
    {
        LobbyNickname.text = PhotonNetwork.LocalPlayer.NickName;
        existingLoginManager = GameObject.Find("LoginManager");

        // PhotonNetwork.JoinLobby();
    }

    //닉네임값 받아오기
    private void Start()
    {
        // DontDestroyOnLoad(gameObject);
        // UpdateRoomListUI();
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
        if (!PhotonNetwork.InLobby)
        {
            return;
        }

        if (RandomImage.gameObject.activeInHierarchy)
        {
            PhotonNetwork.JoinRandomOrCreateRoom(roomOptions: new RoomOptions
            {
                MaxPlayers = 4,
                IsVisible = true,
                PublishUserId = true,
                CustomRoomProperties = new ExitGames.Client.Photon.Hashtable
                {
                    { "C0", "coding-strategy" },
                    { "C1", 0 }
                },
                CustomRoomPropertiesForLobby = new string[] { "C0", "C1" }
            });
        }
        else if (StandardImage.gameObject.activeInHierarchy)
        {
            PhotonNetwork.JoinRoom(RoomID);
        }
    }

    public override void OnJoinedLobby()
    {
        Debug.LogFormat("Connected to Master {0}", PhotonNetwork.CurrentLobby);

        Invoke(nameof(QueryRoomList), 5f);
        // PhotonNetwork.GetCustomRoomList(PhotonNetwork.CurrentLobby, "C0='coding-strategy' AND C1 < 4");
        // PhotonNetwork.JoinRandomOrCreateRoom(roomOptions: new RoomOptions
        // {
        //     MaxPlayers = 4,
        //     IsVisible = true,
        //     PublishUserId = true,
        //     CustomRoomProperties = new ExitGames.Client.Photon.Hashtable
        //     {
        //         { "C0", "coding-strategy" },
        //         { "C1", 0 }
        //     },
        //     CustomRoomPropertiesForLobby = new string[] { "C0", "C1" }
        // });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Successfully joined room");
        // Destroy(existingLoginManager);
        SceneManager.LoadScene("GameRoom");
        int currentPlayerCount = (int) PhotonNetwork.CurrentRoom.CustomProperties["C1"];
        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable
        {
            { "C1", (currentPlayerCount + 1) }
        });
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
        // 방 리스트가 갱신되면 cachedRoomList에 저장해 둡니다.
        cachedRoomList = roomList;
        UpdateRoomListUI();
    }

    private void QueryRoomList()
    {
        PhotonNetwork.GetCustomRoomList(PhotonNetwork.CurrentLobby, "C0='coding-strategy' AND C1 < 4");
    }

    public void UpdateRoomListUI()
    {
        // 사용자가 방 리스트를 갱신하라는 명령을 내렸을 때, cachedRoomList를 사용하여 UI를 갱신합니다.
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

    //이 함수는 이미지를 변경할 것입니다.
    void OnRoomClick(string roomId)
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
