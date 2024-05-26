#nullable enable


namespace CodingStrategy.Entities.Runtime.Statement
{
    using Robot;
    using UnityEngine;

    public class MoveCoordinateStatement : IStatement
    {
        private readonly IRobotDelegate _robotDelegate;
        private readonly Coordinate _coordinate;
        private bool _isEdge;

        public MoveCoordinateStatement(IRobotDelegate robotDelegate, Coordinate coordinate, bool isEdge = false)
        {
            _robotDelegate = robotDelegate;
            _coordinate = coordinate;
            _isEdge = isEdge;
        }

        public void Execute(RuntimeExecutorContext context)
        {
            Debug.LogFormat("Robot {1} Tries to move to {0}", _coordinate.X+","+_coordinate.Y, _robotDelegate.Id);

            Coordinate destination=_robotDelegate.Position+RotateMatrix.Rotate((int)_robotDelegate.Direction)*_coordinate;
            _isEdge = !_robotDelegate.Move(destination);
            if (_isEdge)
            {
                throw new ExecutionException();
            }
        }

        public StatementPhase Phase => StatementPhase.Move;

        public IStatement Reverse => _isEdge ?
            new MoveCoordinateStatement(_robotDelegate, _coordinate) :
            new MoveCoordinateStatement(_robotDelegate, -1*_coordinate, _isEdge);
    }
}
