#nullable enable


using CodingStrategy.Entities.CodingTime;
using CodingStrategy.Entities.Runtime.Statement;
using CodingStrategy.Factory;

namespace CodingStrategy
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Entities;
    using Entities.Board;
    using Entities.Player;
    using Entities.Robot;
    using Entities.Runtime;
    using Unity.VisualScripting;
    using UnityEngine;

    public class GameManager : MonoBehaviour
    {
        public int boardWidth = 9;
        public int boardHeight = 9;

        private IBoardDelegate _boardDelegate = null!;
        private IRobotDelegatePool _robotDelegatePool = null!;
        private IPlayerPool _playerPool = null!;

        private AnimationCoroutineManager _animationCoroutineManager = null!;
        private BitDispenser _bitDispenser = null!;

        public InGameUI InGameUI = null!;
        public GameObject BoardCellPrefab = null!;
        public GameObject BitPrefab = null!;

        public List<GameObject> RobotPrefabs = new List<GameObject>();

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
        }

        public void Start()
        {
            Debug.Log("GameManager instance started.");
            StartCoroutine(StartGameManagerCoroutine());
        }

        private IEnumerator StartGameManagerCoroutine()
        {
            foreach (IPlayerDelegate playerDelegate in _playerPool)
            {
                playerDelegate.Algorithm.Add(new TestCommand());
            }

            CodingTimeExecutor codingTimeExecutor = gameObject.GetOrAddComponent<CodingTimeExecutor>();

            yield return LifeCycleMonoBehaviourBase.AwaitLifeCycleCoroutine(codingTimeExecutor);

            RuntimeExecutor runtimeExecutor = gameObject.GetOrAddComponent<RuntimeExecutor>();

            runtimeExecutor.BoardCellPrefab = BoardCellPrefab;
            runtimeExecutor.BitPrefab = BitPrefab;
            runtimeExecutor.RobotPrefabs = RobotPrefabs;
            runtimeExecutor.BoardDelegate = _boardDelegate;
            runtimeExecutor.RobotDelegatePool = _robotDelegatePool;
            runtimeExecutor.PlayerPool = _playerPool;
            runtimeExecutor.BitDispenser = _bitDispenser;
            runtimeExecutor.AnimationCoroutineManager = _animationCoroutineManager;

            yield return LifeCycleMonoBehaviourBase.AwaitLifeCycleCoroutine(runtimeExecutor);
        }

        private static readonly IPlayerDelegate[] MockPlayerDelegates =
        {
            new PlayerDelegateCreateFactory(new PlayerDelegateCreateStrategy("1")).Build(),
            new PlayerDelegateCreateFactory(new PlayerDelegateCreateStrategy("2")).Build(),
            new PlayerDelegateCreateFactory(new PlayerDelegateCreateStrategy("3")).Build(),
            new PlayerDelegateCreateFactory(new PlayerDelegateCreateStrategy("4")).Build()
        };

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
