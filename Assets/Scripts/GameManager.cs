#nullable enable


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

        public GameObject BoardCellPrefab = null!;
        public GameObject RobotPrefab = null!;
        public float BlockGap;

        public void Awake()
        {
            _boardDelegate = new BoardDelegateImpl(boardWidth, boardHeight);
            _robotDelegatePool = new RobotDelegatePoolImpl();
            _playerPool = new PlayerPoolImpl();
            Debug.Log("AnimationCoroutineManager created.");
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
            // CodingTimeExecutor codingTimeExecutor = gameObject.GetOrAddComponent<CodingTimeExecutor>();
            //
            // yield return LifeCycleMonoBehaviourBase.AwaitLifeCycleCoroutine(codingTimeExecutor);

            foreach (IPlayerDelegate playerDelegate in _playerPool)
            {
                playerDelegate.Algorithm.Add(new TestCommandBuilder()
                    .SetRobotDelegate(playerDelegate.Robot)
                    .SetDirection(1)
                    .Build());
            }

            RuntimeExecutor runtimeExecutor = gameObject.GetOrAddComponent<RuntimeExecutor>();

            runtimeExecutor.BoardCellPrefab = BoardCellPrefab;
            runtimeExecutor.RobotPrefab = RobotPrefab;
            runtimeExecutor.BlockGap = BlockGap;
            runtimeExecutor.BoardDelegate = _boardDelegate;
            runtimeExecutor.RobotDelegatePool = _robotDelegatePool;
            runtimeExecutor.PlayerPool = _playerPool;
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

        public class TestCommandBuilder
        {
            private IRobotDelegate? _robotDelegate;
            private int _direction;

            public TestCommandBuilder SetRobotDelegate(IRobotDelegate robotDelegate)
            {
                _robotDelegate = robotDelegate;
                return this;
            }

            public TestCommandBuilder SetDirection(int direction)
            {
                _direction = direction;
                return this;
            }

            public ICommand Build()
            {
                if (_direction == 0)
                {
                    throw new Exception();
                }

                return new TestCommand(_robotDelegate!, _direction);
            }
        }

        private class TestCommand : AbstractCommand, ICommand
        {
            private readonly IRobotDelegate _robotDelegate;
            private readonly int _direction;

            public TestCommand(IRobotDelegate robotDelegate, int direction) :
                base("0", "TestCommand", 0, 0)
            {
                _robotDelegate = robotDelegate;
                _direction = direction;
            }

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
                    new MoveStatement(_robotDelegate, _direction)
                };
            }

            public override ICommand Copy(bool keepStatus = true)
            {
                throw new NotImplementedException();
            }
        }
    }
}
