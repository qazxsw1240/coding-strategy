#nullable enable

using CodingStrategy.Entities.Robot;
using CodingStrategy.Entities.Runtime.Statement;

namespace CodingStrategy.Entities.Runtime.Command
{
    public class SelfRepairCommand : AbstractCommand
    {
        public SelfRepairCommand(int enhancedLevel = 1)
            : base(CommandLoader.Load(22), enhancedLevel, 2)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            return keepStatus ? new SelfRepairCommand(Info.EnhancedLevel) : new SelfRepairCommand();
        }

        protected override void AddStatementOnLevel1(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Append(new AddHealthPointStatement(robotDelegate, Info.EnhancedLevel, 2));
        }

        protected override void AddStatementOnLevel2(IRobotDelegate robotDelegate) {}

        protected override void AddStatementOnLevel3(IRobotDelegate robotDelegate) {}
    }
}
