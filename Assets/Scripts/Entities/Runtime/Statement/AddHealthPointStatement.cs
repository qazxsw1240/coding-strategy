#nullable enable


namespace CodingStrategy.Entities.Runtime.Statement
{
    using Robot;

    public class AddHealthPointStatement : AbstractStatement
    {
        private readonly int _value;
        private readonly bool _isReverse;
        public AddHealthPointStatement(IRobotDelegate robotDelegate, int value, bool isReverse=false)
        :base(robotDelegate)
        {
            _value = value;
            _isReverse = isReverse;
        }

        public override void Execute(RuntimeExecutorContext context)
        {
            if(_isReverse)
            {
                _robotDelegate.HealthPoint += _value;
                return;
            }
            if(_robotDelegate.HealthPoint>2)
            {
                return;
            }
            _robotDelegate.HealthPoint += _value;
        }

        public override StatementPhase Phase => StatementPhase.Static;

        public override IStatement Reverse => new AddHealthPointStatement(_robotDelegate, -_value, true);
    }
}
