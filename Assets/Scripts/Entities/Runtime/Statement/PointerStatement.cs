#nullable enable


using System;
using CodingStrategy.Entities.BadSector;
using CodingStrategy.Entities.Board;
using UnityEngine;

namespace CodingStrategy.Entities.Runtime.Statement
{
    using Robot;

    public class PointerStatement : IStatement
    {
        private readonly IBoardDelegate _boardDelegate;
        private readonly IRobotDelegate _robotDelegate;
        private readonly Func<IBoardDelegate, IRobotDelegate, IBadSectorDelegate> _generator;

        private IBadSectorDelegate? _badSectorDelegate;

        public PointerStatement(
            IBoardDelegate boardDelegate,
            IRobotDelegate robotDelegate,
            Func<IBoardDelegate, IRobotDelegate, IBadSectorDelegate> generator)
        {
            _boardDelegate = boardDelegate;
            _robotDelegate = robotDelegate;
            _generator = generator;
        }

        public void Execute()
        {
            _badSectorDelegate = _generator(_boardDelegate, _robotDelegate);
            Debug.LogFormat("Robot {0} Tries to put bad sector {1}", _robotDelegate.Id, _badSectorDelegate.Id);
            _boardDelegate.Add(_badSectorDelegate, _robotDelegate.Position);
        }

        public StatementPhase Phase => StatementPhase.Pointer;

        public IStatement Reverse => new PointerRollbackStatement(this);

        private class PointerRollbackStatement : IStatement
        {
            private readonly PointerStatement _statement;

            public PointerRollbackStatement(PointerStatement statement)
            {
                _statement = statement;
            }

            public void Execute()
            {
                IBadSectorDelegate? badSectorDelegate = _statement._badSectorDelegate;
                if (badSectorDelegate == null)
                {
                    return;
                }

                _statement._boardDelegate.Remove(badSectorDelegate);
            }

            public StatementPhase Phase => StatementPhase.Pointer;

            public IStatement Reverse => _statement;
        }
    }
}
