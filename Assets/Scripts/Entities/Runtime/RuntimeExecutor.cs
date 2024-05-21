#nullable enable


using System;
using System.Linq;
using CodingStrategy.Entities.BadSector;
using CodingStrategy.Entities.Placeable;
using CodingStrategy.Entities.Runtime.Validator;
using Unity.VisualScripting;

namespace CodingStrategy.Entities.Runtime
{
    using Animations;
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using Board;
    using Player;
    using Robot;

    public class RuntimeExecutor : LifeCycleMonoBehaviourBase, ILifeCycle
    {
        private static readonly (RobotDirection, Coordinate)[] StartPositions =
        {
            (RobotDirection.North, new Coordinate(4, 0)),
            (RobotDirection.East, new Coordinate(0, 4)),
            (RobotDirection.South, new Coordinate(4, 8)),
            (RobotDirection.West, new Coordinate(8, 4))
        };

        private readonly int _countdown = 20;

        private readonly ExecutionQueuePool _executionQueuePool = new ExecutionQueuePool();

        private readonly IList<IExecutionValidator> _validators = new List<IExecutionValidator>
        {
            new MoveValidator(),
            new PointerValidator(),
            new StaticValidator(),
            new AttackValidator()
        };

        private readonly IDictionary<IRobotDelegate, GameObject> _robotObjects =
            new Dictionary<IRobotDelegate, GameObject>();

        private GameObject[,] _cellObjects = null!;
        // private LevelController _levelController = null!;

        private int _currentCountdown;

        public GameObject BoardCellPrefab = null!;
        public GameObject RobotPrefab = null!;
        public GameObject BitPrefab = null!;

        public IBoardDelegate BoardDelegate { private get; set; } = null!;

        public IRobotDelegatePool RobotDelegatePool { private get; set; } = null!;

        public IPlayerPool PlayerPool { private get; set; } = null!;

        public BitDispenser BitDispenser { private get; set; } = null!;

        public AnimationCoroutineManager AnimationCoroutineManager { private get; set; } = null!;

        public List<GameObject> RobotPrefabs { get; set; } = null!;

        public void Awake()
        {
            LifeCycle = this;
        }

        public void Initialize()
        {
            // Place robots
            // Place bad sectors
            // Inject abnormality code
            // Initialize ExecutionQueue
            _executionQueuePool.Clear();
            _cellObjects = new GameObject[BoardDelegate.Width, BoardDelegate.Height];
            BoardDelegate.OnRobotAdd.AddListener(CreateRobotInstance);
            BoardDelegate.OnBadSectorAdd.AddListener(CreateBadSectorInstance);
            BoardDelegate.OnPlaceableAdd.AddListener(CreateBitInstance);
            BoardDelegate.OnRobotRemove.AddListener(RemoveRobotInstance);

            InitializeCells();

            foreach ((int index, IPlayerDelegate playerDelegate) in ToIndexedEnumerable(PlayerPool))
            {
                (RobotDirection direction, Coordinate position) = StartPositions[index];
                IRobotDelegate robotDelegate = RobotDelegatePool[playerDelegate.Id];
                BoardDelegate.Add(robotDelegate, position, direction);
                _executionQueuePool[robotDelegate] = new ExecutionQueueImpl();
            }

            BitDispenser.Dispense();
        }

        public bool MoveNext()
        {
            // Check deadlock
            bool result = !IsDeadlock() && ++_currentCountdown <= _countdown;
            Debug.LogFormat("Check MoveNext() in RuntimeExecutor {0}", result);
            return result;
        }

        public bool Execute()
        {
            // Check deadlock
            if (IsDeadlock())
            {
                return false;
            }

            foreach ((IRobotDelegate robotDelegate, IExecutionQueue executionQueue) in _executionQueuePool)
            {
                IAlgorithm algorithm = robotDelegate.Algorithm;
                int capacity = algorithm.Capacity;
                if (capacity == 0)
                {
                    continue;
                }

                ICommand command = algorithm[_currentCountdown % algorithm.Count];
                foreach (IStatement statement in command.GetCommandStatements(robotDelegate))
                {
                    executionQueue.Add(statement);
                }
            }

            return true;
        }

        public void Terminate()
        {
            foreach (IExecutionQueue executionQueue in _executionQueuePool.Values)
            {
                executionQueue.Clear();
            }

            _executionQueuePool.Clear();
        }

        protected override IEnumerator OnAfterInitialization()
        {
            Debug.Log("RuntimeExecutor Started.");
            yield return null;
        }

        protected override IEnumerator OnBeforeExecution()
        {
            yield return null;
        }

        protected override IEnumerator OnAfterFailExecution()
        {
            yield return null;
        }

