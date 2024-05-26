#nullable enable


using System;
using UnityEngine;

namespace CodingStrategy.Entities.Runtime.Statement
{
    using System.Collections.Generic;
    using BadSector;
    using Board;
    using Robot;

    public class PointerStatement : IStatement
    {
        private IBoardDelegate? _boardDelegate;
        private readonly IRobotDelegate _robotDelegate;
        private readonly Func<IBoardDelegate, IRobotDelegate, IBadSectorDelegate> _generator;

        private readonly IList<IBadSectorDelegate> _badSectorDelegates=new List<IBadSectorDelegate>();
        private readonly IList<Coordinate> _coordinates;

        public PointerStatement(
            IRobotDelegate robotDelegate,
            Func<IBoardDelegate, IRobotDelegate, IBadSectorDelegate> generator,
            IList<Coordinate> coordinates)
        {
            _robotDelegate=robotDelegate;
            _generator = generator;
            _coordinates = coordinates;
        }

        public void Execute(RuntimeExecutorContext context)
        {
            _boardDelegate=context.BoardDelegate;
            foreach(Coordinate coordinate in _coordinates)
            {
                IBadSectorDelegate badSectorDelegate = _generator(_boardDelegate, _robotDelegate);
                Debug.LogFormat("Robot {0} Tries to put bad sector {1}", _robotDelegate.Id, badSectorDelegate.Id);
                Coordinate installCoordinate=_robotDelegate.Position+RotateMatrix.Rotate((int)_robotDelegate.Direction)*coordinate;    
                if(_boardDelegate.Add(badSectorDelegate, installCoordinate))
                {
                    _badSectorDelegates.Add(badSectorDelegate);
                }
            }
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

            public void Execute(RuntimeExecutorContext context)
            {
                foreach(IBadSectorDelegate badSectorDelegate in _statement._badSectorDelegates)
                {
                    _statement._boardDelegate?.Remove(badSectorDelegate);
                }
            }

            public StatementPhase Phase => StatementPhase.Pointer;

            public IStatement Reverse => _statement;
        }
    }
}
