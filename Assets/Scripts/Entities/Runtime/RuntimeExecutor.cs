#nullable enable


using System.Linq;
using CodingStrategy.Entities.Runtime.Validator;
using Unity.VisualScripting;
using UnityEngine.Events;

namespace CodingStrategy.Entities.Runtime
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using Board;
    using Player;
    using Robot;

    public class RuntimeExecutor : LifeCycleMonoBehaviourBase, ILifeCycle
    {
        private readonly int _countdown = 20;

        private readonly ExecutionQueuePool _executionQueuePool = new ExecutionQueuePool();

        private readonly IList<IExecutionValidator> _validators = new List<IExecutionValidator>
        {
            new MoveValidator(),
            new PointerValidator(),
            new StaticValidator(),
            new AttackValidator()
        };

        private int _currentCountdown;

        public IBoardDelegate BoardDelegate { private get; set; } = null!;

        public IRobotDelegatePool RobotDelegatePool { private get; set; } = null!;

        public IPlayerPool PlayerPool { private get; set; } = null!;

        public BitDispenser BitDispenser { private get; set; } = null!;

        public AnimationCoroutineManager AnimationCoroutineManager { private get; set; } = null!;

        public UnityEvent<int, int> OnRoundNumberChange { get; } = new UnityEvent<int, int>();

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

            foreach (IPlayerDelegate playerDelegate in PlayerPool)
            {
                IRobotDelegate robotDelegate = RobotDelegatePool[playerDelegate.Id];
                _executionQueuePool[robotDelegate] = new ExecutionQueueImpl();
            }

            OnRoundNumberChange.Invoke(0, _countdown - _currentCountdown);
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

            foreach (IExecutionQueue executionQueue in _executionQueuePool.Values)
            {
                executionQueue.IsProtected = false;
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
            OnRoundNumberChange.Invoke(0, _countdown - _currentCountdown);
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

        private static bool IsExecutionQueueEmpty(ExecutionQueuePool pool)
        {
            return pool.Values.All(executionQueue => executionQueue.Count == 0);
        }
    }
}
