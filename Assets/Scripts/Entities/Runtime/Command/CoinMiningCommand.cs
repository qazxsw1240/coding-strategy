#nullable enable

using System.Collections.Generic;

using CodingStrategy.Entities.Player;
using CodingStrategy.Entities.Robot;
using CodingStrategy.Entities.Runtime.Abnormality;
using CodingStrategy.Entities.Runtime.Statement;

namespace CodingStrategy.Entities.Runtime.Command
{
    public class CoinMiningCommand : AbstractCommand
    {
        private readonly int _coefficient;
        private readonly IPlayerDelegate[] _target = new IPlayerDelegate[1];

        private readonly AbnormalityProfile _abnormalityProfile;

        public CoinMiningCommand(int enhancedLevel = 1)
            : base(CommandLoader.Load(11), enhancedLevel, 0)
        {
            _abnormalityProfile = AbnormalityLoader.Load("Stack");
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            return keepStatus ? new CoinMiningCommand(Info.EnhancedLevel) : new CoinMiningCommand();
        }

        public override IList<IStatement> GetCommandStatements(IRobotDelegate robot)
        {
            _target[0] = Context?.PlayerDelegate!;
            return base.GetCommandStatements(robot);
        }

        protected override void AddStatementOnLevel1(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Append(
                new AddCurrencyTargetPlayerStatement(robotDelegate, _target, _coefficient, _abnormalityProfile.Name));
        }

        protected override void AddStatementOnLevel2(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Append(
                new AddCurrencyTargetPlayerStatement(robotDelegate, _target, _coefficient, _abnormalityProfile.Name));
        }

        protected override void AddStatementOnLevel3(IRobotDelegate robotDelegate)
        {
            _commandBuilder.AppendFirst(new SuperStatement(robotDelegate));
        }
    }
}
