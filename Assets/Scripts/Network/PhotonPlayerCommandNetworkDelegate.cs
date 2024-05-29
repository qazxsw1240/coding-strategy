#nullable enable


using System.Collections.Generic;
using CodingStrategy.Utility;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace CodingStrategy.Network
{
    public class PhotonPlayerCommandNetworkDelegate : IPlayerCommandNetworkDelegate, IOnEventCallback
    {
        private static readonly IDictionary<string, int> CommandCountDictionary = new Dictionary<string, int>();

        public static IDictionary<string, int> GetCachedCommandCounts()
        {
            return CommandCountDictionary;
        }

        public static void AttachCommandIdCount(string id, int count)
        {
            CommandCountDictionary[id] = count;
        }

        public PhotonPlayerCommandNetworkDelegate()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private static bool IsPhotonCommandCode(byte code)
        {
            return code >= (byte) PhotonNetworkCodes.CommandRefreshRequest;
        }

        private static object[][] BuildCommandKeyValuePairs()
        {
            object[][] pairs = new object[CommandCountDictionary.Count][];
            foreach ((int index, (string key, int value)) in CommandCountDictionary.ToIndexed())
            {
                pairs[index] = new object[] { key, value };
            }

            return pairs;
        }

        private static object[] BuildModifyCommandKeyValuePair(string id, int count)
        {
            return new object[] { id, count };
        }

        private static void AttachCommandKeyValuePairs(object[][] pairs)
        {
            foreach (object[] pair in pairs)
            {
                string key = (string) pair[0];
                int value = (int) pair[1];
                CommandCountDictionary[key] = value;
            }
        }

        private static void AttachCommandKeyValuePair(object[] pair)
        {
            string key = (string) pair[0];
            int value = (int) pair[1];
            CommandCountDictionary[key] = value;
        }

        public void RequestRefresh()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("command refresh: {0}", "commandRefresh");
                return;
            }

            PhotonNetwork.RaiseEvent((byte) PhotonNetworkCodes.CommandRefreshRequest,
                null,
                new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient },
                SendOptions.SendReliable);
        }

        public void ModifyCommandCount(string id, int count)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                CommandCountDictionary[id] = count;
                return;
            }

            PhotonNetwork.RaiseEvent((byte) PhotonNetworkCodes.CommandRefreshRequest,
                BuildModifyCommandKeyValuePair(id, count),
                new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient },
                SendOptions.SendReliable);
        }

        public IDictionary<string, int> GetCachedCommandCount()
        {
            return CommandCountDictionary;
        }

        public void OnEvent(EventData photonEvent)
        {
            byte code = photonEvent.Code;

            if (!IsPhotonCommandCode(code))
            {
                return;
            }

            PhotonNetworkCodes codes = (PhotonNetworkCodes) code;

            switch (codes)
            {
                case PhotonNetworkCodes.CommandRefreshRequest:
                {
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        return;
                    }

                    PhotonNetwork.RaiseEvent((byte) PhotonNetworkCodes.CommandRefreshResponse,
                        BuildCommandKeyValuePairs(),
                        new RaiseEventOptions { Receivers = ReceiverGroup.All },
                        SendOptions.SendReliable);
                    return;
                }
                case PhotonNetworkCodes.CommandRefreshResponse:
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        // Prevents from duplicate handling
                        return;
                    }

                    AttachCommandKeyValuePairs((object[][]) photonEvent.CustomData);
                    Debug.Log("Command refreshed");
                    return;
                }
                case PhotonNetworkCodes.CommandModifyRequest:
                {
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        return;
                    }

                    object[] pair = (object[]) photonEvent.CustomData;
                    PhotonNetwork.RaiseEvent((byte) PhotonNetworkCodes.CommandModifyResponse,
                        pair,
                        new RaiseEventOptions { Receivers = ReceiverGroup.All },
                        SendOptions.SendReliable);
                    return;
                }
                case PhotonNetworkCodes.CommandModifyResponse:
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        // Prevents from duplicate handling
                        return;
                    }

                    AttachCommandKeyValuePair((object[]) photonEvent.CustomData);
                    Debug.Log("Command modified");
                    return;
                }
            }
        }
    }
}
