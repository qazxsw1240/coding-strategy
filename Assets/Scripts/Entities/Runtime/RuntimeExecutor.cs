#nullable enable


namespace CodingStrategy.Entities.Runtime
{
    using System.Collections.Generic;
    using Board;
    using Robot;

    public class RuntimeExecutor : ILifeCycle
    {
        private readonly IBoardDelegate _boardDelegate;
        private readonly int _countdown;
        private readonly ExecutionQueuePool _executionQueuePool;
        private readonly IList<IExecutionValidator> _validators;

        private int _currentCountdown;

        public RuntimeExecutor(IBoardDelegate boardDelegate, int countdown)
        {
            _boardDelegate = boardDelegate;
            _countdown = countdown;
            _executionQueuePool = new ExecutionQueuePool();
            _validators = new List<IExecutionValidator>();

            _currentCountdown = 0;
        }

        public void Initialize()
        {
            // Place robots
            // Place bad sectors
            // Inject abnormality code
            // Initialize ExecutionQueue
            _executionQueuePool.Clear();
            // Initialize players' data
        }

        public bool MoveNext()
        {
            // Check deadlock
            return _executionQueuePool.Count != 0 && ++_currentCountdown <= _countdown;
        }

        public bool Execute()
        {
            // Check deadlock
            if (_executionQueuePool.Count == 0)
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
                // execute command
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
    }
}
