#nullable enable


using System.Collections.Generic;
using CodingStrategy.Entities.Runtime;

namespace CodingStrategy.Entities.BadSector
{
    using System;
    using Board;
    using Runtime.CommandImpl;
    using Robot;

    public　abstract class AbstractBadSectorDelegate : IBadSectorDelegate
    {
        protected readonly CommandBuilder _commandBuilder;
        private readonly IBoardDelegate _boardDelegate;

        protected AbstractBadSectorDelegate(string id, IBoardDelegate boardDelegate, IRobotDelegate installer)
        {
            Id = id;
            Installer = installer;
            _boardDelegate = boardDelegate;
            _commandBuilder = new();
        }

        public string Id { get; }

        public IRobotDelegate Installer { get; }

        public Coordinate Position => _boardDelegate.GetPosition(this);

        public void Remove()
        {
            _boardDelegate.Remove(this);
        }

        public abstract IList<IStatement> Execute(IRobotDelegate target);

        public int CompareTo(IGameEntity other)
        {
            return string.Compare(Id, other.Id, StringComparison.Ordinal);
        }
    }
}
