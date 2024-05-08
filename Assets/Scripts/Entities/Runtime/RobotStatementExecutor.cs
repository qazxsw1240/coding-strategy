#nullable enable


namespace CodingStrategy.Entities.Runtime
{
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;
    using Robot;

    public class RobotStatementExecutor : LifeCycleMonoBehaviourBase, ILifeCycle
    {
        private readonly IDictionary<IRobotDelegate, Stack<IStatement>> _statements =
            new Dictionary<IRobotDelegate, Stack<IStatement>>();

        public IExecutionValidator Validator { get; set; } = null!;

        public RuntimeExecutorContext Context { get; set; } = null!;

        public void Awake()
        {
            LifeCycle = this;
        }

        public void Initialize()
        {
            CheckExecutionQueueValidity();
            InitializeStacks();
        }

        public bool MoveNext() => true;

        public bool Execute()
        {
            IDictionary<IRobotDelegate, bool> problematicRobots =
                new Dictionary<IRobotDelegate, bool>();

            foreach ((IRobotDelegate robotDelegate, IExecutionQueue executionQueue) in Context.ExecutionQueuePool)
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
                    if (!Validator.IsValid(Context.BoardDelegate))
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
                    Context.ExecutionQueuePool.Remove(robotDelegate);
                }
            }

            return true;
        }

        public void Terminate() => CheckExecutionQueueValidity();

        private bool IsQueueEmpty()
        {
            return Context.ExecutionQueuePool.Values.All(executionQueue => executionQueue.Count == 0);
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
            foreach (IRobotDelegate robotDelegate in Context.ExecutionQueuePool.Keys)
            {
                _statements[robotDelegate] = new Stack<IStatement>();
            }
        }

        private void RollbackCommands(IExecutionQueue executionQueue,
            Stack<IStatement> statements)
        {
            executionQueue.Clear();
            while (statements.TryPop(out IStatement statement))
            {
                executionQueue.Enqueue(statement.Reverse);
            }
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
            yield return Context.AnimationCoroutineManager.ApplyAnimations();
        }

        protected override IEnumerator OnAfterTermination()
        {
            yield return null;
        }
    }
}
