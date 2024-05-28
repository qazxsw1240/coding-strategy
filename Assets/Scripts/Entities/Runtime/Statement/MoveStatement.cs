#nullable enable


namespace CodingStrategy.Entities.Runtime.Statement
{
    using System;
    using Robot;
    using UnityEngine;

    public class MoveStatement : AbstractStatement
    {
        private readonly int _direction;
        private bool _isEdge;

        public MoveStatement(IRobotDelegate robotDelegate, int direction, bool isEdge = false)
        :base(robotDelegate)
        {
            // if (direction != 1 && direction != -1)
            // {
            //     throw new ArgumentException();
            // }

            _direction = direction;
            _isEdge = isEdge;
        }

        public override void Execute(RuntimeExecutorContext context)
        {
            Debug.LogFormat("Robot {1} Tries to move to {0}", _direction, _robotDelegate.Id);
            _isEdge = !_robotDelegate.Move(_direction);
            if (_isEdge)
            {
                throw new ExecutionException();
            }
        }

        public override StatementPhase Phase => StatementPhase.Move;

        public override IStatement Reverse => _isEdge ?
            new MoveStatement(_robotDelegate, 0) :
            new MoveStatement(_robotDelegate, -_direction, _isEdge);
    }
}
