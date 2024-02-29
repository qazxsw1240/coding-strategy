#nullable enable


namespace CodingStrategy.Entities.Board
{
    using System.Collections.Generic;
    using CodingStrategy.Entities.Robot;

    public interface IBoard
    {
        public abstract IReadOnlyList<IRobot> Robots { get; }
    }
}
