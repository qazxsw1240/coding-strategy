#nullable enable

using System;
using System.Collections.Generic;

using CodingStrategy.Entities.BadSector;
using CodingStrategy.Entities.Board;
using CodingStrategy.Entities.Robot;

using UnityEngine;

namespace CodingStrategy.Entities.Runtime.Statement
{
    public class PointerStatement : AbstractStatement
    {
        private readonly IList<IBadSectorDelegate> _badSectorDelegates = new List<IBadSectorDelegate>();
        private readonly IList<Coordinate> _coordinates;
        private readonly Func<IBoardDelegate, IRobotDelegate, IBadSectorDelegate> _generator;
        private IBoardDelegate? _boardDelegate;

        public PointerStatement(
            IRobotDelegate robotDelegate,
            Func<IBoardDelegate, IRobotDelegate, IBadSectorDelegate> generator,
            IList<Coordinate> coordinates) : base(robotDelegate)
        {
            _generator = generator;
            _coordinates = coordinates;
        }

        public override StatementPhase Phase => StatementPhase.Pointer;

        public override IStatement Reverse => new PointerRollbackStatement(this);

        public override void Execute(RuntimeExecutorContext context)
        {
            _boardDelegate = context.BoardDelegate;
            foreach (Coordinate coordinate in _coordinates)
            {
                IBadSectorDelegate badSectorDelegate = _generator(_boardDelegate, _robotDelegate);
                Debug.LogFormat("Robot {0} Tries to put bad sector {1}", _robotDelegate.ID, badSectorDelegate.ID);
                Coordinate installCoordinate = _robotDelegate.Position
                                             + RotateMatrix.Rotate((int) _robotDelegate.Direction) * coordinate;
                if (_boardDelegate.Add(badSectorDelegate, installCoordinate))
                {
                    _badSectorDelegates.Add(badSectorDelegate);
                }
            }
        }

        private class PointerRollbackStatement : IStatement
        {
            private readonly PointerStatement _statement;

            public PointerRollbackStatement(PointerStatement statement)
            {
                _statement = statement;
            }

            public void Execute(RuntimeExecutorContext context)
            {
                foreach (IBadSectorDelegate badSectorDelegate in _statement._badSectorDelegates)
                {
                    _statement._boardDelegate?.Remove(badSectorDelegate);
                }
            }

            public StatementPhase Phase => StatementPhase.Pointer;

            public IStatement Reverse => _statement;
        }
    }
}
