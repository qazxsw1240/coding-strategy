#nullable enable

using CodingStrategy.Entities.Robot;
using CodingStrategy.Entities.Runtime.Abnormality;

namespace CodingStrategy.Entities.Runtime.Statement
{
    public class AddAbnormalityAllRobotStatement : AbstractStatement
    {
        private readonly IAbnormality _abnormality;
        private readonly int _value;

        public AddAbnormalityAllRobotStatement(IRobotDelegate robotDelegate, IAbnormality abnormality, int value)
            : base(robotDelegate)
        {
            _abnormality = abnormality;
            _value = value;
        }

        public override StatementPhase Phase => StatementPhase.Static;

        public override IStatement Reverse =>
            new AddAbnormalityAllRobotStatement(_robotDelegate, _abnormality, -_value);

        public override void Execute(RuntimeExecutorContext context)
        {
            foreach (IRobotDelegate robotDelegate in context.RobotDelegatePool)
            {
                IAbnormality? abnormality = GameManager.GetAbnormalityValue(robotDelegate.Id + "-" + _abnormality.Name);
                if (abnormality == null)
                {
                    GameManager.SetAbnormalityValue(
                        $"{robotDelegate.Id}-{_abnormality.Name}",
                        _abnormality.Copy(robotDelegate));
                    _abnormality.PlayerDelegate = context.PlayerPool[_robotDelegate.Id];
                    _abnormality.Value = _value;
                    continue;
                }
                abnormality.Value += _value;
            }
        }
    }
}
