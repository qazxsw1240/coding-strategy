#nullable enable


namespace CodingStrategy.Entities.Runtime.CommandImpl
{
    using System.Collections.Generic;
    using CodingStrategy.Entities.Player;
    using CodingStrategy.Entities.Runtime.Abnormality;
    using Robot;
    using Statement;

    public class CoinMiningCommand : AbstractCommand
    {
        private readonly int _coefficient;
        private readonly IPlayerDelegate[] _target=new IPlayerDelegate[1];
        public CoinMiningCommand(string id="11", string name="코인 채굴", int enhancedLevel=1, int grade=4)
        : base(id, name, enhancedLevel, grade, 0)
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
            _target[0]=Context?.PlayerDelegate!;
            return base.GetCommandStatements(robot);
        }

        public override bool Invoke(params object[] args)
        {
            throw new System.NotImplementedException();
        }

        public override bool Revoke(params object[] args)
        {
            throw new System.NotImplementedException();
        }

        protected override void AddStatementOnLevel1(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Append(new AddCurrencyTargetPlayerStatement(robotDelegate, _target, _coefficient, Stack.Name));
        }

        protected override void AddStatementOnLevel2(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Append(new AddCurrencyTargetPlayerStatement(robotDelegate, _target, _coefficient, Stack.Name));
        }

        protected override void AddStatementOnLevel3(IRobotDelegate robotDelegate)
        {
            _commandBuilder.AppendFirst(new SuperStatement(robotDelegate));
        }
    }
}