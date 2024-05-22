#nullable enable


namespace CodingStrategy.Entities.Runtime.Statement
{
    using System;
    using Robot;
    public class MoveSidewaysStatement : IStatement
    {
        private readonly IRobotDelegate _robotDelegate;
        private readonly int _direction;
        private bool _isEdge;
        public MoveSidewaysStatement(IRobotDelegate robotDelegate, int direction, bool isEdge = false)
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
            Coordinate rightVector = vectors[((int)robotDirection + 1) % range];
            Coordinate robotPosition = _robotDelegate.Position;
            Coordinate destination = robotPosition + rightVector*_direction;
            _isEdge = !_robotDelegate.Move(destination);
            if (_isEdge)
            {
                throw new ExecutionException();
            }
        }

        public IStatement Reverse
        {
            get { return _isEdge ? this : new MoveSidewaysStatement(_robotDelegate, _direction*-1, _isEdge); }
        }

        public StatementPhase Phase => StatementPhase.Move;
    }
}
