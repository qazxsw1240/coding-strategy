#nullable enable


namespace CodingStrategy.Entities.Board
{
    using System.Collections.Generic;
    using CodingStrategy.Entities.BadSector;
    using CodingStrategy.Entities.Robot;
    using UnityEngine.Events;

    public interface IBoardDelegate
    {
        public abstract IReadOnlyList<IRobotDelegate> Robots { get; }

        public abstract bool AddRobot(IRobotDelegate robotDelegate);

        public abstract bool RemoveRobot(IRobotDelegate robotDelegate);

        public abstract bool Place(IRobotDelegate robot, Coordinate coordinate);

        public abstract bool Place(IBadSector badSector, Coordinate coordinate);

        public abstract IBoardDelegate Clone();

        public abstract ITile[,] AsArray();

        public abstract void UpdateTiles(ITile[,] tiles);

        public abstract UnityEvent<IRobotDelegate> OnRobotAdd { get; }

        public abstract UnityEvent<IRobotDelegate> OnRobotRemove { get; }

        public abstract UnityEvent<IRobotDelegate, Coordinate, Coordinate> OnRobotChangePosition { get; }

        public abstract UnityEvent<IRobotDelegate, RobotDirection, RobotDirection> OnRobotChangeDirection { get; }
    }
}
