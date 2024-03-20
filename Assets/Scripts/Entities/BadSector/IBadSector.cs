#nullable enable


namespace CodingStrategy.Entities.BadSector
{
    using CodingStrategy.Entities.Robot;

    public interface IBadSector
    {
        public abstract string Id { get; }

        public abstract IRobot Installer { get; }

        public abstract Coordinate Position { get; }

        public abstract void Remove();

        public abstract void Execute(IRobot target);
    }
}
