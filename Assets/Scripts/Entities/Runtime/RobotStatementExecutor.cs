#nullable enable

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using CodingStrategy.Entities.BadSector;
using CodingStrategy.Entities.Player;
using CodingStrategy.Entities.Robot;

using UnityEngine;
using UnityEngine.Events;

namespace CodingStrategy.Entities.Runtime
{
    public class RobotStatementExecutor : LifeCycleMonoBehaviourBase, ILifeCycle
    {
        private readonly ISet<IRobotDelegate> _problematicRobots = new HashSet<IRobotDelegate>();

        private readonly IDictionary<IRobotDelegate, Stack<IStatement>> _statements =
            new Dictionary<IRobotDelegate, Stack<IStatement>>();

        public IExecutionValidator Validator { get; set; } = null!;

        public GameManager GameManager { get; set; } = null!;

        public RuntimeExecutorContext Context { get; set; } = null!;

        public UnityEvent<IRobotDelegate, IPlayerDelegate, IStatement> OnStatementExecuteEvents { get; } =
            new UnityEvent<IRobotDelegate, IPlayerDelegate, IStatement>();

        public void Awake()
        {
            LifeCycle = this;
        }

        public void Initialize()
        {
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
                }
            }
            IList<IRobotDelegate> invalidRobots = Validator.GetInvalidRobots(Context.BoardDelegate);
            foreach ((IRobotDelegate robotDelegate, IExecutionQueue executionQueue) in Context.ExecutionQueuePool)
            {
                if (invalidRobots.Contains(robotDelegate))
                {
                    continue;
                }
                // BadSector Check
                Stack<IStatement> statements = _statements[robotDelegate];
                IBadSectorDelegate? badSectorDelegate =
                    Context.BoardDelegate.GetBadSectorDelegate(robotDelegate.Position);
                Debug.LogErrorFormat("bad sector {0} found", badSectorDelegate?.Id);
                if (!ReferenceEquals(badSectorDelegate, null) && badSectorDelegate.Installer.Id != robotDelegate.Id)
                {
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
                if (robotDelegate.HealthPoint > 0)
                {
                    continue;
                }
                statements.Clear();
                executionQueue.Clear();
                Context.AnimationCoroutineManager.ClearAnimationQueue(robotDelegate);
                if (robotDelegate.Id == GameManager.util.LocalPhotonPlayerDelegate.Id)
                {
                    GameManager.util.LocalPhotonPlayerDelegate.HealthPoint -= 1;
                }
                else
                {
                    robotDelegate.HealthPoint -= 1;
                }
            }

            foreach (IRobotDelegate robotDelegate in invalidRobots)
            {
                if (Context.ExecutionQueuePool.TryGetValue(robotDelegate, out IExecutionQueue executionQueue))
                {
                    Stack<IStatement> statements = _statements[robotDelegate];
                    RollbackCommands(executionQueue, statements);
                }

                _problematicRobots.Add(robotDelegate);
            }

            foreach (IRobotDelegate robotDelegate in _problematicRobots)
            {
                if (!Context.ExecutionQueuePool.TryGetValue(robotDelegate, out IExecutionQueue executionQueue))
                {
                    continue;
                }
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
                if (Context.ExecutionQueuePool.TryGetValue(problematicRobot, out IExecutionQueue executionQueue))
                {
                    if (executionQueue.Count == 0)
                    {
                        Context.ExecutionQueuePool.Remove(problematicRobot);
                    }
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
