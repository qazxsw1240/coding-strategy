#nullable enable


namespace CodingStrategy.Entities.Board
{
    using System.Collections.Generic;
    using CodingStrategy.Entities.BadSector;
    using CodingStrategy.Entities.Robot;

    public interface ICellDelegate
    {
        public abstract IBadSectorDelegate? BadSector { get; set; }

        public abstract ISet<IRobotDelegate> Robot { get; set; }
    }
}