        protected override IEnumerator OnAfterExecution()
        {
            foreach (IExecutionValidator validator in _validators)
            {
                if (IsDeadlock() || IsExecutionQueueEmpty(_executionQueuePool))
                {
                    break;
                }

                RobotStatementExecutor robotStatementExecutor = gameObject.GetOrAddComponent<RobotStatementExecutor>();
                robotStatementExecutor.Validator = validator;
                robotStatementExecutor.Context = new RuntimeExecutorContext(BoardDelegate,
                    PlayerPool,
                    RobotDelegatePool,
                    _executionQueuePool,
                    AnimationCoroutineManager);
                yield return AwaitLifeCycleCoroutine(robotStatementExecutor);
                Destroy(robotStatementExecutor);

                BitDispenser.ClearTakenBits();
            }
        }

        protected override IEnumerator OnAfterTermination()
        {
            BitDispenser.Clear();
            Debug.Log("RuntimeExecutor Terminated.");
            yield return null;
        }

        private bool IsDeadlock()
        {
            return _executionQueuePool.Count == 0;
        }

        private void InitializeCells()
        {
            for (int i = 0; i < BoardDelegate.Width; i++)
            {
                for (int j = 0; j < BoardDelegate.Height; j++)
                {
                    Vector3 position = ConvertToVector(new Coordinate(i, j), 0);
                    _cellObjects[i, j] = Instantiate(BoardCellPrefab, position, Quaternion.identity, transform);
                }
            }
        }

        private Vector3 ConvertToVector(Coordinate coordinate, float heightOffset)
        {
            // int halfWidth = BoardDelegate.Width / 2;
            // int halfHeight = BoardDelegate.Height / 2;
            return new Vector3(coordinate.X, heightOffset, coordinate.Y);
        }

        private void CreateRobotInstance(IRobotDelegate robotDelegate)
        {
            Coordinate coordinate = robotDelegate.Position;
            RobotDirection direction = robotDelegate.Direction;
            Vector3 position = ConvertToVector(coordinate, 0.5f);
            Quaternion quaternion = Quaternion.Euler(0, (int) direction * 90f, 0);
            GameObject robotPrefab = RobotPrefabs[int.Parse(robotDelegate.Id) - 1];
            GameObject robotObject = Instantiate(robotPrefab, position, quaternion, transform);
            robotObject.name = robotDelegate.Id;
            robotObject.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
            _robotObjects[robotDelegate] = robotObject;
            robotDelegate.OnRobotChangePosition.AddListener((_, previous, next) =>
            {
                Debug.LogFormat("Robot {2} moved from {0} to {1}", previous, next, robotDelegate.Id);
                Vector3 start = ConvertToVector(previous, 0.5f);
                Vector3 end = ConvertToVector(next, 0.5f);
                MoveAnimation moveAnimation = new MoveAnimation(robotObject, start, end, 0.25f);
                AnimationCoroutineManager.AddAnimation(robotObject, moveAnimation);
            });
            robotDelegate.OnRobotChangeDirection.AddListener((_, previous, next) =>
            {
                Debug.LogFormat("Robot rotated from {0} to {1}", previous, next);
                Quaternion start = Quaternion.Euler(0, (int) previous * 90f, 0);
                Quaternion end = Quaternion.Euler(0, (int) next * 90f, 0);
                RotateAnimation rotateAnimation = new RotateAnimation(robotObject, start, end, 0.125f);
                AnimationCoroutineManager.AddAnimation(robotObject, rotateAnimation);
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
            BoardDelegate.OnBadSectorRemove.AddListener(b =>
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
            GameObject bitGameObject = Instantiate(BitPrefab, position, Quaternion.Euler(90f, 0, 0), transform);
            bitDelegate.OnRobotTakeInEvents.AddListener(_ => bitGameObject.SetActive(false));
            bitDelegate.OnRobotTakeAwayEvents.AddListener(_ => bitGameObject.SetActive(true));
            BoardDelegate.OnPlaceableRemove.AddListener(p =>
            {
                if (p == bitDelegate)
                {
                    Destroy(bitGameObject);
                }
            });
        }

        private void RemoveRobotInstance(IRobotDelegate robotDelegate)
        {
            if (!_robotObjects.TryGetValue(robotDelegate, out GameObject robotObject))
            {
                return;
            }

            Destroy(robotObject);

            _robotObjects.Remove(robotDelegate);
        }

        private static bool IsExecutionQueueEmpty(ExecutionQueuePool pool)
        {
            return pool.Values.All(executionQueue => executionQueue.Count == 0);
        }

        private static IEnumerable<(int, T)> ToIndexedEnumerable<T>(IEnumerable<T> enumerable)
        {
            int i = 0;
            foreach (T value in enumerable)
            {
                yield return (i++, value);
            }
        }
    }
}
