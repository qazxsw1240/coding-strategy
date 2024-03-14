#nullable enable


namespace CodingStrategy.Entities.Board
{
    using System.Collections.Generic;
    using CodingStrategy.Entities.Robot;
    using UnityEngine.Events;

    public interface IBoard
    {
        public abstract IReadOnlyList<IRobot> Robots { get; }

        public abstract bool Add(IRobot robotDelegate, Coordinate position, RobotDirection direction);

        public abstract bool Remove(IRobot robotDelegate);

        public abstract Coordinate GetPosition(IRobot robotDelegate);

        public abstract RobotDirection GetDirection(IRobot robotDelegate);

        public abstract bool Place(IRobot robotDelegate, Coordinate position);

        public abstract bool Rotate(IRobot robotDelegate, RobotDirection direction);

        public abstract ITile[,] AsArray();

        public abstract UnityEvent<IRobot> OnRobotAdd { get; }

        public abstract UnityEvent<IRobot> OnRobotRemove { get; }

        public abstract UnityEvent<IRobot, Coordinate, Coordinate> OnRobotChangePosition { get; }

        public abstract UnityEvent<IRobot, RobotDirection, RobotDirection> OnRobotChangeDirection { get; }
    }
}
