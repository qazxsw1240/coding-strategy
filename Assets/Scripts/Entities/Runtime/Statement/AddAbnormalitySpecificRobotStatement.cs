#nullable enable


namespace CodingStrategy.Entities.Runtime.Statement
{
    using Abnormality;
    using Robot;

    public class AddAbnormalitySpecificRobotStatement : IStatement
    {
        private readonly IRobotDelegate _robotDelegate;
        private readonly IAbnormality _abnormality;
        private readonly int _value;

        public AddAbnormalitySpecificRobotStatement(IRobotDelegate robotDelegate, IAbnormality abnormality, int value)
        {
            _robotDelegate = robotDelegate;
            _abnormality = abnormality;
            _value = value;
        }

        public void Execute(RuntimeExecutorContext context)
        {
            IAbnormality? abnormality=GameManager.GetAbnormalityValue(_robotDelegate.Id+"-"+_abnormality.Name);
            if(abnormality==null)
            {
                GameManager.SetAbnormalityValue(_robotDelegate.Id+"-"+_abnormality.Name, _abnormality.Copy(_robotDelegate));
                _abnormality.Value=_value;
                return;
            }
            abnormality.Value+=_value;
        }

        public StatementPhase Phase => StatementPhase.Static;

        public IStatement Reverse => new AddAbnormalitySpecificRobotStatement(_robotDelegate, _abnormality, -_value);
    }
}
