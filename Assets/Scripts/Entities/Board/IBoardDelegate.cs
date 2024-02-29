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

        public abstract bool Add(IRobotDelegate robotDelegate);

        public abstract bool Remove(IRobotDelegate robotDelegate);

        public abstract bool Add(IBadSectorDelegate badSectorDelegate);

        public abstract bool Remove(IBadSectorDelegate badSectorDelegate);

        public abstract Coordinate GetPosition(IRobotDelegate robotDelegate);

        public abstract Coordinate GetPosition(IBadSectorDelegate badSectorDelegate);

        public abstract bool Place(IRobotDelegate robot, Coordinate coordinate);

        public abstract bool Rotate(IRobotDelegate robot, RobotDirection robotDirection);

        public abstract bool Place(IBadSector badSector, Coordinate coordinate);

        public abstract ITile[,] AsArray();

        public abstract UnityEvent<IRobotDelegate> OnRobotAdd { get; }

        public abstract UnityEvent<IBadSectorDelegate> OnBadSectorAdd { get; }

        public abstract UnityEvent<IRobotDelegate> OnRobotRemove { get; }

        public abstract UnityEvent<IBadSectorDelegate> OnBadSectorRemove { get; }

        public abstract UnityEvent<IRobotDelegate, Coordinate, Coordinate> OnRobotChangePosition { get; }

        public abstract UnityEvent<IRobotDelegate, RobotDirection, RobotDirection> OnRobotChangeDirection { get; }
    }
}
