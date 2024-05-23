#nullable enable


namespace CodingStrategy.Entities.Runtime.CommandImpl
{
    using System.Collections.Generic;
    using Robot;
    using Statement;

    public class CoinMiningCommand : AbstractCommand
    {
        private readonly CommandBuilder _commandBuilder=new();

        public CoinMiningCommand(string id="11", string name="코인 채굴", int enhancedLevel=1, int grade=1)
        : base(id, name, enhancedLevel, grade)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            if(!keepStatus)
            {
                return new CoinMiningCommand();
            }
            return new CoinMiningCommand(Id, Info.Name, Info.EnhancedLevel, Info.Grade);
        }

        public override IList<IStatement> GetCommandStatements(IRobotDelegate robot)
        {
            _commandBuilder.Clear();
            _commandBuilder.Append(new AddCurrencyByStackStatement(robot, 1));
            if(Info.EnhancedLevel>=2)
                _commandBuilder.Append(new AddCurrencyByStackStatement(robot, 1));
            return _commandBuilder.Build();
        }

        public override bool Invoke(params object[] args)
        {
            throw new System.NotImplementedException();
        }

        public override bool Revoke(params object[] args)
        {
            throw new System.NotImplementedException();
        }
    }
}