#nullable enable


namespace CodingStrategy.Entities.Runtime.Statement
{
    using CodingStrategy.Entities.Player;
    using CodingStrategy.Entities.Runtime.Abnormality;
    using Robot;

    public class AddCurrencyByStackStatement : IStatement
    {
        private readonly IRobotDelegate _robotDelegate;
        private readonly int _coefficient;

        public AddCurrencyByStackStatement(IRobotDelegate robotDelegate, int coefficient)
        {
            _robotDelegate = robotDelegate;
            _coefficient = coefficient;
        }

        public void Execute(RuntimeExecutorContext context)
        {
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

        public StatementPhase Phase => StatementPhase.Static;

        public IStatement Reverse => new AddCurrencyByStackStatement(_robotDelegate, -_coefficient);
    }
}
