#nullable enable


namespace CodingStrategy.Entities.Runtime.Statement
{
    using Abnormality;
    using Robot;

    public class AddAbnormalityAllEnemyStatement : AbstractStatement
    {
        private readonly IAbnormality _abnormality;
        private readonly int _value;

        public AddAbnormalityAllEnemyStatement(IRobotDelegate robotDelegate, int energy, IAbnormality abnormality, int value)
        : base(robotDelegate, energy)
        {
            _abnormality = abnormality;
            _value = value;
        }

        public override void Execute(RuntimeExecutorContext context)
        {
            base.Execute(context);
            foreach(IRobotDelegate robotDelegate in context.RobotDelegatePool)
            {
                if(robotDelegate == _robotDelegate) continue;
                IAbnormality? abnormality=GameManager.GetAbnormalityValue(robotDelegate.Id+"-"+_abnormality.Name);
                if(abnormality==null)
                {
                    GameManager.SetAbnormalityValue(robotDelegate.Id+"-"+_abnormality.Name, _abnormality);
                    _abnormality.Value=_value;
                    continue;
                }
                abnormality.Value+=_value;
            }
        }

        public override StatementPhase Phase => StatementPhase.Static;

        public override IStatement Reverse => new AddAbnormalityAllEnemyStatement(_robotDelegate, _energy, _abnormality, -_value);
    }
}
