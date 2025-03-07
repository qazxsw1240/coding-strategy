#nullable enable

using CodingStrategy.Entities.Robot;
using CodingStrategy.Entities.Runtime.Statement;

namespace CodingStrategy.Entities.Runtime.Command
{
    public class EnergyStorageIncreaseCommand : AbstractCommand
    {
        public EnergyStorageIncreaseCommand(int enhancedLevel = 1)
            : base(CommandLoader.Load(19), enhancedLevel, 0)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            return keepStatus
                ? new EnergyStorageIncreaseCommand(Info.EnhancedLevel)
                : new EnergyStorageIncreaseCommand();
        }

        protected override void AddStatementOnLevel1(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Append(new AddMaxEnergyStatement(robotDelegate, Info.EnhancedLevel));
        }

        protected override void AddStatementOnLevel2(IRobotDelegate robotDelegate) {}

        protected override void AddStatementOnLevel3(IRobotDelegate robotDelegate) {}
    }
}
