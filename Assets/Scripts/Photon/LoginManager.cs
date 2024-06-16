using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Events;

public class LoginManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button startButton;
    [SerializeField] private TextMeshProUGUI clientStateLabel;

    [SerializeField] private TMP_InputField nicknameInputField;
    [SerializeField] private TMP_Text warningMessageLabel;
    [SerializeField] private TMP_Text loadingMessageLabel;

    [SerializeField] private UnityEvent onInvalidNicknameProvide;
    [SerializeField] private UnityEvent onValidConnectionCreate;

    private string Nickname => nicknameInputField.text;

    private string ClientStateMessage
    {
        get => clientStateLabel.text;
        set => clientStateLabel.text = value;
    }

    public void Awake()
    {
        PhotonNetwork.NetworkingClient.StateChanged += UpdateState;
    }

    public void Update()
    {
        clientStateLabel.text = PhotonNetwork.NetworkClientState.ToString();
    }

    public void OnDestroy()
    {
        PhotonNetwork.NetworkingClient.StateChanged -= UpdateState;
    }

    public override void OnConnectedToMaster()
    {
        TypedLobby lobby = new TypedLobby("coding-strategy", LobbyType.SqlLobby);
        PhotonNetwork.JoinLobby(lobby);
        SceneManager.LoadScene("GameLobby");
    }

    public void CreateConnection()
    {
        if (PhotonNetwork.NetworkingClient.State != ClientState.PeerCreated)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(Nickname))
        {
            onInvalidNicknameProvide.Invoke();
            return;
        }

        PhotonNetwork.LocalPlayer.NickName = Nickname;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.PhotonServerSettings.AppSettings.EnableLobbyStatistics = false;
        PhotonNetwork.NetworkingClient.EnableLobbyStatistics = false;
        PhotonNetwork.IsMessageQueueRunning = true;
        PhotonNetwork.ConnectUsingSettings();

        onValidConnectionCreate.Invoke();
    }

    private void UpdateState(ClientState previous, ClientState next)
    {
        ClientStateMessage = next.ToString();
    }

    [Obsolete]
    public void OnStartButtonClick()
    {
        if (PhotonNetwork.NetworkingClient.State != ClientState.PeerCreated)
        {
            return;
        }

        if (string.IsNullOrEmpty(nicknameInputField.text))
        {
            warningMessageLabel.gameObject.SetActive(true);
            return;
        }

        PhotonNetwork.LocalPlayer.NickName = nicknameInputField.text;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.PhotonServerSettings.AppSettings.EnableLobbyStatistics = false;
        PhotonNetwork.NetworkingClient.EnableLobbyStatistics = false;
        PhotonNetwork.IsMessageQueueRunning = true;
        PhotonNetwork.ConnectUsingSettings();

        startButton.gameObject.SetActive(false);
        nicknameInputField.gameObject.SetActive(false);
        loadingMessageLabel.gameObject.SetActive(true);
    }
}
