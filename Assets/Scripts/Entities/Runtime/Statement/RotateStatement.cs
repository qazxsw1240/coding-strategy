#nullable enable

using System;

using CodingStrategy.Entities.Robot;

namespace CodingStrategy.Entities.Runtime.Statement
{
    public class RotateStatement : AbstractStatement
    {
        private readonly int _direction;

        public RotateStatement(IRobotDelegate robotDelegate, int direction) : base(robotDelegate)
        {
            if (direction != 1 && direction != -1)
            {
                throw new ArgumentException();
            }

            _direction = direction;
        }

        public override StatementPhase Phase => StatementPhase.Move;

        public override IStatement Reverse => new RotateStatement(_robotDelegate, -_direction);

        public override void Execute(RuntimeExecutorContext context)
        {
            _robotDelegate.Rotate(_direction);
        }
    }
}
