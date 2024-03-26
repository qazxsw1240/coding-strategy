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

        public abstract bool Add(IRobotDelegate robotDelegate, Coordinate position, RobotDirection direction);

        public abstract bool Remove(IRobotDelegate robotDelegate);

        public abstract bool Add(IBadSectorDelegate badSectorDelegate, Coordinate position);

        public abstract bool Remove(IBadSectorDelegate badSectorDelegate);

        public abstract Coordinate GetPosition(IRobotDelegate robotDelegate);

        public abstract Coordinate GetPosition(IBadSectorDelegate badSectorDelegate);

        public abstract RobotDirection GetDirection(IRobotDelegate robotDelegate);

        public abstract bool Place(IRobotDelegate robotDelegate, Coordinate position);

        public abstract bool Rotate(IRobotDelegate robotDelegate, RobotDirection direction);

        public abstract bool Place(IBadSectorDelegate badSectorDelegate, Coordinate position);

        public abstract ICellDelegate[,] AsArray();

        public abstract UnityEvent<IRobotDelegate> OnRobotAdd { get; }

        public abstract UnityEvent<IBadSectorDelegate> OnBadSectorAdd { get; }

        public abstract UnityEvent<IRobotDelegate> OnRobotRemove { get; }

        public abstract UnityEvent<IBadSectorDelegate> OnBadSectorRemove { get; }

        public abstract UnityEvent<IRobotDelegate, Coordinate, Coordinate> OnRobotChangePosition { get; }

        public abstract UnityEvent<IRobotDelegate, RobotDirection, RobotDirection> OnRobotChangeDirection { get; }
    }
}
