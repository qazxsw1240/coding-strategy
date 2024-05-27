#nullable enable


namespace CodingStrategy.Entities.Runtime.Statement
{
    using System;
    using Robot;

    [Obsolete]
    public class MoveLeftDiagnolStatement : IStatement
    {
        private readonly IRobotDelegate _robotDelegate;
        private readonly int _direction;
        private bool _isEdge;

        public MoveLeftDiagnolStatement(IRobotDelegate robotDelegate, int direction, bool isEdge = false)
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
            RobotDirection robotDirection = _robotDelegate.Direction;
            Coordinate[] vectors = _robotDelegate.Vectors;
            Coordinate forwardVector = vectors[(int) robotDirection];
            Coordinate leftVector = vectors[((int) robotDirection - 1 + range) % range];
            Coordinate robotPosition = _robotDelegate.Position;
            Coordinate destination = robotPosition + forwardVector * _direction + leftVector * _direction;
            _isEdge = !_robotDelegate.Move(destination);
            if (_isEdge)
            {
                throw new ExecutionException();
            }
        }

        public IStatement Reverse => _isEdge
            ? new MoveStatement(_robotDelegate, 0, _isEdge)
            : new MoveLeftDiagnolStatement(_robotDelegate, -_direction, _isEdge);

        public StatementPhase Phase => StatementPhase.Move;
    }
}
