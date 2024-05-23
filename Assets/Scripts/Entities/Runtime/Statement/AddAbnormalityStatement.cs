#nullable enable


namespace CodingStrategy.Entities.Runtime.Statement
{
    using Abnormality;
    using Robot;

    public class AddAbnormalityAllRobotStatement : IStatement
    {
        private readonly IAbnormality _abnormality;
        private readonly int _value;

        public AddAbnormalityAllRobotStatement(IAbnormality abnormality, int value)
        {
            _abnormality = abnormality;
            _value = value;
        }

        public void Execute(RuntimeExecutorContext context)
        {
            foreach(IRobotDelegate robotDelegate in context.RobotDelegatePool)
            {
                IAbnormality? abnormality=GameManager.GetAbnormalityValue(robotDelegate.Id+"-"+_abnormality.Name);
                if(abnormality==null)
                {
                    GameManager.SetAbnormalityValue(robotDelegate.Id+"-"+_abnormality.Name, _abnormality.Copy(robotDelegate));
                    _abnormality.Value=_value;
                    continue;
                }
                abnormality.Value+=_value;
            }
        }

        public StatementPhase Phase => StatementPhase.Static;

        public IStatement Reverse => new AddAbnormalityAllRobotStatement(_abnormality, -_value);
    }
}
