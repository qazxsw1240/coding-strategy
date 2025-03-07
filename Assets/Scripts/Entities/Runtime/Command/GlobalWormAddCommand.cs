#nullable enable

using CodingStrategy.Entities.Robot;
using CodingStrategy.Entities.Runtime.Abnormality;
using CodingStrategy.Entities.Runtime.Statement;

namespace CodingStrategy.Entities.Runtime.Command
{
    public class GlobalWormAddCommand : AbstractCommand
    {
        public GlobalWormAddCommand(int enhancedLevel = 1)
            : base(CommandLoader.Load(9), enhancedLevel, 0)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            return keepStatus
                ? new GlobalWormAddCommand(Info.EnhancedLevel)
                : new GlobalWormAddCommand();
        }

        protected override void AddStatementOnLevel1(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Append(new AddAbnormalityAllEnemyStatement(robotDelegate, new Worm(robotDelegate), 1));
        }

        protected override void AddStatementOnLevel2(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Append(new AddAbnormalityAllEnemyStatement(robotDelegate, new Worm(robotDelegate), 1));
        }

        protected override void AddStatementOnLevel3(IRobotDelegate robotDelegate)
        {
            _commandBuilder.AppendFirst(new SuperStatement(robotDelegate));
        }
    }
}
