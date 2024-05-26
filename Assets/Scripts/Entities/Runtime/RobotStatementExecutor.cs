#nullable enable


using System;
using CodingStrategy.Entities.BadSector;
using CodingStrategy.Entities.Board;
using CodingStrategy.Entities.Player;
using UnityEngine;
using UnityEngine.Events;

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

        private readonly ISet<IRobotDelegate> _problematicRobots = new HashSet<IRobotDelegate>();

        public IExecutionValidator Validator { get; set; } = null!;

        public RuntimeExecutorContext Context { get; set; } = null!;

        public UnityEvent<IRobotDelegate, IPlayerDelegate,  IStatement> OnStatementExecuteEvents { get; } =
            new UnityEvent<IRobotDelegate, IPlayerDelegate, IStatement>();

        public void Awake()
        {
            LifeCycle = this;
        }

        public void Initialize()
        {
            // CheckExecutionQueueValidity();
            InitializeStacks();
        }

        public bool MoveNext()
        {
            return !IsQueueEmpty();
        }

        public bool Execute()
        {
            foreach ((IRobotDelegate robotDelegate, IExecutionQueue executionQueue) in Context.ExecutionQueuePool)
            {
                if (!executionQueue.TryDequeue(out IStatement? statement))
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
                    statements.Push(statement!);
                    statement!.Execute(Context);
                    IPlayerDelegate playerDelegate = Context.PlayerPool[robotDelegate.Id];
                    OnStatementExecuteEvents.Invoke(robotDelegate, playerDelegate, statement);
                }
                catch (ExecutionException)
                {
                    RollbackCommands(executionQueue, statements);
                    _problematicRobots.Add(robotDelegate);
                    continue;
                }

                // BadSector Check

                IBadSectorDelegate? badSectorDelegate =
                    Context.BoardDelegate.GetBadSectorDelegate(robotDelegate.Position);

                if (ReferenceEquals(badSectorDelegate, null) || badSectorDelegate.Installer == robotDelegate)
                {
                    continue;
                }

                if (executionQueue.IsProtected)
                {
                    continue;
                }

                badSectorDelegate.Remove();
                statements.Clear();
                executionQueue.Clear();

                IList<IStatement> badSectorStatements = badSectorDelegate.Execute(robotDelegate);

                foreach (IStatement s in badSectorStatements.Reverse())
                {
                    executionQueue.EnqueueFirst(s);
                }
            }

            IList<IRobotDelegate> invalidRobots = Validator.GetInvalidRobots(Context.BoardDelegate);

            if (invalidRobots.Count == 0)
            {
                return true;
            }

            foreach (IRobotDelegate robotDelegate in invalidRobots)
            {
                IExecutionQueue executionQueue = Context.ExecutionQueuePool[robotDelegate];
                Stack<IStatement> statements = _statements[robotDelegate];
                RollbackCommands(executionQueue, statements);
                _problematicRobots.Add(robotDelegate);
            }

            foreach (IRobotDelegate robotDelegate in _problematicRobots)
            {
                IExecutionQueue executionQueue = Context.ExecutionQueuePool[robotDelegate];
                if (executionQueue.Count == 0)
                {
                    Context.ExecutionQueuePool.Remove(robotDelegate);
                }
            }

            return true;
        }

        public void Terminate()
        {
            _problematicRobots.Clear();
            CheckExecutionQueueValidity();
        }

        private bool IsQueueEmpty()
        {
            return Context.ExecutionQueuePool.Count == 0 ||
                   Context.ExecutionQueuePool.Values.All(executionQueue => executionQueue.Count == 0);
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

        private void RollbackCommands(IExecutionQueue executionQueue, Stack<IStatement> statements)
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
            foreach (IRobotDelegate problematicRobot in _problematicRobots)
            {
                IExecutionQueue executionQueue = Context.ExecutionQueuePool[problematicRobot];
                if (executionQueue.Count == 0)
                {
                    Context.ExecutionQueuePool.Remove(problematicRobot);
                }
            }

            yield return Context.AnimationCoroutineManager.ApplyAnimations();
        }

        protected override IEnumerator OnAfterTermination()
        {
            yield return null;
        }
    }
}
