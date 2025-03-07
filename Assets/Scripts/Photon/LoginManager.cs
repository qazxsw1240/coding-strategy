using CodingStrategy.Sound;

using Photon.Pun;
using Photon.Realtime;

using TMPro;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CodingStrategy.Photon
{
    public class LoginManager : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private TextMeshProUGUI clientStateLabel;

        [SerializeField]
        private TMP_Text warningMessageLabel;

        [SerializeField]
        private TMP_Text loadingMessageLabel;

        [SerializeField]
        private UnityEvent onInvalidNicknameProvide;

        [SerializeField]
        private UnityEvent onValidConnectionCreate;

        [SerializeField]
        private TMP_InputField nicknameInputField;

        [SerializeField]
        private Button connectionButton;

        private string Nickname
        {
            get => nicknameInputField.text;
        }

        private string ClientStateMessage
        {
            get => clientStateLabel.text;
            set => clientStateLabel.text = value;
        }

        public void Awake()
        {
            SoundManager.Initialize();
            PhotonNetwork.NetworkingClient.StateChanged += UpdateState;

            AudioClip bgmClip = Resources.Load<AudioClip>("Sound/Game_Play_Ost");
            AudioClip typingSoundClip = Resources.Load<AudioClip>("Sound/Keyboard_Click_Sound");
            AudioClip buttonPressClip = Resources.Load<AudioClip>("Sound/Shop_Experience_Up");
            SoundManager.Instance.Play(bgmClip, SoundType.Bgm, 1.0f, 0.1f);
            nicknameInputField.onValueChanged.AddListener(
                _ =>
                    SoundManager.Instance.Play(typingSoundClip, SoundType.Effect, 3.0f, 0.6f));
            connectionButton.onClick.AddListener(
                () =>
                    SoundManager.Instance.Play(buttonPressClip));
        }

        public void Update()
        {
            // clientStateLabel.text = PhotonNetwork.NetworkClientState.ToString();
        }

        public void OnDestroy()
        {
            PhotonNetwork.NetworkingClient.StateChanged -= UpdateState;
            nicknameInputField.onValueChanged.RemoveAllListeners();
            connectionButton.onClick.RemoveAllListeners();
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
    }
}
