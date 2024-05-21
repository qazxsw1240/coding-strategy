using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoginManager : MonoBehaviourPunCallbacks
{
    public Button startButton;
    public TextMeshProUGUI connectionStatusText;
    public InputField nicknameInputField;

    void Start()
    {
        startButton.onClick.AddListener(OnStartButtonClick);
    }

    void Update()
    {
        connectionStatusText.text = PhotonNetwork.NetworkClientState.ToString();
    }

    public void OnStartButtonClick()
    {
        PhotonNetwork.NickName = nicknameInputField.text;
        PhotonNetwork.ConnectUsingSettings();
        DontDestroyOnLoad(gameObject);
    }

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("GameLobby");
        Debug.Log(PhotonNetwork.NickName + "님 환영합니다.");
    }
}
