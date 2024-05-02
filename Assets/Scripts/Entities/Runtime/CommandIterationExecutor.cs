#nullable enable


namespace CodingStrategy.Entities.Runtime
{
    using System.Collections.Generic;
    using CodingStrategy.Entities.Board;

    public class CommandIterationExecutor : ILifeCycle
    {
        private readonly IBoardDelegate _boardDelegate;
        private readonly ExecutionQueuePool _executionQueuePool;
        private readonly IReadOnlyList<IExecutionValidator> _validators;
        private readonly IEnumerator<IExecutionValidator> _enumerator;

        public CommandIterationExecutor(
            IBoardDelegate boardDelegate,
            ExecutionQueuePool executionQueuePool,
            IReadOnlyList<IExecutionValidator> validators)
        {
            _boardDelegate = boardDelegate;
            _executionQueuePool = executionQueuePool;
            _validators = validators;
            _enumerator = _validators.GetEnumerator();
        }

        public void Initialize() => _enumerator.Reset();

        public bool MoveNext() => _enumerator.MoveNext();

        public bool Execute()
        {
            IExecutionValidator validator = _enumerator.Current;
            RobotStatementExecutor executor =
                new RobotStatementExecutor(_boardDelegate, _executionQueuePool, validator);
            executor.Initialize();
            while (executor.MoveNext())
            {
                if (!executor.Execute())
                {
                    return false;
                }
            }

            executor.Terminate();
            return true;
        }

        public void Terminate() => _enumerator.Dispose();
    }
}
