using CodingStrategy.Entities.Runtime;

using ExitGames.Client.Photon;

using Photon.Chat;
using Photon.Pun;

using TMPro;

using UnityEngine;

namespace CodingStrategy.Photon
{
    public class ChatManager : MonoBehaviour, IChatClientListener
    {
        private const string _announceChannel = "Announce";
        private static readonly string _roomChannel = "Room";

        [SerializeField]
        public TMP_InputField chatField;

        [SerializeField]
        public TMP_Text chatDisplay;

        [SerializeField]
        private string currentAnnounceChannel;

        [SerializeField]
        private string currentRoomChannel;

        [SerializeField]
        private string[] currentChannels;

        private readonly string privateReceiver = "";

        private ChatClient _chatClient;
        private bool _isConnected;
        private string currentChat;

        public void Awake()
        {
            if (!PhotonNetwork.NetworkingClient.InRoom)
            {
                throw new RuntimeException("Client must be in a room.");
            }

            _chatClient = new ChatClient(this) { ChatRegion = "kr" };
            currentAnnounceChannel = _announceChannel + PhotonNetwork.NetworkingClient.CurrentRoom;
            currentRoomChannel = _roomChannel + PhotonNetwork.NetworkingClient.CurrentRoom;
            currentChannels = new[] { currentAnnounceChannel, currentRoomChannel };
        }

        public void Start()
        {
            string appIdChat = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat;
            string appVersion = PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion;
            AuthenticationValues authenticationValues =
                new AuthenticationValues(PhotonNetwork.NetworkingClient.NickName);
            _chatClient.Connect(appIdChat, appVersion, authenticationValues);
        }

        public void Update()
        {
            if (_isConnected)
            {
                _chatClient.Service();
            }

            if (chatField.text != "" && Input.GetKey(KeyCode.Return))
            {
                SubmitPublicChatOnClick();
            }
        }

        public void DebugReturn(DebugLevel level, string message) {}

        public void OnChatStateChange(ChatState state)
        {
            if (state == ChatState.Uninitialized)
            {
                _isConnected = false;
            }
        }

        public void OnConnected()
        {
            Debug.Log("Connected");
            _isConnected = true;
            _chatClient.Subscribe(currentChannels);
        }

        public void OnDisconnected()
        {
            _isConnected = false;
            _chatClient.Unsubscribe(currentChannels);
        }

        public void OnGetMessages(string channelName, string[] senders, object[] messages)
        {
            for (int i = 0; i < senders.Length; i++)
            {
                string msgs = $"{(channelName == _announceChannel ? "시스템" : senders[i])}: {messages[i]}";
                chatDisplay.text += "\n" + msgs;
                Debug.Log(msgs);
            }
        }

        public void OnPrivateMessage(string sender, object message, string channelName) {}

        public void OnStatusUpdate(string user, int status, bool gotMessage, object message) {}

        public void OnSubscribed(string[] channels, bool[] results) {}

        public void OnUnsubscribed(string[] channels) {}

        public void OnUserSubscribed(string channel, string user) {}

        public void OnUserUnsubscribed(string channel, string user) {}

        public void Announce(string message)
        {
            _chatClient.PublishMessage(_announceChannel, message);
        }

        public void SubmitPublicChatOnClick()
        {
            if (privateReceiver == "")
            {
                _chatClient.PublishMessage(_roomChannel, currentChat);
                chatField.text = "";
                currentChat = "";
            }
        }

        public void TypeChatOnValueChange(string valueIn)
        {
            currentChat = valueIn;
        }
    }
}
