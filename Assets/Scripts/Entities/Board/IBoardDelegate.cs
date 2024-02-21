#nullable enable


namespace CodingStrategy.Entities.Board
{
    using System.Collections.Generic;
    using CodingStrategy.Entities.Obstacle;
    using CodingStrategy.Entities.Robot;

    public interface IBoardDelegate
    {
        public abstract IList<IRobot> Robots { get; }

        public abstract bool Place(IRobotDelegate robot, Coordinate coordinate);

        public abstract bool Place(IObstacle obstacle, Coordinate coordinate);

        public abstract IBoardDelegate Clone();

        public abstract ITile[,] AsArray();

        public abstract void UpdateTiles(ITile[,] tiles);
    }
}
