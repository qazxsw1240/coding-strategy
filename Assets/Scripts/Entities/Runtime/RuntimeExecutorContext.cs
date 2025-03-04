#nullable enable

using CodingStrategy.Entities.Board;
using CodingStrategy.Entities.Player;
using CodingStrategy.Entities.Robot;

namespace CodingStrategy.Entities.Runtime
{
    public class RuntimeExecutorContext
    {
        public RuntimeExecutorContext(
            IBoardDelegate boardDelegate,
            IPlayerPool playerPool,
            IRobotDelegatePool robotDelegatePool,
            ExecutionQueuePool executionQueuePool,
            AnimationCoroutineManager animationCoroutineManager)
        {
            BoardDelegate = boardDelegate;
            PlayerPool = playerPool;
            RobotDelegatePool = robotDelegatePool;
            ExecutionQueuePool = executionQueuePool;
            AnimationCoroutineManager = animationCoroutineManager;
        }

        public IBoardDelegate BoardDelegate { get; }

        public IPlayerPool PlayerPool { get; }

        public IRobotDelegatePool RobotDelegatePool { get; }

        public ExecutionQueuePool ExecutionQueuePool { get; }

        public AnimationCoroutineManager AnimationCoroutineManager { get; }
    }
}
