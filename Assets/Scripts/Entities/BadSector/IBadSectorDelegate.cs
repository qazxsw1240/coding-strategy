#nullable enable


using CodingStrategy.Entities.Robot;

namespace CodingStrategy.Entities.BadSector
{
    public interface IBadSectorDelegate : IGameEntity, IPlaceable
    {
        public abstract IRobotDelegate Installer { get; }

        public new abstract Coordinate Position { get; }

        public abstract void Remove();

        public abstract void Execute(IRobotDelegate target);
    }
}
