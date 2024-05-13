#nullable enable


namespace CodingStrategy.Factory
{
    using Entities.Robot;

    public interface IRobotDelegateCreateFactory
    {
        public abstract IRobotDelegateCreateStrategy Strategy { get; }

        public abstract IRobotDelegate Build();
    }
}
