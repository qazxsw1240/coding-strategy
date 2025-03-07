#nullable enable

using CodingStrategy.Entities.Robot;
using CodingStrategy.Entities.Runtime.Statement;

namespace CodingStrategy.Entities.Runtime.Command
{
    public class RestorationCommand : AbstractCommand
    {
        public RestorationCommand(int enhancedLevel = 1)
            : base(CommandLoader.Load(24), enhancedLevel, 0)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            return keepStatus ? new RestorationCommand(Info.EnhancedLevel) : new RestorationCommand();
        }

        protected override void AddStatementOnLevel1(IRobotDelegate robotDelegate)
        {
            robotDelegate.EnergyPoint = 0;
            _commandBuilder.Append(new AddHealthPointStatement(robotDelegate, 4, 4));
        }

        protected override void AddStatementOnLevel2(IRobotDelegate robotDelegate) {}

        protected override void AddStatementOnLevel3(IRobotDelegate robotDelegate) {}
    }
}
