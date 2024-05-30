#nullable enable


using System.Collections.Generic;
using CodingStrategy.Entities.Robot;
using CodingStrategy.Entities.Runtime;

namespace CodingStrategy.Entities.BadSector
{
    public interface IBadSectorDelegate : IGameEntity, IPlaceable
    {
        public abstract string Explanation { get; }
        
        public abstract IRobotDelegate Installer { get; }

        public new abstract Coordinate Position { get; }

        public abstract void Remove();

        public abstract IList<IStatement> Execute(IRobotDelegate target);
    }
}
