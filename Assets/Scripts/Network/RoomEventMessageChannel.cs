using System.Collections.Generic;

using CodingStrategy.Entities.Runtime;

using ExitGames.Client.Photon;

using Photon.Pun;
using Photon.Realtime;

using UnityEngine;
using UnityEngine.Rendering;

namespace CodingStrategy.Network
{
    [DisallowMultipleComponent]
    public class RoomEventMessageChannel : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        private const byte EventCode = 199;

        private static RoomEventMessageChannel _instance;

        public static RoomEventMessageChannel Create(Room room)
        {
            if (_instance)
            {
                Debug.LogFormat("Destroyed previous RoomEventMessageChannel instance {0}", _instance.gameObject.name);
                Destroy(_instance.gameObject);
            }
            string objectName = $"Room Event Message Channel-{room.Name}";
            GameObject gameObject = new GameObject(objectName, typeof(RoomEventMessageChannel));
            DontDestroyOnLoad(gameObject);
            _instance = gameObject.GetComponent<RoomEventMessageChannel>();
            Debug.LogFormat("Created Room event message channel for room {0}", room.Name);
            return _instance;
        }

        public static RoomEventMessageChannel Instance
        {
            get
            {
                if (!_instance)
                {
                    if (PhotonNetwork.InRoom)
                    {
                        return Create(PhotonNetwork.CurrentRoom);
                    }
                    throw new RuntimeException("RoomEventMessageChannel is not active");
                }
                return _instance;
            }
        }

        [SerializeReference]
        private IDictionary<string, List<string>> _messagePool =
            new SerializedDictionary<string, List<string>>();

        [SerializeReference]
        private IDictionary<string, AwaitableCompletionSource> _messageCompletionSources =
            new SerializedDictionary<string, AwaitableCompletionSource>();

        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code != EventCode)
            {
                return;
            }
            string message = (string) photonEvent.CustomData;
            Debug.LogFormat("Received Message: {0}", message);
            List<string> list = GetPlayerMessagePool(message);
            Player sender = PhotonNetwork.CurrentRoom.Players[photonEvent.Sender];
            if (list.Contains(sender.UserId))
            {
                return;
            }
            list.Add(sender.UserId);
            if (list.Count < PhotonNetwork.CurrentRoom.PlayerCount)
            {
                return;
            }
            AwaitableCompletionSource source = _messageCompletionSources[message];
            source.SetResult();
            RemovePlayerMessageCache(message);
        }

        private List<string> GetPlayerMessagePool(string message)
        {
            if (!_messagePool.TryGetValue(message, out List<string> list))
            {
                list = new List<string>();
                _messagePool.Add(message, list);
                _messageCompletionSources.TryAdd(message, new AwaitableCompletionSource());
            }
            return list;
        }

        private void RemovePlayerMessageCache(string message)
        {
            _messageCompletionSources.Remove(message);
            _messagePool.Remove(message);
        }

        public async Awaitable AwaitClientMessageAsync(string expectedMessage)
        {
            Debug.LogFormat("Send message: {0}", expectedMessage);
            RaiseEventOptions options = new RaiseEventOptions
            {
                Flags = WebFlags.Default,
                Receivers = ReceiverGroup.All
            };
            PhotonNetwork.RaiseEvent(EventCode, expectedMessage, options, SendOptions.SendReliable);
            if (_messageCompletionSources.TryGetValue(expectedMessage, out AwaitableCompletionSource source))
            {
                await source.Awaitable;
                return;
            }
            source = new AwaitableCompletionSource();
            _messagePool.Add(expectedMessage, new List<string>());
            _messageCompletionSources.Add(expectedMessage, source);
            await source.Awaitable;
        }

        public override void OnLeftRoom()
        {
            Destroy(gameObject);
            foreach (AwaitableCompletionSource source in _messageCompletionSources.Values)
            {
                source.SetCanceled();
            }
            _instance = null;
        }
    }
}
