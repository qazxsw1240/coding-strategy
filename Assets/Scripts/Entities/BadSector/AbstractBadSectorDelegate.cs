#nullable enable


namespace CodingStrategy.Entities.BadSector
{
    using System;
    using Board;
    using Robot;

    public　abstract class AbstractBadSectorDelegate : IBadSectorDelegate
    {
        private readonly IBoardDelegate _boardDelegate;

        protected AbstractBadSectorDelegate(string id, IBoardDelegate boardDelegate, IRobotDelegate installer)
        {
            Id = id;
            Installer = installer;
            _boardDelegate = boardDelegate;
        }

        public string Id { get; }

        public IRobotDelegate Installer { get; }

        public Coordinate Position => _boardDelegate.GetPosition(this);

        public void Remove()
        {
            _boardDelegate.Remove(this);
        }

        public abstract void Execute(IRobotDelegate target);

        public int CompareTo(IGameEntity other)
        {
            return string.Compare(Id, other.Id, StringComparison.Ordinal);
        }
    }
}
