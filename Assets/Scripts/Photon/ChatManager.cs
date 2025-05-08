using System;

using ExitGames.Client.Photon;

using Photon.Pun;
using Photon.Realtime;

namespace CodingStrategy.Photon
{
    public readonly struct ChatMessage
    {
        public ChatMessage(string sender, string message)
        {
            Sender = sender;
            Message = message;
        }

        public string Sender { get; }

        public string Message { get; }
    }

    public class ChatManager : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        private const byte EventCode = 198;

        public event Action<ChatMessage> OnChatMessageReceived;

        public void Send(string message)
        {
            if (!PhotonNetwork.InRoom || string.IsNullOrWhiteSpace(message))
            {
                return;
            }
            SendMessage(PhotonNetwork.LocalPlayer.NickName, message);
        }

        public void Announce(string message)
        {
            SendMessage("시스템", message);
        }

        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code != EventCode)
            {
                return;
            }
            ChatMessage chatMessage = ParseMessage(photonEvent.CustomData);
            OnChatMessageReceived?.Invoke(chatMessage);
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

        private static ChatMessage ParseMessage(object data)
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
            return new ChatMessage(sender, message);
        }
    }
}
