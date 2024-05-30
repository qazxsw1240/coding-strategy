using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace CodingStrategy
{
    public class InGameStatusSynchronizer : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        private const byte StateSynchronizationRequestCode = 100;
        private const byte StateSynchronizationResponseCode = 101;
        private const byte StateSynchronizationRequestSignalCode = 102;

        private readonly HashSet<int> _synchronizedStatusRequests = new HashSet<int>();
        private readonly HashSet<int> _synchronizedStatusNextRequests = new HashSet<int>();

        private string _expectedStatus;
        private string _actualStatus;
        private string[] _nextStatus;

        private readonly ConcurrentQueue<Action> _actions = new ConcurrentQueue<Action>();

        public void Update()
        {
            if (_actions.TryDequeue(out Action action))
            {
                if (!PhotonNetwork.InRoom)
                {
                    return;
                }

                action();
            }
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (changedProps.TryGetValue("message", out object message))
            {
                string messageValue = (string) message;
                Debug.Log($"Player {targetPlayer.IsMasterClient}" + messageValue);
            }
        }

        public IEnumerator AwaitAllPlayersStatus(string status, params string[] nextStatus)
        {
            yield return new WaitUntil(() => _expectedStatus == null);

            _expectedStatus = status;
            _nextStatus = nextStatus;
            Debug.LogFormat("expect status {0}, next status {1}", status, nextStatus);

            _actions.Enqueue(() =>
            {
                Debug.LogFormat("Request signal {0}", _expectedStatus);
                PhotonNetwork.RaiseEvent(StateSynchronizationRequestSignalCode, _expectedStatus,
                    new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient }, SendOptions.SendReliable);
            });

            // if (PhotonNetwork.IsMasterClient)
            // {
            //     _currentStatusRequest = status;
            //     _actions.Enqueue(() =>
            //     {
            //         PhotonNetwork.RaiseEvent(StateSynchronizationRequestCode, _expectedStatus,
            //             new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
            //     });
            // }

            // _actions.Enqueue(() =>
            // {
            //     Debug.LogFormat("Request synchronization for status {0}", _expectedStatus);
            //     PhotonNetwork.RaiseEvent(StateSynchronizationRequestCode, _expectedStatus,
            //         new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient }, SendOptions.SendReliable);
            // });
            // {
            //     if (PhotonNetwork.IsMasterClient)
            //     {
            //         _actions.Enqueue(() =>
            //         {
            //             Debug.LogFormat("Request synchronization for status {0} as Master client", _expectedStatus);
            //             PhotonNetwork.RaiseEvent(StateSynchronizationRequestCode, _expectedStatus,
            //                 new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient }, SendOptions.SendReliable);
            //         });
            //     }
            // }

                yield return new WaitUntil(() =>
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        if (_synchronizedStatusRequests.Count + _synchronizedStatusNextRequests.Count >=
                            PhotonNetwork.CurrentRoom.PlayerCount)
                        {
                            _synchronizedStatusNextRequests.Clear();
                            _synchronizedStatusRequests.Clear();
                            PhotonNetwork.RaiseEvent(StateSynchronizationRequestCode, _expectedStatus,
                                new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
                            return true;
                        }
                    }

                    if (_expectedStatus == null || _actualStatus == null)
                    {
                        return false;
                    }

                    // Debug.LogFormat("Synchronized for {0}:  {1}", _expectedStatus, _expectedStatus == _actualStatus);
                    return _expectedStatus == _actualStatus;
                });

                if (PhotonNetwork.IsMasterClient)
                {
                    yield return new WaitUntil(() => _expectedStatus == _actualStatus);
                }

                Debug.LogFormat("Synchronized");
                _expectedStatus = null;
                _actualStatus = null;
                yield return null;
        }


        public void OnEvent(EventData photonEvent)
        {
            byte eventCode = photonEvent.Code;

            Debug.LogErrorFormat("event code: {0}, {1}, {2}", eventCode, photonEvent.CustomData,  _nextStatus);

            if (eventCode == StateSynchronizationRequestSignalCode)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    Player player = PhotonNetwork.CurrentRoom.Players[photonEvent.Sender];
                    string clientExpectedStatusValue = (string) photonEvent.CustomData;


                    if (clientExpectedStatusValue == _expectedStatus)
                    {
                        Debug.LogWarningFormat("Check count of next req {0}", _synchronizedStatusNextRequests.Count);
                        if (_synchronizedStatusNextRequests.Count != 0)
                        {
                            _synchronizedStatusRequests.AddRange(_synchronizedStatusNextRequests);
                        }

                        Debug.LogWarningFormat("Player {0} expected for status {1}", player.IsMasterClient,
                            clientExpectedStatusValue);
                        _synchronizedStatusRequests.Add(photonEvent.Sender);
                    }

                    if (Array.IndexOf(_nextStatus, clientExpectedStatusValue) >= 0)
                    {
                        Debug.LogWarningFormat("Player {0} expected for next status {1}", player.IsMasterClient,
                            clientExpectedStatusValue);
                        _synchronizedStatusNextRequests.Add(photonEvent.Sender);
                    }

                    Debug.LogFormat("{0} {1}", string.Join(',', _synchronizedStatusRequests),
                        string.Join(',', _synchronizedStatusNextRequests));
                }

                return;
            }

            if (eventCode == StateSynchronizationRequestCode) // master's dispatch
            {
                _actions.Enqueue(() =>
                {
                    if (_expectedStatus == null)
                    {
                        Debug.Log("unknown status while await player synchronization");
                        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable
                        {
                            { "message", "unknown status while await player synchronization" }
                        });
                        return;
                    }

                    _actions.Enqueue(() => { _actualStatus = (string) photonEvent.CustomData; });

                    // string expectedStatus = (string) photonEvent.CustomData;
                    // if (_expectedStatus == expectedStatus)
                    // {
                    //     Player player = PhotonNetwork.CurrentRoom.Players[photonEvent.Sender];
                    //     Debug.LogFormat("Player {0} has synchronized to status {1}", player.UserId, expectedStatus);
                    //     PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable
                    //     {
                    //         { "message", $"Player {player.UserId} has synchronized to status {expectedStatus}" }
                    //     });
                    //     _synchronizedStatusRequests.Add(photonEvent.Sender);
                    // }
                    //
                    // if (_synchronizedStatusRequests.Count == PhotonNetwork.CurrentRoom.PlayerCount)
                    // {
                    //     _synchronizedStatusRequests.Clear();
                    //     PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable
                    //     {
                    //         { "message", $"All players have synchronized to status {expectedStatus}" }
                    //     });
                    //     PhotonNetwork.RaiseEvent(StateSynchronizationResponseCode,
                    //         expectedStatus,
                    //         new RaiseEventOptions
                    //         {
                    //             Receivers = ReceiverGroup.All
                    //         }, SendOptions.SendReliable);
                    // }
                });
                return;
            }

            if (eventCode == StateSynchronizationResponseCode)
            {
                _actions.Enqueue(() =>
                {
                    _actualStatus = (string) photonEvent.CustomData;
                    Debug.LogFormat("Expected status {0} received", _actualStatus);
                });
            }
        }
    }
}
