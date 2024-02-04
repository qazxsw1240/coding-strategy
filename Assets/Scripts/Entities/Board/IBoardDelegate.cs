#nullable enable


namespace CodingStrategy.Entities.Board
{
    using System.Collections.Generic;
    using CodingStrategy.Entities.Obstacle;
    using CodingStrategy.Entities.Robot;

    public interface IBoardDelegate
    {
        public abstract ITile this[Coordinate coordinate] { get; set; }

        public abstract ITile[,] Tiles { get; set; }

        public abstract IList<IRobot> Robots { get; }

        public abstract bool Move(IRobotDelegate robot, Coordinate coordinate);

        public abstract bool Place(IRobotDelegate robot, Coordinate coordinate);

        public abstract bool Place(IObstacle obstacle, Coordinate coordinate);
    }
}
