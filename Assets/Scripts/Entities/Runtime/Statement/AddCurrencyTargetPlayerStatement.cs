#nullable enable

using CodingStrategy.Entities.Player;
using CodingStrategy.Entities.Robot;
using CodingStrategy.Entities.Runtime.Abnormality;

namespace CodingStrategy.Entities.Runtime.Statement
{
    public class AddCurrencyTargetPlayerStatement : AbstractStatement
    {
        private readonly string _abnormalityName;
        private readonly int _coefficient;
        private readonly IPlayerDelegate[] _target;

        public AddCurrencyTargetPlayerStatement(IRobotDelegate robotDelegate, IPlayerDelegate[] target, int coefficient,
            string abnormalityName)
            : base(robotDelegate)
        {
            _target = target;
            _coefficient = coefficient;
            _abnormalityName = abnormalityName;
        }

        public override StatementPhase Phase
        {
            get { return StatementPhase.Static; }
        }

        public override IStatement Reverse
        {
            get
            {
                return new AddCurrencyTargetPlayerStatement(_robotDelegate, _target, -_coefficient, _abnormalityName);
            }
        }

        public override void Execute(RuntimeExecutorContext context)
        {
            foreach (IPlayerDelegate playerDelegate in _target)
            {
                IAbnormality? abnormality = GameManager.GetAbnormalityValue(playerDelegate.Id + "-" + _abnormalityName);
                if (abnormality == null)
                {
                    return;
                }
                int addNum = abnormality.Value * _coefficient;
                playerDelegate.Currency += addNum;
            }
        }
    }
}
