#nullable enable

using System;
using System.Collections.Generic;

using CodingStrategy.Entities.Board;
using CodingStrategy.Entities.Robot;
using CodingStrategy.Entities.Runtime;

namespace CodingStrategy.Entities.BadSector
{
    public abstract class AbstractBadSectorDelegate : IBadSectorDelegate
    {
        private readonly IBoardDelegate _boardDelegate;

        protected AbstractBadSectorDelegate(string id, IBoardDelegate boardDelegate, IRobotDelegate installer)
        {
            ID = id;
            Installer = installer;
            _boardDelegate = boardDelegate;
        }

        public string ID { get; }

        public IRobotDelegate Installer { get; }

        public Coordinate Position => _boardDelegate.GetPosition(this);

        public abstract string Explanation { get; }

        public void Remove()
        {
            _boardDelegate.Remove(this);
        }

        public abstract IList<IStatement> Execute(IRobotDelegate target);

        public int CompareTo(IGameEntity other)
        {
            return string.Compare(ID, other.ID, StringComparison.Ordinal);
        }
    }
}
