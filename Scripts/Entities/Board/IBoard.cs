#nullable enable


namespace CodingStrategy.Entities.Board
{
    using System.Collections.Generic;
    using CodingStrategy.Entities.Robot;

    public interface IBoard
    {
        public abstract ITile this[Coordinate coordinate] { get; }

        public abstract ITile[,] Tiles { get; }

        public abstract IReadOnlyList<IRobot> Robots { get; }
    }
}
