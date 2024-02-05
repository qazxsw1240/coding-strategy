#nullable enable


namespace CodingStrategy.Entities.Robot
{
    public interface IRobot
    {
        public abstract string Id { get; }

        public abstract IAlgorithm Algorithm { get; }

        public abstract Coordinate Position { get; }

        public abstract RobotDirection Direction { get; }

        public abstract int HealthPoint { get; }

        public abstract int EnergyPoint { get; }

        public abstract int ArmorPoint { get; }

        public abstract int AttackPoint { get; }

        public abstract bool Rotate(int count);

        public abstract bool Move(int count);
    }
}
