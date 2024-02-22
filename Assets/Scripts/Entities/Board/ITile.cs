#nullable enable


namespace CodingStrategy.Entities.Board
{
    using System.Collections.Generic;
    using CodingStrategy.Entities.BadSector;
    using CodingStrategy.Entities.Robot;

    public interface ITile
    {
        public abstract IList<IBadSector> Obstacle { get; set; }

        public abstract IList<IRobot> Robot { get; set; }
    }
}
