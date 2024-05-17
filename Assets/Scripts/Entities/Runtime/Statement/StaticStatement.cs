#nullable enable


using System;
using CodingStrategy.Entities.BadSector;
using CodingStrategy.Entities.Board;
using UnityEngine;

namespace CodingStrategy.Entities.Runtime.Statement
{
    using Robot;

    public class StaticStatement : IStatement
    {
        private readonly IBoardDelegate _boardDelegate;
        private readonly IRobotDelegate _robotDelegate;
        private readonly Func<IBoardDelegate, IRobotDelegate, IBadSectorDelegate> _generator;

        private IBadSectorDelegate? _badSectorDelegate;

        public StaticStatement(
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
            bool result = _boardDelegate.Add(_badSectorDelegate, _robotDelegate.Position);
            if (!result)
            {
                throw new ExecutionException($"Cannot add bad sector {_badSectorDelegate.Id}");
            }
        }

        public IStatement Reverse => new StaticRollbackStatement(this);

        private class StaticRollbackStatement : IStatement
        {
            private readonly StaticStatement _statement;

            public StaticRollbackStatement(StaticStatement statement)
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

            public IStatement Reverse => _statement;
        }
    }
}
