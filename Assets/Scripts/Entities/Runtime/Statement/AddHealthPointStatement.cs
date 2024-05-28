#nullable enable


namespace CodingStrategy.Entities.Runtime.Statement
{
    using Robot;

    public class AddHealthPointStatement : AbstractStatement
    {
        private readonly int _value;
        private readonly int _healthPointThreshold;
        private readonly bool _isReverse;
        public AddHealthPointStatement(IRobotDelegate robotDelegate, int value, int healthPointThreshold, bool isReverse=false)
        :base(robotDelegate)
        {
            _value = value;
            _healthPointThreshold = healthPointThreshold;
            _isReverse = isReverse;
        }

        public override void Execute(RuntimeExecutorContext context)
        {
            if(_isReverse)
            {
                _robotDelegate.HealthPoint += _value;
                return;
            }
            if(_robotDelegate.HealthPoint>_healthPointThreshold)
            {
                return;
            }
            _robotDelegate.HealthPoint += _value;
        }

        public override StatementPhase Phase => StatementPhase.Static;

        public override IStatement Reverse => new AddHealthPointStatement(_robotDelegate, -_value, _healthPointThreshold, true);
    }
}
