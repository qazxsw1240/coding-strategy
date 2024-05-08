#nullable enable


namespace CodingStrategy.Entities.Runtime
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Board;
    using Player;
    using Robot;

    public class RuntimeExecutor : LifeCycleMonoBehaviourBase, ILifeCycle
    {
        private int _countdown = 20;

        private ExecutionQueuePool _executionQueuePool =
            new ExecutionQueuePool();

        private IList<IExecutionValidator> _validators =
            new List<IExecutionValidator>();

        private LevelController _levelController = null!;

        private int _currentCountdown = 0;

        public IBoardDelegate BoardDelegate { private get; set; } = null!;

        public IRobotDelegatePool RobotDelegatePool { private get; set; } =
            null!;

        public IPlayerPool PlayerPool { private get; set; } = null!;

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
            foreach (IPlayerDelegate playerDelegate in PlayerPool.PlayerPool.Values)
            {
                IRobotDelegate robotDelegate = RobotDelegatePool[playerDelegate.Id];
                _executionQueuePool[robotDelegate] = new ExecutionQueueImpl();
            }
        }

        public bool MoveNext()
        {
            // Check deadlock
            return !IsDeadlock() && ++_currentCountdown <= _countdown;
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
                int capacity = algorithm.Count;
                if (capacity == 0)
                {
                    continue;
                }

                ICommand command = algorithm[_currentCountdown % capacity];
                foreach (IStatement statement in command.GetCommandStatements())
                {
                    executionQueue.Add(statement);
                }
            }

            ILifeCycle commandIterationExecutor = gameObject.AddComponent<CommandIterationExecutor>();
            commandIterationExecutor.Initialize();
            while (commandIterationExecutor.MoveNext())
            {
                commandIterationExecutor.Execute();
            }

            commandIterationExecutor.Terminate();

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
            yield return null;
        }

        protected override IEnumerator OnAfterTermination()
        {
            yield return null;
        }

        private bool IsDeadlock()
        {
            return _executionQueuePool.Count == 0;
        }
    }
}
