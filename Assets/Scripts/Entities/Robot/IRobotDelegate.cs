#nullable enable


namespace CodingStrategy.Entities.Robot
{
    public interface IRobotDelegate
    {
        public abstract string Id { get; }

        public abstract IAlgorithm Algorithm { get; }

        public abstract Coordinate Position { get; set; }

        public abstract RobotDirection Direction { get; set; }

        public abstract int HeathPoint { get; set; }

        public abstract int EnergyPoint { get; set; }

        public abstract int ArmorPoint { get; set; }

        public abstract int AttackPoint { get; set; }

        public abstract bool Rotate(int count);

        public abstract bool Move(int count);
    }
}
