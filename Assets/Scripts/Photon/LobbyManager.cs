using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;
using static UnityEngine.Rendering.DebugUI;

public class LobbyManager : MonoBehaviour
{/*
    public GameObject RandomRoomDescribe;
    public GameObject StandardRoomDescribe;
    public TMP_Text RoomTitle;
    public TMP_Text UserCounts;
    public Button RandomEnterBtn;

    public void CreateRoom() => PhotonNetwork.CreateRoom(RoomTitle.text == "방 제목" ? "Room" + Random.Range(0, 100) : RoomTitle.text, new RoomOptions { MaxPlayers = 4 });

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    // 랜덤 참가 버튼을 눌렀을 때
    public void RandomRoomEnter()
    {
        StandardRoomDescribe.SetActive(false);
        RandomRoomDescribe.SetActive(true);
    }

    // 생성되어 있는 방을 눌렀을 때
    public void CreatedRoomEnter()
    {
        RandomRoomDescribe.SetActive(false);
        StandardRoomDescribe.SetActive(true);
    }

    // 게임시작 버튼을 눌렀을 때
    public void EnterGame()
    {

        SceneManager.LoadScene("GameScene");
    }


    void Start()
    {
        RandomEnterBtn.onClick.AddListener(RandomRoomEnter());
    }

    // Update is called once per frame
    void Update()
    {

    }*/
}
