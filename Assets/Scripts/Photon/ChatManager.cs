using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CodingStrategy.Photon.Chat
{
    public class ChatManager : MonoBehaviour, IChatClientListener
    {
        #region Setup

        ChatClient chatClient;
        bool isConnected;

        #endregion Setup

        #region General

        string privateReceiver = "";
        string currentChat;
        [SerializeField] TMP_InputField chatField;
        [SerializeField] TMP_Text chatDisplay;
        public static string announceChannel = "Announce";
        public static string roomChannel = "Room";

        // Start is called before the first frame update
        void Start()
        {
            isConnected = true;
            chatClient = new ChatClient(this);
            chatClient.ChatRegion = "kr";
            chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(PhotonNetwork.NickName));
            announceChannel += PhotonNetwork.CurrentRoom.Name;
            roomChannel += PhotonNetwork.CurrentRoom.Name;
            Debug.Log("Connecting");
        }

        // Update is called once per frame
        void Update()
        {
            if (isConnected)
            {
                chatClient.Service();
            }

            if (chatField.text != "" && Input.GetKey(KeyCode.Return))
            {
                SubmitPublicChatOnClick();
            }
        }

        #endregion General

        #region Announce

        public void Announce(string message)
        {
            chatClient.PublishMessage(announceChannel, message);
        }

        #endregion Announce

        #region PublicChat

        public void SubmitPublicChatOnClick()
        {
            if (privateReceiver == "")
            {
                chatClient.PublishMessage(roomChannel, currentChat);
                chatField.text = "";
                currentChat = "";
            }
        }

        public void TypeChatOnValueChange(string valueIn)
        {
            currentChat = valueIn;
        }

        #endregion PublicChat

        #region PrivateChat
        public void ReceiverOnValueChange(string valueIn)
        {
            privateReceiver = valueIn;
        }
        public void SubmitPrivateChatOnClick()
        {
            if (privateReceiver != "")
            {
                chatClient.SendPrivateMessage(privateReceiver, currentChat);
                chatField.text = "";
                currentChat = "";
            }
        }
        #endregion PrivateChat

        #region Callbacks

        public void DebugReturn(DebugLevel level, string message)
        {
            //throw new System.NotImplementedException();
        }

        public void OnChatStateChange(ChatState state)
        {
            if (state == ChatState.Uninitialized)
            {
                isConnected = false;
            }
        }

        public void OnConnected()
        {
            Debug.Log("Connected");
            chatClient.Subscribe(new string[] { roomChannel, announceChannel });
        }

        public void OnDisconnected()
        {
            isConnected = false;
        }

        public void OnGetMessages(string channelName, string[] senders, object[] messages)
        {
            string msgs = "";
            for (int i = 0; i < senders.Length; i++)
            {
                msgs = string.Format("{0}: {1}", channelName == announceChannel ? "시스템" : senders[i], messages[i]);
                chatDisplay.text += "\n" + msgs;

                Debug.Log(msgs);
            }
        }

        public void OnPrivateMessage(string sender, object message, string channelName)
        {
            string msgs = "";

            msgs = string.Format("(Private) {0}: {1}", sender, message);

            chatDisplay.text += "\n " + msgs;

            Debug.Log(msgs);

        }

        public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
        {
            throw new System.NotImplementedException();
        }

        public void OnSubscribed(string[] channels, bool[] results)
        {

        }

        public void OnUnsubscribed(string[] channels)
        {
            throw new System.NotImplementedException();
        }

        public void OnUserSubscribed(string channel, string user)
        {
            throw new System.NotImplementedException();
        }

        public void OnUserUnsubscribed(string channel, string user)
        {
            throw new System.NotImplementedException();
        }

        #endregion Callbacks
    }
}