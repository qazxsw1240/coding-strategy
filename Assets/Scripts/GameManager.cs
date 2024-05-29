#nullable enable


using System.Linq;
using CodingStrategy.Entities.CodingTime;
using CodingStrategy.Factory;
using CodingStrategy.Network;
using CodingStrategy.UI.InGame;
using CodingStrategy.Utility;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

namespace CodingStrategy
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Entities.Runtime.Abnormality;
    using Entities;
    using Entities.Board;
    using Entities.Player;
    using Entities.Robot;
    using Entities.Runtime;
    using Unity.VisualScripting;
    using UnityEngine;

    public class GameManager : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        public static readonly (RobotDirection, Coordinate, Color)[] StartPositions =
        {
            (RobotDirection.North, new Coordinate(4, 0), PlayerStatusUI.Red),
            (RobotDirection.East, new Coordinate(0, 4), PlayerStatusUI.Yellow),
            (RobotDirection.South, new Coordinate(4, 8), PlayerStatusUI.Green),
            (RobotDirection.West, new Coordinate(8, 4), PlayerStatusUI.Blue)
        };

        private static readonly IDictionary<string, IAbnormality> AbnormalityDictionary =
            new Dictionary<string, IAbnormality>();

        public static IDictionary<string, IAbnormality> GetAbnormalities()
        {
            return AbnormalityDictionary;
        }

        public static void SetAbnormalityValue(string key, IAbnormality abnormality)
        {
            AbnormalityDictionary[key] = abnormality;
        }

        public static IAbnormality? GetAbnormalityValue(string key)
        {
            if (AbnormalityDictionary.TryGetValue(key, out IAbnormality value))
            {
                return value;
            }

            return null;
        }

        private const string ReadyStatus = "ready";
        private const string RobotPlaceStatus = "robot-place";
        private const string PlaceablePlaceStatus = "placeable-place";
        private const string CodingTimeStatus = "coding-time";
        private const string RuntimeStatus = "runtime";


        public int round = 15;
        public int boardWidth = 9;
        public int boardHeight = 9;
        public InGameUI inGameUI = null!;
        public GameObject boardCellPrefab = null!;
        public GameObject bitPrefab = null!;
        public GameObject badSectorPrefab = null!;
        public List<GameObject> robotPrefabs = new List<GameObject>();
        public GameManagerUtil util = null!;
        public GameMangerNetworkProcessor networkProcessor = null!;

        public IBoardDelegate BoardDelegate { get; private set; } = null!;
        public IRobotDelegatePool RobotDelegatePool { get; private set; } = null!;
        public IPlayerPool PlayerPool { get; private set; } = null!;
        public AnimationCoroutineManager AnimationCoroutineManager { get; private set; } = null!;

        private BitDispenser _bitDispenser = null!;
        private IPlayerCommandNetworkDelegate _networkDelegate = null!;
        private IPlayerCommandCache _commandCache = null!;

        private GameManagerObjectSynchronizer _objectSynchronizer = null!;
        private GameManagerPlayerStatusSynchronizer _playerStatusSynchronizer = null!;

        public readonly IDictionary<string, int> PlayerIndexMap = new Dictionary<string, int>();

        public static IPlayerDelegate BuildPlayerDelegate(string id)
        {
            IPlayerDelegateCreateStrategy strategy = new PlayerDelegateCreateStrategy(id);
            IPlayerDelegateCreateFactory factory = new PlayerDelegateCreateFactory(strategy);
            return factory.Build();
        }

        public static IRobotDelegate BuildRobotDelegate(IBoardDelegate boardDelegate, IPlayerDelegate playerDelegate)
        {
            IRobotDelegateCreateStrategy strategy = new RobotDelegateCreateStrategy();
            IRobotDelegateCreateFactory factory =
                new RobotDelegateCreateFactory(strategy, boardDelegate, playerDelegate);
            return factory.Build();
        }

        public void Awake()
        {
            GameInitializer.Initialize();

            BoardDelegate = new BoardDelegateImpl(boardWidth, boardHeight);
            RobotDelegatePool = new RobotDelegatePoolImpl();
            _objectSynchronizer = SetUpObjectSynchronizer();
            AnimationCoroutineManager = gameObject.GetOrAddComponent<AnimationCoroutineManager>();

            util = gameObject.GetOrAddComponent<GameManagerUtil>();
            _bitDispenser = new BitDispenser(BoardDelegate, util.PlayerDelegatePool);
            PlayerPool = util.PlayerDelegatePool;
            networkProcessor = gameObject.GetOrAddComponent<GameMangerNetworkProcessor>();
            networkProcessor.GameManagerUtil = util;
        }

        public void Start()
        {
            Debug.Log("GameManager instance started.");

            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.AutomaticallySyncScene = true;
                PhotonNetwork.PhotonServerSettings.AppSettings.EnableLobbyStatistics = true;
                PhotonNetwork.NetworkingClient.EnableLobbyStatistics = true;
                PhotonNetwork.IsMessageQueueRunning = true;
                PhotonNetwork.ConnectUsingSettings();
                return;
            }

            StartCoroutine(StartGameManagerCoroutine());
        }

        public override void OnConnectedToMaster()
        {
            TypedLobby lobby = new TypedLobby("coding-strategy", LobbyType.SqlLobby);
            PhotonNetwork.JoinLobby(lobby);
        }

        public override void OnJoinedLobby()
        {
            Debug.LogFormat("Connected to Master {0}", PhotonNetwork.CurrentLobby);
            PhotonNetwork.GetCustomRoomList(PhotonNetwork.CurrentLobby, "C0='coding-strategy'");
            PhotonNetwork.NickName = "asdf";
            PhotonNetwork.CreateRoom("test", roomOptions: new RoomOptions
            {
                MaxPlayers = 4,
                IsVisible = false,
                PublishUserId = true,
                CustomRoomProperties = new ExitGames.Client.Photon.Hashtable
                {
                    { "C0", "coding-strategy" }
                },
                CustomRoomPropertiesForLobby = new string[] { "C0" }
            });
        }

        public override void OnJoinedRoom()
        {
            Debug.LogFormat("Connected to Room {0}", PhotonNetwork.CurrentRoom.Name);

            foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                string id = player.UserId;
                IPlayerDelegate playerDelegate = BuildPlayerDelegate(id);
                IRobotDelegate robotDelegate = BuildRobotDelegate(BoardDelegate, playerDelegate);
                util.PlayerDelegatePool[id] = playerDelegate;
                RobotDelegatePool[id] = robotDelegate;
            }

            StartCoroutine(StartGameManagerCoroutine());
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            DetachPlayerUI(otherPlayer);
            util.RemovePlayerDelegate(otherPlayer);
        }

        public override void OnPlayerPropertiesUpdate(
            Player targetPlayer,
            ExitGames.Client.Photon.Hashtable changedProps)
        {
            Debug.Log(string.Join(", ", targetPlayer.CustomProperties.ToList()));
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.LogFormat("Room list count: {0}", roomList.Count);
        }

        private GameManagerObjectSynchronizer SetUpObjectSynchronizer()
        {
            GameManagerObjectSynchronizer objectSynchronizer =
                gameObject.GetOrAddComponent<GameManagerObjectSynchronizer>();
            objectSynchronizer.GameManager = this;
            return objectSynchronizer;
        }

        private GameManagerPlayerStatusSynchronizer SetUpPlayerStatusSynchronizer()
        {
            GameManagerPlayerStatusSynchronizer playerStatusSynchronizer =
                gameObject.GetOrAddComponent<GameManagerPlayerStatusSynchronizer>();
            playerStatusSynchronizer.GameManager = this;
            return playerStatusSynchronizer;
        }

        public static IEnumerator AwaitAllPlayersStatus(string status, bool includingMasterClient = true)
        {
            if (includingMasterClient || PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
                {
                    { "status", status }
                });
            }


            yield return new WaitUntil(() =>
            {
                Dictionary<int, Player>.ValueCollection players = PhotonNetwork.CurrentRoom.Players.Values;
                foreach (Player player in players)
                {
                    ExitGames.Client.Photon.Hashtable properties = player.CustomProperties;
                    if (properties.TryGetValue("status", out object value))
                    {
                        continue;
                    }

                    string statusValue = (string) value!;
                    if (statusValue != status)
                    {
                        return false;
                    }
                }

                return true;
            });
        }

        private IEnumerator StartGameManagerCoroutine()
        {
            _networkDelegate = new PhotonPlayerCommandNetworkDelegate();
            _commandCache = new PhotonPlayerCommandCache(_networkDelegate);

            util.InitializePlayersByPhoton();

            foreach ((int index, Player photonPlayer) in PhotonNetwork.PlayerList.ToIndexed())
            {
                Debug.LogFormat("{0}: {1}", index, photonPlayer.NickName);
                PlayerIndexMap[photonPlayer.UserId] = index;
                IPlayerDelegate playerDelegate = util.GetPlayerDelegate(photonPlayer);
                IRobotDelegate robotDelegate = BuildRobotDelegate(BoardDelegate, playerDelegate);
                playerDelegate.Robot = robotDelegate;
                RobotDelegatePool[playerDelegate.Id] = robotDelegate;
                (RobotDirection _, Coordinate _, Color color) = StartPositions[index];
                PreparePlayerUI(photonPlayer, inGameUI.playerStatusUI[index], color);
            }

            _objectSynchronizer.InitializeCells();

            yield return StartCoroutine(AwaitAllPlayersStatus(ReadyStatus));

            _playerStatusSynchronizer = SetUpPlayerStatusSynchronizer();

            #region ITERATION

            for (int i = 0; i < round; i++)
            {
                #region INITIALIZATION

                inGameUI.gameturn.SetTurn(20);

                _networkDelegate.RequestRefresh();

                inGameUI.SetCameraPosition(PlayerIndexMap[PhotonNetwork.LocalPlayer.UserId]);


                foreach (IPlayerDelegate playerDelegate in util.PlayerDelegatePool)
                {
                    int index = PlayerIndexMap[playerDelegate.Id];
                    (RobotDirection direction, Coordinate position, Color _) = StartPositions[index];
                    IRobotDelegate robotDelegate = RobotDelegatePool[playerDelegate.Id];
                    BoardDelegate.Add(robotDelegate, position, direction);
                }

                NotifyDispatchBits();

                yield return StartCoroutine(AwaitAllPlayerPlaceablePlaceEventSynchronization());

                #endregion

                #region CODING_TIME

                yield return StartCoroutine(AwaitAllPlayersStatus(CodingTimeStatus));

                yield return new WaitForSeconds(2.0f);

                CodingTimeExecutor codingTimeExecutor = gameObject.GetOrAddComponent<CodingTimeExecutor>();

                PrepareCodingTimeExecutor(codingTimeExecutor);

                yield return LifeCycleMonoBehaviourBase.AwaitLifeCycleCoroutine(codingTimeExecutor);

                #endregion

                #region RUNTIME

                RuntimeExecutor runtimeExecutor = gameObject.GetOrAddComponent<RuntimeExecutor>();

                PrepareRuntimeExecutor(runtimeExecutor);

                yield return LifeCycleMonoBehaviourBase.AwaitLifeCycleCoroutine(runtimeExecutor);

                _bitDispenser.Clear();

                yield return StartCoroutine(AwaitAllPlayersStatus(RuntimeStatus));

                #endregion
            }

            #endregion

            Debug.Log("Runtime terminated");
        }

        private void PreparePlayerUI(Player photonPlayer, PlayerStatusUI playerStatusUI, Color color)
        {
            IPlayerDelegate playerDelegate = util.GetPlayerDelegate(photonPlayer);
            IRobotDelegate robotDelegate = RobotDelegatePool[playerDelegate.Id];
            playerStatusUI.SetUserID(playerDelegate.Id);
            playerStatusUI.SetColor(color);
            playerStatusUI.SetName(photonPlayer.NickName);
            playerStatusUI.SetRank(1);
            playerStatusUI.SetMoney(playerDelegate.Currency);
            playerStatusUI.SetPlayerHP(playerDelegate.HealthPoint);
            playerStatusUI.SetRobotHP(robotDelegate.HealthPoint);
            playerDelegate.OnCurrencyChange.AddListener((_, currency) => playerStatusUI.SetMoney(currency));
            playerDelegate.OnCurrencyChange.AddListener((_, _) => UpdatePlayerRanks());
            playerDelegate.OnHealthPointChange.AddListener((_, hp) => playerStatusUI.SetPlayerHP(hp));
            robotDelegate.OnHealthPointChange.AddListener((_, _, hp) => playerStatusUI.SetRobotHP(Math.Max(hp, 0)));
        }

        private void DetachPlayerUI(Player photonPlayer)
        {
            IPlayerDelegate playerDelegate = util.GetPlayerDelegate(photonPlayer);
            PlayerStatusUI playerStatusUI = FindPlayerStatusUI(playerDelegate)!;
            playerStatusUI.SetName("연결 끊김");
        }

        private void PrepareCodingTimeExecutor(CodingTimeExecutor codingTimeExecutor)
        {
            codingTimeExecutor.Util = util;
            codingTimeExecutor.InGameUI = inGameUI;
            codingTimeExecutor.PlayerPool = util.PlayerDelegatePool;
            codingTimeExecutor.NetworkDelegate = _networkDelegate;
            codingTimeExecutor.NetworkProcessor = networkProcessor;
            codingTimeExecutor.CommandCache = _commandCache;
        }

        private void PrepareRuntimeExecutor(RuntimeExecutor runtimeExecutor)
        {
            runtimeExecutor.GameManager = this;
            runtimeExecutor.BoardDelegate = BoardDelegate;
            runtimeExecutor.RobotDelegatePool = RobotDelegatePool;
            runtimeExecutor.PlayerPool = util.PlayerDelegatePool;
            runtimeExecutor.BitDispenser = _bitDispenser;
            runtimeExecutor.AnimationCoroutineManager = AnimationCoroutineManager;
            runtimeExecutor.OnRoundNumberChange.AddListener((_, round) => inGameUI.gameturn.SetTurn(round));
        }

        public PlayerStatusUI? FindPlayerStatusUI(IPlayerDelegate playerDelegate)
        {
            foreach (PlayerStatusUI playerStatusUI in inGameUI.playerStatusUI)
            {
                if (playerStatusUI.GetUserID() == playerDelegate.Id)
                {
                    return playerStatusUI;
                }
            }

            return null;
        }

        public void UpdatePlayerRanks()
        {
            IPlayerDelegate[] playerDelegates = util.PlayerDelegatePool.ToArray();
            Array.Sort(playerDelegates, (x, y) => y.Currency - x.Currency);
            int rank = 1;
            for (int i = 0; i < playerDelegates.Length; i++)
            {
                PlayerStatusUI? playerStatusUI = FindPlayerStatusUI(playerDelegates[i]);

                if (ReferenceEquals(playerStatusUI, null))
                {
                    throw new Exception();
                }

                if (i != 0)
                {
                    IPlayerDelegate previousPlayerDelegate = playerDelegates[i - 1];
                    if (previousPlayerDelegate.Currency != playerDelegates[i].Currency)
                    {
                        rank++;
                    }
                }

                playerStatusUI.SetRank(rank);
            }
        }

        private const byte BitPlaceRequestCode = 128;
        private const byte BitPlaceResponseCode = 129;

        private readonly HashSet<Player> _responsePlayers = new HashSet<Player>();

        public void OnEvent(EventData photonEvent)
        {
            byte eventCode = photonEvent.Code;

            if (eventCode == BitPlaceResponseCode)
            {
                Debug.Assert(PhotonNetwork.IsMasterClient);
                Player photonPlayer = PhotonNetwork.CurrentRoom.Players[photonEvent.Sender];
                _responsePlayers.Add(photonPlayer);

                if (_responsePlayers.Count != PhotonNetwork.CurrentRoom.PlayerCount - 1)
                {
                    return;
                }

                _responsePlayers.Clear();
                PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
                {
                    { "status", PlaceablePlaceStatus }
                });
                return;
            }

            if (eventCode != BitPlaceRequestCode)
            {
                return;
            }

            object[][] data = (object[][]) photonEvent.CustomData;
            foreach (object[] position in data)
            {
                int x = (int) position[0];
                int y = (int) position[1];
                Coordinate coordinate = new Coordinate(x, y);
                _bitDispenser.Dispense(coordinate);
            }

            PhotonNetwork.RaiseEvent(BitPlaceResponseCode,
                true,
                new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient },
                SendOptions.SendReliable);
        }

        public void NotifyDispatchBits()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            IList<Coordinate> positions = _bitDispenser.GetBitPositions(PhotonNetwork.CurrentRoom.PlayerCount * 2);
            object[][] serializedPositions = new object[positions.Count][];

            foreach ((int i, Coordinate position) in positions.ToIndexed())
            {
                serializedPositions[i] = new object[] { position.X, position.Y };
            }

            PhotonNetwork.RaiseEvent(BitPlaceRequestCode,
                serializedPositions,
                new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.All
                },
                SendOptions.SendReliable);
        }

        public IEnumerator AwaitAllPlayerPlaceablePlaceEventSynchronization()
        {
            yield return StartCoroutine(AwaitAllPlayersStatus(PlaceablePlaceStatus, false));
        }
    }
}
