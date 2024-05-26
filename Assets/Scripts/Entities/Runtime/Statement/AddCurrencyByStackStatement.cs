#nullable enable


namespace CodingStrategy.Entities.Runtime.Statement
{
    using CodingStrategy.Entities.Player;
    using CodingStrategy.Entities.Runtime.Abnormality;
    using Robot;

    public class AddCurrencyByStackStatement : AbstractStatement
    {
        private readonly int _coefficient;

        public AddCurrencyByStackStatement(IRobotDelegate robotDelegate, int energy, int coefficient)
        :base(robotDelegate, energy)
        {
            _coefficient = coefficient;
        }

        public override void Execute(RuntimeExecutorContext context)
        {
            base.Execute(context);
            IAbnormality? abnormality=GameManager.GetAbnormalityValue(_robotDelegate.Id+"-"+Stack.Name);
            if(abnormality==null)
            {
                return;
            }
            IPlayerDelegate? player=null;
            foreach(IPlayerDelegate playerDelegate in context.PlayerPool)
            {
                if(playerDelegate.Robot.Id == _robotDelegate.Id)
                {
                    player=playerDelegate;
                    break;
                }
            }
            int addNum=abnormality.Value*_coefficient;
            if(player!=null)
                player.Currency+=addNum;
        }

        public override StatementPhase Phase => StatementPhase.Static;

        public override IStatement Reverse => new AddCurrencyByStackStatement(_robotDelegate, _energy, -_coefficient);
    }
}
