#nullable enable


namespace CodingStrategy.Entities.Board
{
    using System;
    using System.Collections.Generic;
    using CodingStrategy.Entities.BadSector;
    using CodingStrategy.Entities.Robot;

    [Obsolete("Deprecated", true)]
    public interface ITile
    {
        public abstract IBadSector? BadSector { get; set; }

        public abstract ISet<IRobot> Robot { get; set; }
    }
}
