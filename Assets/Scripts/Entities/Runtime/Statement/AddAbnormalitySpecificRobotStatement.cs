#nullable enable

using CodingStrategy.Entities.Robot;
using CodingStrategy.Entities.Runtime.Abnormality;

namespace CodingStrategy.Entities.Runtime.Statement
{
    public class AddAbnormalitySpecificRobotStatement : AbstractStatement
    {
        private readonly IAbnormality _abnormality;
        private readonly int _value;

        public AddAbnormalitySpecificRobotStatement(IRobotDelegate robotDelegate, IAbnormality abnormality, int value)
            : base(robotDelegate)
        {
            _abnormality = abnormality;
            _value = value;
        }

        public override StatementPhase Phase
        {
            get { return StatementPhase.Static; }
        }

        public override IStatement Reverse
        {
            get { return new AddAbnormalitySpecificRobotStatement(_robotDelegate, _abnormality, -_value); }
        }

        public override void Execute(RuntimeExecutorContext context)
        {
            IAbnormality? abnormality = GameManager.GetAbnormalityValue(_robotDelegate.Id + "-" + _abnormality.Name);
            if (abnormality == null)
            {
                GameManager.SetAbnormalityValue(
                    _robotDelegate.Id + "-" + _abnormality.Name,
                    _abnormality.Copy(_robotDelegate));
                _abnormality.PlayerDelegate = context.PlayerPool[_robotDelegate.Id];
                _abnormality.Value = _value;
                return;
            }
            abnormality.Value += _value;
        }
    }
}
