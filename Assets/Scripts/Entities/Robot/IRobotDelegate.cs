#nullable enable


namespace CodingStrategy.Entities.Robot
{
    public interface IRobotDelegate : IPlaceable
    {
        public abstract string Id { get; }

        public abstract IAlgorithm Algorithm { get; }

        public abstract RobotDirection Direction { get; set; }

        public abstract int HealthPoint { get; set; }

        public abstract int EnergyPoint { get; set; }

        public abstract int ArmorPoint { get; set; }

        public abstract int AttackPoint { get; set; }

        public abstract bool Rotate(int count);

        public abstract bool Move(int count);
    }
}
