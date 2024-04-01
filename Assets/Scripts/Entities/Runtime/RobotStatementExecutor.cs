#nullable enable


namespace CodingStrategy.Runtime
{
    using System;
    using System.Collections.Generic;
    using CodingStrategy.Entities;
    using CodingStrategy.Entities.Board;
    using CodingStrategy.Entities.Robot;
    using Unity.VisualScripting;

    public class RobotStatementExecutor : ILifeCycle
    {
        private readonly IBoardDelegate _boardDelegate;
        private readonly ExecutionQueuePool _executionQueuePool;
        private readonly IExecutionValidator _validator;
        private readonly IDictionary<IRobotDelegate, Stack<IStatement>> _statements;

        public RobotStatementExecutor(
            IBoardDelegate boardDelegate,
            ExecutionQueuePool executionQueuePool,
            IExecutionValidator validator)
        {
            _boardDelegate = boardDelegate;
            _executionQueuePool = executionQueuePool;
            _validator = validator;
            _statements = new Dictionary<IRobotDelegate, Stack<IStatement>>();
        }

        public void Initialize()
        {
            CheckExecutionQueueValidity();
            InitializeStacks();
        }

        public bool MoveNext() => !IsQueueEmpty();

        public bool Execute()
        {
            IDictionary<IRobotDelegate, bool> problematicRobots = new Dictionary<IRobotDelegate, bool>();

            foreach ((IRobotDelegate robotDelegate, IExecutionQueue executionQueue) in _executionQueuePool)
            {
                problematicRobots[robotDelegate] = false;

                if (!executionQueue.TryDequeue(out IStatement statement))
                {
                    continue;
                }

                if (!_statements.ContainsKey(robotDelegate))
                {
                    _statements[robotDelegate] = new Stack<IStatement>();
                }

                Stack<IStatement> statements = _statements[robotDelegate];

                try
                {
                    statements.Push(statement);
                    statement.Execute();
                    if (!_validator.IsValid(_boardDelegate))
                    {
                        throw new ExecutionException();
                    }
                }
                catch (ExecutionException)
                {
                    RollbackCommands(executionQueue, statements);
                    problematicRobots[robotDelegate] = true;
                }
            }

            foreach ((IRobotDelegate robotDelegate, bool hasProblems) in problematicRobots)
            {
                if (hasProblems)
                {
                    _executionQueuePool.Remove(robotDelegate);
                }
            }

            return true;
        }

        public void Terminate() => CheckExecutionQueueValidity();

        private bool IsQueueEmpty()
        {
            foreach (IExecutionQueue executionQueue in _executionQueuePool.Values)
            {
                if (executionQueue.Count != 0)
                {
                    return false;
                }
            }
            return true;
        }

        private void CheckExecutionQueueValidity()
        {
            if (!IsQueueEmpty())
            {
                throw new RuntimeException("Queue is not empty");
            }
        }

        private void InitializeStacks()
        {
            foreach (IRobotDelegate robotDelegate in _executionQueuePool.Keys)
            {
                _statements[robotDelegate] = new Stack<IStatement>();
            }
        }

        private void RollbackCommands(IExecutionQueue executionQueue, Stack<IStatement> statements)
        {
            executionQueue.Clear();
            while (statements.TryPop(out IStatement statement))
            {
                executionQueue.Enqueue(statement.Reverse);
            }
        }
    }
}
