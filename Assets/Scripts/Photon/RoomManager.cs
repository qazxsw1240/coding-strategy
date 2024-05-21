using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [Header("DisconnectPanel")]
    public InputField NickNameInput;

    [Header("LobbyPanel")]
    public GameObject LobbyPanel;


    [Header("ETC")]
    public Text StatusText;

    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    private void Update()
    {
        StatusText.text = PhotonNetwork.NetworkClientState.ToString();
    }
    //// Start is called before the first frame update
    //void Start()
    //{
    //    
    //}
    //
    //private string nickname = "Unnamed";
    //
    //public void ChangeNickname(string _name)
    //{
    //    nickname = _name;
    //}
    //
    //public void JoinRoomButtonPressed() 
    //{
    //    Debug.Log("Connecting...");
    //    PhotonNetwork.ConnectUsingSettings();
    //}
    //
    //public override void OnConnectedToMaster()
    //{
    //    base.OnConnectedToMaster();
    //    Debug.Log("Connected to Server");
    //
    //    PhotonNetwork.JoinLobby();
    //}
    //
    //public override void OnJoinedLobby()
    //{
    //    base.OnJoinedLobby();
    //
    //    PhotonNetwork.JoinOrCreateRoom("test", null, null);
    //}
}
