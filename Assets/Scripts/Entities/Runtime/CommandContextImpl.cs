using CodingStrategy.Entities.Board;
using CodingStrategy.Entities.Player;
using CodingStrategy.Entities.Robot;

namespace CodingStrategy.Entities.Runtime
{
    public class CommandContextImpl : ICommandContext
    {
        public IBoardDelegate BoardDelegate { get; }

        public IRobotDelegatePool RobotDelegatePool { get; }

        public IPlayerPool PlayerPool { get; }

        public IRobotDelegate RobotDelegate { get; }

        public IPlayerDelegate PlayerDelegate { get; }

        public CommandContextImpl(
            IBoardDelegate boardDelegate,
            IRobotDelegatePool robotDelegatePool,
            IPlayerPool playerPool,
            IRobotDelegate robotDelegate,
            IPlayerDelegate playerDelegate)
        {
            BoardDelegate = boardDelegate;
            RobotDelegatePool = robotDelegatePool;
            PlayerPool = playerPool;
            RobotDelegate = robotDelegate;
            PlayerDelegate = playerDelegate;
        }
    }
}
