#nullable enable

using CodingStrategy.Entities.Robot;

namespace CodingStrategy.Entities.Runtime.Command
{
    public class EmptyCommand : AbstractCommand
    {
        public EmptyCommand(int enhancedLevel = 0)
            : base(CommandLoader.Load(0), enhancedLevel, 0)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            return keepStatus ? new EmptyCommand(Info.EnhancedLevel) : new EmptyCommand();
        }

        protected override void AddStatementOnLevel1(IRobotDelegate robotDelegate) {}

        protected override void AddStatementOnLevel2(IRobotDelegate robotDelegate) {}

        protected override void AddStatementOnLevel3(IRobotDelegate robotDelegate) {}
    }
}
