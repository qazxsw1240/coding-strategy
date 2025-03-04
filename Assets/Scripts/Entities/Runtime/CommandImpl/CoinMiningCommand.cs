#nullable enable

using System.Collections.Generic;

using CodingStrategy.Entities.Player;
using CodingStrategy.Entities.Robot;
using CodingStrategy.Entities.Runtime.Abnormality;
using CodingStrategy.Entities.Runtime.Statement;

namespace CodingStrategy.Entities.Runtime.CommandImpl
{
    public class CoinMiningCommand : AbstractCommand
    {
        private readonly int _coefficient;
        private readonly IPlayerDelegate[] _target = new IPlayerDelegate[1];

        public CoinMiningCommand(
            string id = "11",
            string name = "코인 채굴",
            int enhancedLevel = 1,
            int grade = 4,
            string explanation = "사용시 자신의 스택 계수만큼 비트를 획득합니다.")
            : base(id, name, enhancedLevel, grade, 0, explanation)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            if (!keepStatus)
            {
                return new CoinMiningCommand();
            }
            return new CoinMiningCommand(Id, Info.Name, Info.EnhancedLevel, Info.Grade);
        }

        public override IList<IStatement> GetCommandStatements(IRobotDelegate robot)
        {
            _target[0] = Context?.PlayerDelegate!;
            return base.GetCommandStatements(robot);
        }

        protected override void AddStatementOnLevel1(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Append(
                new AddCurrencyTargetPlayerStatement(robotDelegate, _target, _coefficient, Stack.Name));
        }

        protected override void AddStatementOnLevel2(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Append(
                new AddCurrencyTargetPlayerStatement(robotDelegate, _target, _coefficient, Stack.Name));
        }

        protected override void AddStatementOnLevel3(IRobotDelegate robotDelegate)
        {
            _commandBuilder.AppendFirst(new SuperStatement(robotDelegate));
        }
    }
}
