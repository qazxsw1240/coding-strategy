#nullable enable


namespace CodingStrategy.Entities.Robot
{
    using CodingStrategy.Runtime;
    using UnityEngine.Events;

    public interface IRobotDelegate : IGameEntity
    {
        public abstract IAlgorithm Algorithm { get; }

        public abstract Coordinate Position { get; set; }

        public abstract RobotDirection Direction { get; set; }

        public abstract int HealthPoint { get; set; }

        public abstract int EnergyPoint { get; set; }

        public abstract int ArmorPoint { get; set; }

        public abstract int AttackPoint { get; set; }

        public abstract bool Move(int count);

        public abstract bool Move(Coordinate position);

        public abstract bool Rotate(int count);

        public abstract bool Rotate(RobotDirection direction);

        public abstract bool Attack(IRobotAttackStrategy strategy, params Coordinate[] relativePosition);

        public abstract UnityEvent<IRobotDelegate, Coordinate, Coordinate> OnRobotChangePosition { get; }

        public abstract UnityEvent<IRobotDelegate, RobotDirection, RobotDirection> OnRobotChangeDirection { get; }

        public abstract UnityEvent<IRobotDelegate, int, int> OnHealthPointChange { get; }

        public abstract UnityEvent<IRobotDelegate, int, int> OnEnergyPointChange { get; }

        public abstract UnityEvent<IRobotDelegate, int, int> OnArmorPointChange { get; }

        public abstract UnityEvent<IRobotDelegate, int, int> OnAttackPointChange { get; }
    }
}
