#nullable enable


namespace CodingStrategy.Entities.Robot
{
    public interface IRobotPool
    {
        public abstract IRobotDelegate this[string id] { get; }
    }
}
