#nullable enable


namespace CodingStrategy.Entities.Robot
{
    using UnityEngine.Events;

    public interface IRobot : IGameEntity
    {
        public abstract IRobotDelegate Delegate { get; }

        public abstract IAlgorithm Algorithm { get; }

        public abstract Coordinate Position { get; }

        public abstract RobotDirection Direction { get; }

        public abstract int HealthPoint { get; }

        public abstract int EnergyPoint { get; }

        public abstract int ArmorPoint { get; }

        public abstract int AttackPoint { get; }

        public abstract bool Move(int count);

        public abstract bool Move(Coordinate position);

        public abstract bool Rotate(int count);

        public abstract bool Rotate(RobotDirection direction);

        public abstract UnityEvent<Coordinate, Coordinate> OnRobotChangePosition { get; }

        public abstract UnityEvent<RobotDirection, RobotDirection> OnRobotChangeDirection { get; }

        public abstract UnityEvent<int, int> OnHealthPointChange { get; }

        public abstract UnityEvent<int, int> OnEnergyPointChange { get; }

        public abstract UnityEvent<int, int> OnArmorPointChange { get; }

        public abstract UnityEvent<int, int> OnAttackPointChange { get; }
    }
}
