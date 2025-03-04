#nullable enable

using CodingStrategy.Entities.Robot;

namespace CodingStrategy.Entities.Runtime.Statement
{
    public class AddHealthPointStatement : AbstractStatement
    {
        private readonly int _healthPointThreshold;
        private readonly bool _isReverse;
        private readonly int _value;

        public AddHealthPointStatement(IRobotDelegate robotDelegate, int value, int healthPointThreshold,
            bool isReverse = false)
            : base(robotDelegate)
        {
            _value = value;
            _healthPointThreshold = healthPointThreshold;
            _isReverse = isReverse;
        }

        public override StatementPhase Phase
        {
            get { return StatementPhase.Static; }
        }

        public override IStatement Reverse
        {
            get { return new AddHealthPointStatement(_robotDelegate, -_value, _healthPointThreshold, true); }
        }

        public override void Execute(RuntimeExecutorContext context)
        {
            if (_isReverse)
            {
                _robotDelegate.HealthPoint += _value;
                return;
            }
            if (_robotDelegate.HealthPoint > _healthPointThreshold)
            {
                return;
            }
            _robotDelegate.HealthPoint += _value;
        }
    }
}
