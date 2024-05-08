#nullable enable


namespace CodingStrategy.Entities.Runtime
{
    using Player;
    using Robot;
    using Board;

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
