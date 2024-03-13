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

        public abstract Coordinate GetPosition(IRobotDelegate robotDelegate);

        public abstract RobotDirection GetDirection(IRobotDelegate robotDelegate);

        public abstract bool Place(IRobotDelegate robotDelegate, Coordinate position);

        public abstract bool Rotate(IRobotDelegate robotDelegate, RobotDirection direction);

        public abstract ITileDelegate[,] AsArray();

        public abstract UnityEvent<IRobot> OnRobotAdd { get; }

        public abstract UnityEvent<IRobot> OnRobotRemove { get; }

        public abstract UnityEvent<IRobot, Coordinate, Coordinate> OnRobotChangePosition { get; }

        public abstract UnityEvent<IRobot, RobotDirection, RobotDirection> OnRobotChangeDirection { get; }
    }
}
