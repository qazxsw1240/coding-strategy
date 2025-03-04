#nullable enable

using CodingStrategy.Entities.Robot;

namespace CodingStrategy.Factory
{
    public interface IRobotDelegateCreateFactory
    {
        public abstract IRobotDelegateCreateStrategy Strategy { get; }

        public abstract IRobotDelegate Build();
    }
}
