#nullable enable


namespace CodingStrategy.Entities.Runtime.Statement
{
    using Abnormality;
    using Robot;

    public class AddAbnormalitySpecificRobotStatement : AbstractStatement
    {
        private readonly IAbnormality _abnormality;
        private readonly int _value;

        public AddAbnormalitySpecificRobotStatement(IRobotDelegate robotDelegate, IAbnormality abnormality, int value)
        :base(robotDelegate)
        {
            _abnormality = abnormality;
            _value = value;
        }

        public override void Execute(RuntimeExecutorContext context)
        {
            IAbnormality? abnormality=GameManager.GetAbnormalityValue(_robotDelegate.Id+"-"+_abnormality.Name);
            if(abnormality==null)
            {
                GameManager.SetAbnormalityValue(_robotDelegate.Id+"-"+_abnormality.Name, _abnormality.Copy(_robotDelegate));
                _abnormality.PlayerDelegate=context.PlayerPool[_robotDelegate.Id];
                _abnormality.Value=_value;
                return;
            }
            abnormality.Value+=_value;
        }

        public override StatementPhase Phase => StatementPhase.Static;

        public override IStatement Reverse => new AddAbnormalitySpecificRobotStatement(_robotDelegate, _abnormality, -_value);
    }
}
