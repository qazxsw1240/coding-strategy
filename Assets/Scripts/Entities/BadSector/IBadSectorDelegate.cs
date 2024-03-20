#nullable enable


using CodingStrategy.Entities.Robot;

namespace CodingStrategy.Entities.BadSector
{
    public interface IBadSectorDelegate : IGameEntity
    {
        public abstract IRobotDelegate Installer { get; }

        public abstract Coordinate Position { get; }

        public abstract void Remove();

        public abstract void Execute(IRobotDelegate target);
    }
}
