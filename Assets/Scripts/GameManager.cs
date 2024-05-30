#nullable enable


using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using CodingStrategy.Entities.BadSector;
using CodingStrategy.Entities.CodingTime;
using CodingStrategy.Factory;
using CodingStrategy.Network;
using CodingStrategy.UI.InGame;
using CodingStrategy.Utility;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Task = System.Threading.Tasks.Task;

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
        private const string CodingTimeEndStatus = "coding-time-end";
        private const string RuntimeStatus = "runtime";
        private const string RuntimeEndStatus = "runtime-end";

        public int currentRound = 0;
        public int round = 3;
        public int boardWidth = 9;
        public int boardHeight = 9;
        public InGameUI inGameUI = null!;
        public GameObject boardCellPrefab = null!;
        public GameObject bitPrefab = null!;
        public GameObject badSectorPrefab = null!;
        public List<GameObject> robotPrefabs = new List<GameObject>();
        public GameManagerUtil util = null!;
        public GameMangerNetworkProcessor networkProcessor = null!;
        public GameResult gameResult = null!;
        public QuitButtonManager quitButtonManager = null!;

        public bool awaitLobby = false;

        public IBoardDelegate BoardDelegate { get; private set; } = null!;

        public IRobotDelegatePool RobotDelegatePool { get; private set; } = null!;

        // public IPlayerPool PlayerPool { get; private set; } = null!;
        public AnimationCoroutineManager AnimationCoroutineManager { get; private set; } = null!;

        private BitDispenser _bitDispenser = null!;
        private IPlayerCommandNetworkDelegate _networkDelegate = null!;
        private IPlayerCommandCache _commandCache = null!;

        private GameManagerObjectSynchronizer _objectSynchronizer = null!;
        private GameManagerPlayerStatusSynchronizer _playerStatusSynchronizer = null!;

        public readonly IDictionary<string, int> PlayerIndexMap = new Dictionary<string, int>();

        private Coroutine? _coroutine;

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
            networkProcessor = gameObject.GetOrAddComponent<GameMangerNetworkProcessor>();
            networkProcessor.GameManagerUtil = util;
            gameResult = FindObjectOfType<GameResult>();
            quitButtonManager = FindObjectOfType<QuitButtonManager>();
        }

        public void Start()
        {
            Debug.Log("GameManager instance started.");

            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.AutomaticallySyncScene = true;
                PhotonNetwork.PhotonServerSettings.AppSettings.EnableLobbyStatistics = false;
                PhotonNetwork.NetworkingClient.EnableLobbyStatistics = false;
                PhotonNetwork.IsMessageQueueRunning = false;
                PhotonNetwork.ConnectUsingSettings();
                return;
            }

            _coroutine = StartCoroutine(StartGameManagerCoroutine());

            quitButtonManager.OnQuitButtonClick.AddListener(() =>
            {
                Debug.Log("button click in");
                awaitLobby = true;
                PhotonNetwork.LeaveRoom();
            });
        }

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

            if (_expectedStatus != null)
            {
                int elapsedTime = unchecked(PhotonNetwork.ServerTimestamp - _expectedStatusTimestamp);
                if (elapsedTime > 3000)
                {
                    AwaitAllPlayersStatus(_expectedStatus, retry: true);
                }
            }
        }

        public override void OnConnectedToMaster()
        {
            TypedLobby lobby = new TypedLobby("coding-strategy", LobbyType.SqlLobby);
            PhotonNetwork.JoinLobby(lobby);
        }

        public override void OnJoinedLobby()
        {
            if (awaitLobby)
            {
                return;
            }

            Debug.LogFormat("Connected to Master {0}", PhotonNetwork.CurrentLobby);
            // PhotonNetwork.GetCustomRoomList(PhotonNetwork.CurrentLobby, "C0='coding-strategy'");
            PhotonNetwork.NickName = "asdf";
            PhotonNetwork.CreateRoom("debug", roomOptions: new RoomOptions
            {
                MaxPlayers = 4,
                IsVisible = false,
                PublishUserId = true,
                CustomRoomProperties = new ExitGames.Client.Photon.Hashtable
                {
                    { "C0", "coding-strategy-debug" }
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

            _coroutine = StartCoroutine(StartGameManagerCoroutine());
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            DetachPlayerUI(otherPlayer);
            // util.RemovePlayerDelegate(otherPlayer);
        }

        public override void OnPlayerPropertiesUpdate(
            Player targetPlayer,
            ExitGames.Client.Photon.Hashtable changedProps)
        {
            _actions.Enqueue(() =>
            {
                if (!PhotonNetwork.IsMasterClient)
                {
                    return;
                }

                HashSet<string> status = new HashSet<string>();
                int count = 0;
                foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
                {
                    if (player.CustomProperties.ContainsKey("status"))
                    {
                        string statusValue = (string) player.CustomProperties["status"];
                        Debug.LogWarningFormat("Player {1} try to move to status: {0}", statusValue,
                            targetPlayer.UserId);
                        count++;
                        status.Add(statusValue);
                    }
                }

                if (count == PhotonNetwork.CurrentRoom.PlayerCount && status.Count == 1)
                {
                    PhotonNetwork.RaiseEvent(StateSynchronizationResponseCode, null,
                        new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
                }
            });
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.LogFormat("Room list count: {0}", roomList.Count);
        }

        public override void OnLeftRoom()
        {
            PlayerStatusUI playerStatus = FindPlayerStatusUI(util.LocalPhotonPlayerDelegate)!;
            StartCoroutine(gameResult.ResultUIAnimation(int.Parse(playerStatus.GetRank().Substring(0, 1))));
            awaitLobby = true;
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

        private bool _isStatusSynchronized = false;

        private IEnumerator StartGameManagerCoroutine()
        {
            while (PhotonNetwork.NetworkingClient.State != ClientState.Joined)
            {
                yield return null;
            }

            _networkDelegate = new PhotonPlayerCommandNetworkDelegate();
            _commandCache = new PhotonPlayerCommandCache(_networkDelegate);

            util.InitializePlayersByPhoton();

            foreach ((int index, Player photonPlayer) in PhotonNetwork.PlayerList.ToIndexed())
            {
                PlayerIndexMap[photonPlayer.UserId] = index;
                IPlayerDelegate playerDelegate = util.GetPlayerDelegate(photonPlayer);
                IRobotDelegate robotDelegate = BuildRobotDelegate(BoardDelegate, playerDelegate);
                playerDelegate.Robot = robotDelegate;
                RobotDelegatePool[playerDelegate.Id] = robotDelegate;
                (RobotDirection _, Coordinate _, Color color) = StartPositions[index];
                PreparePlayerUI(photonPlayer, inGameUI.playerStatusUI[index], color);
                Debug.LogWarningFormat("Initialize PlayerDelegate {0}", photonPlayer.UserId);
            }

            _objectSynchronizer.InitializeCells();

            _playerStatusSynchronizer = SetUpPlayerStatusSynchronizer();

            yield return null;

            #region ITERATION

            currentRound = 0;

            while (currentRound < round)
            {
                Debug.LogErrorFormat("round: {0} of {1}", currentRound, round);
                ;

                if (currentRound++ == round)
                {
                    break;
                }

                yield return StartCoroutine(AwaitAllPlayersStatus(ReadyStatus));

                #region INITIALIZATION

                inGameUI.gameturn.SetTurn(20);

                _networkDelegate.RequestRefresh();

                #region CHECK_DISCONNECTED_PLAYERS

                System.Collections.Generic.ISet<IPlayerDelegate> disconnectedPlayers = new HashSet<IPlayerDelegate>();

                foreach (IPlayerDelegate playerDelegate in util.PlayerDelegatePool)
                {
                    if (PhotonNetwork.CurrentRoom.Players
                        .Select(pair => pair.Value)
                        .Any(player => player.UserId == playerDelegate.Id))
                    {
                        continue;
                    }

                    disconnectedPlayers.Add(playerDelegate);
                }

                foreach (IPlayerDelegate disconnectedPlayer in disconnectedPlayers)
                {
                    Debug.LogFormat("Player {0} has disconnected", disconnectedPlayer.Id);
                    util.PlayerDelegatePool.Remove(disconnectedPlayer.Id);
                }

                #endregion

                inGameUI.SetCameraPosition(PlayerIndexMap[PhotonNetwork.LocalPlayer.UserId]);

                foreach (IPlayerDelegate playerDelegate in util.PlayerDelegatePool)
                {
                    int index = PlayerIndexMap[playerDelegate.Id];
                    (RobotDirection direction, Coordinate position, Color _) = StartPositions[index];
                    IRobotDelegate robotDelegate = RobotDelegatePool[playerDelegate.Id];

                    if (!BoardDelegate.Robots.Contains(robotDelegate))
                    {
                        BoardDelegate.Add(robotDelegate, position, direction);
                    }
                }

                // IPlayerDelegate dummyPlayerDelegate =
                //     new PlayerDelegateCreateFactory(new PlayerDelegateCreateStrategy("1234"))
                //         .Build();
                // IRobotDelegate dummy = new RobotDelegateCreateFactory(new RobotDelegateCreateStrategy(), BoardDelegate, dummyPlayerDelegate).Build();
                //
                // // util.PlayerDelegatePool["1234"] = dummyPlayerDelegate;
                // PlayerIndexMap["1234"] = 2;
                //
                // util.LocalPhotonPlayerDelegate.Currency = 1000;
                //
                // BoardDelegate.Add(new MalwareBadSector("1", BoardDelegate, dummy), new Coordinate(4, 1));
                // BoardDelegate.Add(new MalwareBadSector("1", BoardDelegate, dummy), new Coordinate(3, 1));
                // BoardDelegate.Add(new MalwareBadSector("1", BoardDelegate, dummy), new Coordinate(3, 0));
                // BoardDelegate.Add(new MalwareBadSector("1", BoardDelegate, dummy), new Coordinate(5, 0));
                // BoardDelegate.Add(new MalwareBadSector("1", BoardDelegate, dummy), new Coordinate(5, 1));

                NotifyDispatchBits();

                yield return StartCoroutine(AwaitAllPlayerPlaceablePlaceEventSynchronization());

                #endregion

                #region CODING_TIME

                yield return StartCoroutine(AwaitAllPlayersStatus(CodingTimeStatus));

                yield return new WaitForSeconds(1.0f);

                CodingTimeExecutor codingTimeExecutor = gameObject.GetOrAddComponent<CodingTimeExecutor>();

                PrepareCodingTimeExecutor(codingTimeExecutor);

                yield return LifeCycleMonoBehaviourBase.AwaitLifeCycleCoroutine(codingTimeExecutor);

                yield return StartCoroutine(AwaitAllPlayersStatus(CodingTimeEndStatus));

                #endregion

                #region RUNTIME

                yield return StartCoroutine(AwaitAllPlayersStatus(RuntimeStatus));
                yield return new WaitForSeconds(1.0f);

                RuntimeExecutor runtimeExecutor = gameObject.GetOrAddComponent<RuntimeExecutor>();

                PrepareRuntimeExecutor(runtimeExecutor);

                yield return LifeCycleMonoBehaviourBase.AwaitLifeCycleCoroutine(runtimeExecutor);

                _bitDispenser.Clear();

                yield return StartCoroutine(AwaitAllPlayersStatus(RuntimeEndStatus));

                #endregion

                if (util.LocalPhotonPlayerDelegate.HealthPoint > 0)
                {
                    yield return null;
                    continue;
                }

                yield break;
            }

            yield return null;


            Debug.LogFormat("Runtime terminated");
            PhotonNetwork.LeaveRoom();

            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            #endregion
        }

        private string? _expectedStatus;
        private int _expectedStatusTimestamp;
        private string? _actualStatus;


        private CustomYieldInstruction? _lastStatusSync;

        public IEnumerator AwaitAllPlayersStatus(string status, bool includingMasterClient = true, bool retry = false)
        {
            _expectedStatus = status;
            _expectedStatusTimestamp = PhotonNetwork.ServerTimestamp;
            int currentExpectedStatusTimestamp = _expectedStatusTimestamp;
            if (PhotonNetwork.IsMasterClient)
            {
                _currentStatusRequest = status;
            }

            if (includingMasterClient)
            {
                _actions.Enqueue(() =>
                {
                    PhotonNetwork.RaiseEvent(StateSynchronizationRequestCode, _expectedStatus,
                        new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient }, SendOptions.SendReliable);
                });
            }
            else
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    _actions.Enqueue(() =>
                    {
                        Debug.LogWarningFormat("Local Player try to move to status: {0}", status);
                        PhotonNetwork.RaiseEvent(StateSynchronizationRequestCode, _expectedStatus,
                            new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient }, SendOptions.SendReliable);
                    });
                }
            }
            // if (includingMasterClient)
            // {
            //     _actions.Enqueue(() =>
            //     {
            //         Debug.LogWarningFormat("Local Player try to move to status: {0}", status);
            //         PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
            //         {
            //             { "status", status }
            //         });
            //     });
            // }
            // else
            // {
            //     if (PhotonNetwork.IsMasterClient)
            //     {
            //         _actions.Enqueue(() =>
            //         {
            //             Debug.LogWarningFormat("Local Player try to move to status: {0}", status);
            //             PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
            //             {
            //                 { "status", status }
            //             });
            //         });
            //     }
            // }

            if (!retry)
            {
                yield return new WaitUntil(() =>
                {
                    // if (!_isStatusSynchronized)
                    // {
                    //     return false;
                    // }

                    // HashSet<string> status = new HashSet<string>();
                    // int count = 0;
                    //
                    // foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
                    // {
                    //     if (!player.CustomProperties.ContainsKey("status"))
                    //     {
                    //         continue;
                    //     }
                    //
                    //     count++;
                    //     status.Add((string) player.CustomProperties["status"]);
                    // }
                    //
                    // Debug.LogFormat("count: {0}, status: {1}", PhotonNetwork.CurrentRoom.PlayerCount,
                    //     string.Join(", ", status));
                    // return count == PhotonNetwork.CurrentRoom.PlayerCount && status.Count == 1;
                    if (_expectedStatus == null || _actualStatus == null)
                    {
                        return false;
                    }

                    _isStatusSynchronized = _expectedStatus == _actualStatus;
                    return _isStatusSynchronized;
                });
                _expectedStatus = null;
                _actualStatus = null;
                _isStatusSynchronized = false;
            }
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
                    continue;
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

        private const byte StateSynchronizationRequestCode = 100;
        private const byte StateSynchronizationResponseCode = 101;

        private readonly HashSet<Player> _responsePlayers = new HashSet<Player>();
        private string? _currentStatusRequest = null;
        private readonly HashSet<int> _synchronizedStatusRequests = new HashSet<int>();

        public void OnEvent(EventData photonEvent)
        {
            byte eventCode = photonEvent.Code;

            Debug.LogFormat("event fired: {0}", eventCode);

            if (eventCode == StateSynchronizationRequestCode)
            {
                _actions.Enqueue(() =>
                {
                    if (_currentStatusRequest == null)
                    {
                        Debug.Log("unknown status while await player synchronization");
                        return;
                    }

                    string expectedStatus = (string) photonEvent.CustomData;
                    if (_expectedStatus == expectedStatus)
                    {
                        Player player = PhotonNetwork.CurrentRoom.Players[photonEvent.Sender];
                        Debug.LogFormat("Player {0} has synchronized to status {1}", player.UserId, expectedStatus);
                        _synchronizedStatusRequests.Add(photonEvent.Sender);
                    }

                    if (_synchronizedStatusRequests.Count == PhotonNetwork.CurrentRoom.PlayerCount)
                    {
                        _synchronizedStatusRequests.Clear();
                        PhotonNetwork.RaiseEvent(StateSynchronizationResponseCode, _currentStatusRequest,
                            new RaiseEventOptions
                            {
                                Receivers = ReceiverGroup.All
                            }, SendOptions.SendReliable);
                    }
                });
                // _actions.Enqueue(() =>
                // {
                //     HashSet<string> status = new HashSet<string>();
                //     int count = 0;
                //
                //     foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
                //     {
                //         if (!player.CustomProperties.ContainsKey("status"))
                //         {
                //             continue;
                //         }
                //
                //         count++;
                //         status.Add((string) player.CustomProperties["status"]);
                //     }
                //
                //     Debug.LogFormat("count: {0}, status: {1}", PhotonNetwork.CurrentRoom.PlayerCount,
                //         string.Join(", ", status));
                //
                //     if (count == PhotonNetwork.CurrentRoom.PlayerCount && status.Count == 1)
                //     {
                //         if (PhotonNetwork.IsMasterClient)
                //         {
                //             PhotonNetwork.RaiseEvent(StateSynchronizationResponseCode, null,
                //                 new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
                //         }
                //     }
                // });

                return;
            }

            if (eventCode == StateSynchronizationResponseCode)
            {
                _actions.Enqueue(() =>
                {
                    // Debug.LogFormat("State synchronized with {0}",
                    //     PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("status"));
                    _actualStatus = (string) photonEvent.CustomData;
                    // _isStatusSynchronized = true;
                });
                return;
            }

            if (eventCode == BitPlaceResponseCode)
            {
                _actions.Enqueue(() =>
                {
                    Debug.Assert(PhotonNetwork.IsMasterClient);
                    Player photonPlayer = PhotonNetwork.CurrentRoom.Players[photonEvent.Sender];
                    _responsePlayers.Add(photonPlayer);

                    if (_responsePlayers.Count >= PhotonNetwork.CurrentRoom.PlayerCount)
                    {
                        return;
                    }

                    Debug.Log("All player has placed all bits");
                    _responsePlayers.Clear();
                    PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
                    {
                        { "status", PlaceablePlaceStatus }
                    });
                });
                return;
            }

            if (eventCode != BitPlaceRequestCode)
            {
                return;
            }

            _actions.Enqueue(() =>
            {
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
            });
        }

        public void NotifyDispatchBits()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            _actions.Enqueue(() =>
            {
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
            });
        }

        public IEnumerator AwaitAllPlayerPlaceablePlaceEventSynchronization()
        {
            yield return StartCoroutine(AwaitAllPlayersStatus(PlaceablePlaceStatus));
        }
    }
}
