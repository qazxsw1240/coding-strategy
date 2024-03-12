#nullable enable


namespace CodingStrategy.Entities.Player
{
    using CodingStrategy.Entities.Robot;
    public interface IPlayerDelegate
    {
        public abstract int Id { get; }
        public abstract int HealthPoint { get; set; }
        public abstract int Level { get; set; }    
        public abstract int Exp { get; set; }
        public abstract int Currency { get; set; }
        public abstract IRobot Robot { get; }
        public abstract IAlgorithm Algorithm { get; }
    }
}
