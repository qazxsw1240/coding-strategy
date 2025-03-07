#nullable enable

using CodingStrategy.Entities.Robot;

using UnityEngine;

namespace CodingStrategy.Entities.Runtime.Statement
{
    public class MoveCoordinateStatement : AbstractStatement
    {
        private readonly Coordinate _coordinate;
        private bool _isEdge;

        public MoveCoordinateStatement(IRobotDelegate robotDelegate, Coordinate coordinate, bool isEdge = false)
            : base(robotDelegate)
        {
            _coordinate = coordinate;
            _isEdge = isEdge;
        }

        public override StatementPhase Phase => StatementPhase.Move;

        public override IStatement Reverse => _isEdge
            ? new MoveCoordinateStatement(_robotDelegate, Coordinate.Unit, _isEdge)
            : new MoveCoordinateStatement(_robotDelegate, -1 * _coordinate, _isEdge);

        public override void Execute(RuntimeExecutorContext context)
        {
            Debug.LogFormat("Robot {1} Tries to move to {0}", _coordinate.X + "," + _coordinate.Y, _robotDelegate.ID);

            Coordinate destination = _robotDelegate.Position
                                   + RotateMatrix.Rotate((int) _robotDelegate.Direction) * _coordinate;
            _isEdge = !_robotDelegate.Move(destination);
            if (_isEdge)
            {
                Debug.LogFormat("Cannot move because the robot is on edge.");
                throw new ExecutionException();
            }
        }
    }
}
