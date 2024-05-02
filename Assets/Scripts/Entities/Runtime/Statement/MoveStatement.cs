#nullable enable


namespace CodingStrategy.Entities.Runtime.Statement
{
    using System;
    using Robot;
    using UnityEngine;

    public class MoveStatement : IStatement
    {
        private readonly IRobotDelegate _robotDelegate;
        private readonly int _direction;
        private bool _isEdge;

        public MoveStatement(IRobotDelegate robotDelegate, int direction, bool isEdge = false)
        {
            if (direction != 1 && direction != -1)
            {
                throw new ArgumentException();
            }

            _robotDelegate = robotDelegate;
            _direction = direction;
            _isEdge = isEdge;
        }

        public void Execute()
        {
            Debug.LogFormat("Try to move to {0}", _direction);
            _isEdge = !_robotDelegate.Move(_direction);
            if (_isEdge)
            {
                throw new ExecutionException();
            }
        }

        public IStatement Reverse
        {
            get { return _isEdge ? this : new MoveStatement(_robotDelegate, -1, _isEdge); }
        }
    }
}
