#nullable enable

using CodingStrategy.Entities.Robot;
using CodingStrategy.Entities.Runtime.Statement;

namespace CodingStrategy.Entities.Runtime.Command
{
    public class DashCommand : AbstractCommand
    {
        public DashCommand(int enhancedLevel = 1)
            : base(CommandLoader.Load(18), enhancedLevel, 1)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            return keepStatus ? new DashCommand(Info.EnhancedLevel) : new DashCommand();
        }

        protected override void AddStatementOnLevel1(IRobotDelegate robotDelegate)
        {
            _commandBuilder
               .Append(new MoveStatement(robotDelegate, 1))
               .Append(new MoveStatement(robotDelegate, 1))
               .Append(new MoveStatement(robotDelegate, 1));
        }

        protected override void AddStatementOnLevel2(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Append(new MoveStatement(robotDelegate, 1));
        }

        protected override void AddStatementOnLevel3(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Append(new MoveStatement(robotDelegate, 1));
        }
    }
}
