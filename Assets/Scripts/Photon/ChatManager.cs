using System;
using System.Collections;

using ExitGames.Client.Photon;

using Photon.Chat;
using Photon.Pun;
using Photon.Realtime;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace CodingStrategy.Photon
{
    public class ChatManager : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        private const byte EventCode = 198;

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

        [Obsolete]
        private readonly string privateReceiver = "";

        [Obsolete]
        private ChatClient _chatClient;

        private bool _isConnected;
        private string currentChat;

        public void Awake()
        {
            if (!chatField)
            {
                throw new UnassignedReferenceException(nameof(chatField));
            }
            if (!chatDisplay)
            {
                throw new UnassignedReferenceException(nameof(chatDisplay));
            }
            chatField.onValueChanged.AddListener(TypeChatOnValueChange);
            Button button = gameObject.GetComponentInChildren<Button>();
            if (!button)
            {
                throw new UnassignedReferenceException(nameof(button));
            }
            button.onClick.AddListener(SubmitPublicChatOnClick);
        }

        public IEnumerator Start()
        {
            yield return new WaitUntil(() => !PhotonNetwork.InRoom);

            // _chatClient = new ChatClient(this) { ChatRegion = "kr" };
            // currentAnnounceChannel = _announceChannel + PhotonNetwork.NetworkingClient.CurrentRoom;
            // currentRoomChannel = _roomChannel + PhotonNetwork.NetworkingClient.CurrentRoom;
            // currentChannels = new[] { currentAnnounceChannel, currentRoomChannel };
            // string appIdChat = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat;
            // string appVersion = PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion;
            // AuthenticationValues authenticationValues =
            //     new AuthenticationValues(PhotonNetwork.NetworkingClient.NickName);
            // _chatClient.Connect(appIdChat, appVersion, authenticationValues);
        }

        public void Update()
        {
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

        public void Announce(string message)
        {
            // _chatClient.PublishMessage(_announceChannel, message);
            SendMessage("시스템", message);
        }

        public void SubmitPublicChatOnClick()
        {
            // if (privateReceiver == "")
            // {
            //     // _chatClient.PublishMessage(_roomChannel, currentChat);
            // }
            if (!PhotonNetwork.InRoom)
            {
                return;
            }
            SendMessage(PhotonNetwork.LocalPlayer.NickName, currentChat);
            chatField.text = "";
            currentChat = "";
        }

        public void TypeChatOnValueChange(string valueIn)
        {
            currentChat = valueIn;
        }

        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code != EventCode)
            {
                return;
            }
            (string sender, string message) = ParseMessage(photonEvent.CustomData);
            Debug.LogFormat("Received Message from {1}: {0}", message, sender);
            string chat = $"{sender}: {message}";
            chatDisplay.text += "\n" + chat;
        }

        private static void SendMessage(string sender, string message)
        {
            PhotonNetwork.RaiseEvent(
                EventCode,
                new object[] { sender, message },
                new RaiseEventOptions
                {
                    Flags = WebFlags.Default,
                    Receivers = ReceiverGroup.All
                },
                SendOptions.SendReliable);
        }

        private static (string, string) ParseMessage(object data)
        {
            if (data is not object[] tuple)
            {
                throw new ArgumentException();
            }
            if (tuple.Length != 2)
            {
                throw new ArgumentException();
            }
            if (tuple[0] is not string sender || tuple[1] is not string message)
            {
                throw new ArgumentException();
            }
            return (sender, message);
        }
    }
}
