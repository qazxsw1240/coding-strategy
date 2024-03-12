#nullable enable

namespace CodingStrategy.Entities.Player
{
    using CodingStrategy.Entities.Robot;
    public interface IPlayer
    {
        public abstract int Id { get; }
        public abstract int HealthPoint { get; }
        public abstract int Level { get; }    
        public abstract int Exp { get; }
        public abstract int Currency { get; }
        public abstract IRobot Robot { get; }
        public abstract IAlgorithm Algorithm { get; }
    }
}
