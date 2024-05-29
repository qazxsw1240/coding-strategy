#nullable enable


using System;
using CodingStrategy.Entities;
using CodingStrategy.Entities.Player;
using CodingStrategy.Entities.Robot;
using CodingStrategy.Factory;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Events;

namespace CodingStrategy
{
    public class GameManagerUtil : MonoBehaviourPunCallbacks
    {
        public readonly IPlayerPool PlayerDelegatePool = new PlayerPoolImpl();

        private IPlayerDelegate? _localPlayerDelegate = null;
        private IRobotDelegate? _localRobotDelegate = null;

        public void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void InitializePlayersByPhoton()
        {
            InitializePlayersByPhoton(PhotonNetwork.CurrentRoom);
        }

        public void InitializePlayersByPhoton(Room room)
        {
            foreach ((int _, Player photonPlayer) in room.Players)
            {
                string id = photonPlayer.UserId;
                IPlayerDelegateCreateStrategy strategy = new PlayerDelegateCreateStrategy(id);
                IPlayerDelegateCreateFactory factory = new PlayerDelegateCreateFactory(strategy);
                IPlayerDelegate playerDelegate = factory.Build();
                PlayerDelegatePool[playerDelegate.Id] = playerDelegate;
            }
        }

        public void ClearPlayers()
        {
            foreach (IPlayerDelegate playerDelegate in PlayerDelegatePool)
            {
                playerDelegate.OnHealthPointChange.RemoveAllListeners();
                playerDelegate.OnCurrencyChange.RemoveAllListeners();
                playerDelegate.OnExpChange.RemoveAllListeners();
                playerDelegate.OnLevelChange.RemoveAllListeners();
            }

            PlayerDelegatePool.Clear();
        }

        public IPlayerDelegate LocalPhotonPlayerDelegate
        {
            get
            {
                if (_localPlayerDelegate == null)
                {
                    Player photonPlayer = PhotonNetwork.LocalPlayer;
                    IPlayerDelegate playerDelegate = new LocalPlayerDelegate(PlayerDelegatePool[photonPlayer.UserId],
                        photonPlayer);
                    _localPlayerDelegate = playerDelegate;
                    PlayerDelegatePool[photonPlayer.UserId] = _localPlayerDelegate;
                }

                return _localPlayerDelegate;
            }
        }

        public IRobotDelegate LocalPhotonRobotDelegate
        {
            get
            {
                if (_localRobotDelegate == null)
                {
                    _localRobotDelegate = LocalPhotonPlayerDelegate.Robot;
                    _localRobotDelegate.OnHealthPointChange.AddListener((_, _, next) =>
                    {
                        int validNext = Math.Max(0, Math.Min(5, next));
                        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable
                        {
                            { GameMangerNetworkProcessor.RobotHpKey, validNext }
                        });
                    });
                }

                return _localRobotDelegate;
            }
        }

        public IRobotDelegate GetRobotDelegateById(string id)
        {
            return GetPlayerDelegateById(id).Robot;
        }

        public IRobotDelegate GetRobotDelegate(Player photonPlayer)
        {
            return GetPlayerDelegate(photonPlayer).Robot;
        }

        public IPlayerDelegate GetPlayerDelegateById(string id)
        {
            return PlayerDelegatePool[id];
        }

        public IPlayerDelegate GetPlayerDelegate(Player photonPlayer)
        {
            return PlayerDelegatePool[photonPlayer.UserId];
        }

        public void RemovePlayerDelegate(Player photonPlayer)
        {
            string id = photonPlayer.UserId;
            IPlayerDelegate playerDelegate = PlayerDelegatePool[id];
            playerDelegate.OnHealthPointChange.RemoveAllListeners();
            playerDelegate.OnCurrencyChange.RemoveAllListeners();
            playerDelegate.OnExpChange.RemoveAllListeners();
            playerDelegate.OnLevelChange.RemoveAllListeners();
            PlayerDelegatePool.Remove(id);
        }

        private class LocalPlayerDelegate : IPlayerDelegate
        {
            private readonly IPlayerDelegate _playerDelegate;
            private readonly Player _photonPlayer;

            public LocalPlayerDelegate(IPlayerDelegate playerDelegate, Player photonPlayer)
            {
                _playerDelegate = playerDelegate;
                _photonPlayer = photonPlayer;
            }

            public string Id => _playerDelegate.Id;

            public int HealthPoint
            {
                get => _playerDelegate.HealthPoint;
                set
                {
                    _playerDelegate.HealthPoint = value;
                    _photonPlayer.SetCustomProperties(new Hashtable
                    {
                        { GameMangerNetworkProcessor.PlayerHpKey, value }
                    });
                }
            }

            public int Level
            {
                get => _playerDelegate.Level;
                set
                {
                    _playerDelegate.Level = value;
                    _photonPlayer.SetCustomProperties(new Hashtable
                    {
                        { GameMangerNetworkProcessor.LevelKey, value }
                    });
                }
            }

            public int Exp
            {
                get => _playerDelegate.Exp;
                set
                {
                    _playerDelegate.Exp = value;
                    _photonPlayer.SetCustomProperties(new Hashtable
                    {
                        { GameMangerNetworkProcessor.ExpKey, value }
                    });
                }
            }

            public int Currency
            {
                get => _playerDelegate.Currency;
                set
                {
                    _playerDelegate.Currency = value;
                    _photonPlayer.SetCustomProperties(new Hashtable
                    {
                        { GameMangerNetworkProcessor.CurrencyKey, value }
                    });
                }
            }

            public IRobotDelegate Robot
            {
                get => _playerDelegate.Robot;
                set => _playerDelegate.Robot = value;
            }

            public IAlgorithm Algorithm => _playerDelegate.Algorithm;

            public UnityEvent<int, int> OnHealthPointChange => _playerDelegate.OnHealthPointChange;

            public UnityEvent<int, int> OnLevelChange => _playerDelegate.OnLevelChange;

            public UnityEvent<int, int> OnExpChange => _playerDelegate.OnExpChange;

            public UnityEvent<int, int> OnCurrencyChange => _playerDelegate.OnCurrencyChange;

            public int CompareTo(IGameEntity other)
            {
                throw new NotImplementedException();
            }
        }
    }
}
