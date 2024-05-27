#nullable enable


namespace CodingStrategy.Entities.Runtime.Statement
{
    using CodingStrategy.Entities.Player;
    using CodingStrategy.Entities.Runtime.Abnormality;
    using Robot;

    public class AddCurrencyTargetPlayerStatement : AbstractStatement
    {
        private readonly IPlayerDelegate[] _target;
        private readonly int _coefficient;
        private readonly string _abnormalityName;

        public AddCurrencyTargetPlayerStatement(IRobotDelegate robotDelegate, IPlayerDelegate[] target, int coefficient, string abnormalityName)
        :base(robotDelegate)
        {
            _target = target;
            _coefficient = coefficient;
            _abnormalityName = abnormalityName;
        }

        public override void Execute(RuntimeExecutorContext context)
        {
            base.Execute(context);
            foreach(IPlayerDelegate playerDelegate in _target)
            {
                IAbnormality? abnormality=GameManager.GetAbnormalityValue(playerDelegate.Id+"-"+_abnormalityName);
                if(abnormality==null)
                {
                    return;
                }
                int addNum=abnormality.Value*_coefficient;
                playerDelegate.Currency+=addNum;
            }
        }

        public override StatementPhase Phase => StatementPhase.Static;

        public override IStatement Reverse => new AddCurrencyTargetPlayerStatement(_robotDelegate, _target, -_coefficient, _abnormalityName);
    }
}
