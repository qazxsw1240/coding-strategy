#nullable enable

using CodingStrategy.Entities.Robot;
using CodingStrategy.Entities.Runtime.Statement;

namespace CodingStrategy.Entities.Runtime.Command
{
    public class EnergyChargeCommand : AbstractCommand
    {
        public EnergyChargeCommand(int enhancedLevel = 1)
            : base(CommandLoader.Load(17), enhancedLevel, 0)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            return keepStatus
                ? new EnergyChargeCommand(Info.EnhancedLevel)
                : new EnergyChargeCommand();
        }

        protected override void AddStatementOnLevel1(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Append(new AddEnergyStatement(robotDelegate, Info.EnhancedLevel));
        }

        protected override void AddStatementOnLevel2(IRobotDelegate robotDelegate) {}

        protected override void AddStatementOnLevel3(IRobotDelegate robotDelegate) {}
    }
}
