#nullable enable


namespace CodingStrategy.Entities.Runtime.Statement
{
    using System;
    using Robot;
    [Obsolete]
    public class MoveRightDiagnolStatement : IStatement
    {
        private readonly IRobotDelegate _robotDelegate;
        private readonly int _direction;
        private bool _isEdge;

        public MoveRightDiagnolStatement(IRobotDelegate robotDelegate, int direction, bool isEdge = false)
        {
            if (direction != 1 && direction != -1)
            {
                throw new ArgumentException();
            }

            _robotDelegate = robotDelegate;
            _direction = direction;
            _isEdge = isEdge;
        }

        public void Execute(RuntimeExecutorContext context)
        {
            int range = Enum.GetValues(typeof(RobotDirection)).Length;
            RobotDirection robotDirection=_robotDelegate.Direction;
            Coordinate[] vectors=_robotDelegate.Vectors;
            Coordinate forwardVector=vectors[(int)robotDirection];
            Coordinate rightVector =vectors[((int)robotDirection + 1) % range];
            Coordinate robotPosition = _robotDelegate.Position;
            Coordinate destination = robotPosition + forwardVector*_direction + rightVector*_direction;
            _isEdge = !_robotDelegate.Move(destination);
            if (_isEdge)
            {
                throw new ExecutionException();
            }
        }

        public IStatement Reverse
        {
            get { return _isEdge ? this : new MoveRightDiagnolStatement(_robotDelegate, -_direction, _isEdge); }
        }

        public StatementPhase Phase => StatementPhase.Move;
    }
}
