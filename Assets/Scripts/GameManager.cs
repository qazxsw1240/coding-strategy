#nullable enable


using System.Linq;
using CodingStrategy.Entities.Animations;
using CodingStrategy.Entities.BadSector;
using CodingStrategy.Entities.Placeable;
using CodingStrategy.Entities.Runtime.Statement;
using CodingStrategy.Factory;
using CodingStrategy.Network;
using CodingStrategy.UI.InGame;
using CodingStrategy.Utility;
using NUnit.Framework;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Serialization;

namespace CodingStrategy
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using CodingStrategy.Entities.Runtime.Abnormality;
    using Entities;
    using Entities.Board;
    using Entities.Player;
    using Entities.Robot;
    using Entities.Runtime;
    using Unity.VisualScripting;
    using UnityEngine;

    public class GameManager : MonoBehaviourPunCallbacks
    {
        private static readonly (RobotDirection, Coordinate, Color)[] StartPositions =
        {
            (RobotDirection.North, new Coordinate(4, 0), PlayerStatusUI.Blue),
            (RobotDirection.East, new Coordinate(0, 4), PlayerStatusUI.Yellow),
            (RobotDirection.South, new Coordinate(4, 8), PlayerStatusUI.Green),
            (RobotDirection.West, new Coordinate(8, 4), PlayerStatusUI.Red)
        };

        private static readonly IDictionary<string, IAbnormality> AbnormalityDictionary = new Dictionary<string, IAbnormality>();

        public static void SetAbnormalityValue(string key, IAbnormality abnormality)
        {
            AbnormalityDictionary[key] = abnormality;
        }

        public static IAbnormality? GetAbnormalityValue(string key)
        {
            if(AbnormalityDictionary.TryGetValue(key, out IAbnormality value))
            {
                return value;
            }
            return null;
        }


        public int boardWidth = 9;
        public int boardHeight = 9;
        public InGameUI inGameUI = null!;
        public GameObject boardCellPrefab = null!;
        public GameObject bitPrefab = null!;
        public List<GameObject> robotPrefabs = new List<GameObject>();

        private IBoardDelegate _boardDelegate = null!;
        private IRobotDelegatePool _robotDelegatePool = null!;
        private IPlayerPool _playerPool = null!;

        private AnimationCoroutineManager _animationCoroutineManager = null!;
        private BitDispenser _bitDispenser = null!;

        private IPlayerCommandNetworkDelegate _networkDelegate = null!;
        private IPlayerCommandCache _commandCache = null!;


        public void Awake()
        {
            _boardDelegate = new BoardDelegateImpl(boardWidth, boardHeight);
            _robotDelegatePool = new RobotDelegatePoolImpl();
            _playerPool = new PlayerPoolImpl();
            _bitDispenser = new BitDispenser(_boardDelegate, _playerPool);
            _animationCoroutineManager = gameObject.GetOrAddComponent<AnimationCoroutineManager>();

            foreach (IPlayerDelegate mockPlayerDelegate in MockPlayerDelegates)
            {
                _playerPool[mockPlayerDelegate.Id] = mockPlayerDelegate;
                IRobotDelegate robotDelegate = new RobotDelegateCreateFactory(new RobotDelegateCreateStrategy(),
                        _boardDelegate,
                        mockPlayerDelegate)
                    .Build();
                mockPlayerDelegate.Robot = robotDelegate;
                _robotDelegatePool[mockPlayerDelegate.Id] = robotDelegate;
            }

            foreach ((int index, IPlayerDelegate playerDelegate) in _playerPool.ToIndexed())
            {
                (RobotDirection direction, Coordinate position, Color color) = StartPositions[index];
                IRobotDelegate robotDelegate = _robotDelegatePool[playerDelegate.Id];
                PreparePlayerUI(playerDelegate, inGameUI.playerStatusUI[index], color);
                PrepareRobotDelegate(robotDelegate, position, direction, robotPrefabs[index]);
                _boardDelegate.Add(robotDelegate, position, direction);
            }

            _boardDelegate.OnBadSectorAdd.AddListener(CreateBadSectorInstance);
            _boardDelegate.OnPlaceableAdd.AddListener(CreateBitInstance);
        }

        public void Start()
        {
            Debug.Log("GameManager instance started.");

            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.AutomaticallySyncScene = true;
                PhotonNetwork.ConnectUsingSettings();
            }

            // StartCoroutine(StartGameManagerCoroutine());
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to Master.");
            PhotonNetwork.NickName = "asdf";
            PhotonNetwork.JoinRandomOrCreateRoom(roomOptions: new RoomOptions
            {
                MaxPlayers = 4,
                PublishUserId = true
            });
        }

        public override void OnJoinedRoom()
        {
            Debug.LogFormat("Connected to Room {0}", PhotonNetwork.CurrentRoom.Name);
            StartCoroutine(StartGameManagerCoroutine());
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
        {
            targetPlayer.SetCustomProperties(changedProps);
        }

        private IEnumerator StartGameManagerCoroutine()
        {
            _networkDelegate = new PhotonPlayerCommandNetworkDelegate();
            _commandCache = new PhotonPlayerCommandCache(_networkDelegate);

            foreach (int playersKey in PhotonNetwork.CurrentRoom.Players.Keys)
            {
                Debug.LogFormat("Player Key: {0}", playersKey);
            }

            InitializeCells();

            _networkDelegate.RequestRefresh();

            foreach (IPlayerDelegate playerDelegate in _playerPool)
            {
                playerDelegate.Algorithm.Add(new TestCommand());
            }

            _bitDispenser.Dispense();

            yield return new WaitForSeconds(2.0f);

            // CodingTimeExecutor codingTimeExecutor = gameObject.GetOrAddComponent<CodingTimeExecutor>();
            //
            // yield return LifeCycleMonoBehaviourBase.AwaitLifeCycleCoroutine(codingTimeExecutor);

            RuntimeExecutor runtimeExecutor = gameObject.GetOrAddComponent<RuntimeExecutor>();

            PrepareRuntimeExecutor(runtimeExecutor);

            yield return LifeCycleMonoBehaviourBase.AwaitLifeCycleCoroutine(runtimeExecutor);

            _bitDispenser.Clear();
        }

        private void PreparePlayerUI(IPlayerDelegate playerDelegate, PlayerStatusUI playerStatusUI, Color color)
        {
            IRobotDelegate robotDelegate = _robotDelegatePool[playerDelegate.Id];
            playerStatusUI.SetUserID(playerDelegate.Id);
            playerStatusUI.SetColor(color);
            playerStatusUI.SetName(playerDelegate.Id);
            playerStatusUI.SetRank(1);
            playerStatusUI.SetMoney(playerDelegate.Currency);
            playerStatusUI.SetPlayerHP(playerDelegate.HealthPoint);
            playerStatusUI.SetRobotHP(robotDelegate.HealthPoint);
            playerDelegate.OnCurrencyChange.AddListener((_, currency) => playerStatusUI.SetMoney(currency));
            playerDelegate.OnCurrencyChange.AddListener((_, _) => UpdatePlayerRanks());
            playerDelegate.OnHealthPointChange.AddListener((_, hp) => playerStatusUI.SetPlayerHP(hp));
            robotDelegate.OnHealthPointChange.AddListener((_, _, hp) => playerStatusUI.SetRobotHP(Math.Max(hp, 0)));
        }

        private void PrepareRuntimeExecutor(RuntimeExecutor runtimeExecutor)
        {
            runtimeExecutor.BoardDelegate = _boardDelegate;
            runtimeExecutor.RobotDelegatePool = _robotDelegatePool;
            runtimeExecutor.PlayerPool = _playerPool;
            runtimeExecutor.BitDispenser = _bitDispenser;
            runtimeExecutor.AnimationCoroutineManager = _animationCoroutineManager;
            runtimeExecutor.OnRoundNumberChange.AddListener((_, round) => inGameUI.gameturn.SetTurn(round));
        }

        private void PrepareRobotDelegate(
            IRobotDelegate robotDelegate,
            Coordinate position,
            RobotDirection direction,
            GameObject prefab)
        {
            _boardDelegate.Add(robotDelegate, position, direction);
            Vector3 vectorPosition = ConvertToVector(position, 0.5f);
            Quaternion quaternion = Quaternion.Euler(0, (int) direction * 90f, 0);
            GameObject robotObject = Instantiate(prefab, vectorPosition, quaternion, transform);
            robotObject.name = robotDelegate.Id;
            robotObject.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
            LilbotController lilbotController = robotObject.AddComponent<LilbotController>();
            lilbotController.animator = robotObject.GetComponent<Animator>();
            robotDelegate.OnRobotChangePosition.AddListener((_, previous, next) =>
            {
                Debug.LogFormat("Robot {2} moved from {0} to {1}", previous, next, robotDelegate.Id);
                LilbotController controller = robotObject.GetComponent<LilbotController>();
                Vector3 end = ConvertToVector(next, 0.5f);
                IEnumerator controllerAnimation = controller.Walk(0.5f, (int) end.x, (int) end.z);
                _animationCoroutineManager.AddAnimation(robotObject, controllerAnimation);
            });
            robotDelegate.OnRobotChangeDirection.AddListener((_, previous, next) =>
            {
                Debug.LogFormat("Robot rotated from {0} to {1}", previous, next);
                Quaternion start = Quaternion.Euler(0, (int) previous * 90f, 0);
                Quaternion end = Quaternion.Euler(0, (int) next * 90f, 0);
                RotateAnimation rotateAnimation = new RotateAnimation(robotObject, start, end, 0.125f);
                _animationCoroutineManager.AddAnimation(robotObject, rotateAnimation);
            });
        }

        private void CreateBadSectorInstance(IBadSectorDelegate badSectorDelegate)
        {
            GameObject? prefab = Resources.Load<GameObject>(badSectorDelegate.Id);
            if (ReferenceEquals(prefab, null))
            {
                throw new NullReferenceException("Cannot find prefab: " + badSectorDelegate.Id);
            }

            Coordinate coordinate = badSectorDelegate.Position;
            Vector3 position = ConvertToVector(coordinate, 0.5f);
            GameObject badSectorObject = Instantiate(prefab, position, Quaternion.identity, transform);
            badSectorObject.name = $"{badSectorDelegate.Installer.Id}${badSectorDelegate.Id}";
            _boardDelegate.OnBadSectorRemove.AddListener(b =>
            {
                if (b == badSectorDelegate)
                {
                    Destroy(badSectorObject);
                }
            });
        }

        private void CreateBitInstance(IPlaceable placeable)
        {
            if (placeable is not IBitDelegate bitDelegate)
            {
                return;
            }

            Coordinate coordinate = bitDelegate.Position;
            Vector3 position = ConvertToVector(coordinate, 1.5f);
            GameObject bitGameObject = Instantiate(bitPrefab, position, Quaternion.Euler(90f, 0, 0), transform);
            bitDelegate.OnRobotTakeInEvents.AddListener(_ => bitGameObject.SetActive(false));
            bitDelegate.OnRobotTakeAwayEvents.AddListener(_ => bitGameObject.SetActive(true));
            _boardDelegate.OnPlaceableRemove.AddListener(p =>
            {
                if (p == bitDelegate)
                {
                    MoveAndRotate moveAndRotate = bitGameObject.GetComponent<MoveAndRotate>();
                    moveAndRotate.GetBit();
                }
            });
        }

        private void InitializeCells()
        {
            for (int i = 0; i < _boardDelegate.Width; i++)
            {
                for (int j = 0; j < _boardDelegate.Height; j++)
                {
                    Vector3 position = ConvertToVector(new Coordinate(i, j), 0);
                    Instantiate(boardCellPrefab, position, Quaternion.identity, transform);
                }
            }
        }

        private PlayerStatusUI? FindPlayerStatusUI(IPlayerDelegate playerDelegate)
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

        private void UpdatePlayerRanks()
        {
            IPlayerDelegate[] playerDelegates = _playerPool.ToArray();
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

        private static readonly IPlayerDelegate[] MockPlayerDelegates =
        {
            new PlayerDelegateCreateFactory(new PlayerDelegateCreateStrategy("Player1")).Build(),
            new PlayerDelegateCreateFactory(new PlayerDelegateCreateStrategy("Player2")).Build(),
            new PlayerDelegateCreateFactory(new PlayerDelegateCreateStrategy("Player3")).Build(),
            new PlayerDelegateCreateFactory(new PlayerDelegateCreateStrategy("Player4")).Build()
        };

        private static Vector3 ConvertToVector(Coordinate coordinate, float heightOffset)
        {
            return new Vector3(coordinate.X, heightOffset, coordinate.Y);
        }

        private class TestCommand : AbstractCommand, ICommand
        {
            public TestCommand() : base("0", "TestCommand", 0, 0) {}

            public override bool Invoke(params object[] args)
            {
                throw new NotImplementedException();
            }

            public override bool Revoke(params object[] args)
            {
                throw new NotImplementedException();
            }

            public override IList<IStatement> GetCommandStatements(IRobotDelegate robotDelegate)
            {
                return new List<IStatement>
                {
                    new MoveStatement(robotDelegate, 1),
                    new RotateStatement(robotDelegate, 1)
                };
            }

            public override ICommand Copy(bool keepStatus = true)
            {
                throw new NotImplementedException();
            }
        }
    }
}
