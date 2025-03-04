using CodingStrategy.Entities.Board;
using CodingStrategy.Entities.Player;
using CodingStrategy.Entities.Robot;

namespace CodingStrategy.Entities.Runtime
{
    public interface ICommandContext
    {
        public abstract IBoardDelegate BoardDelegate { get; }

        public abstract IRobotDelegatePool RobotDelegatePool { get; }

        public abstract IPlayerPool PlayerPool { get; }

        public abstract IRobotDelegate RobotDelegate { get; }

        public abstract IPlayerDelegate PlayerDelegate { get; }
    }
}
