#nullable enable

using CodingStrategy.Entities.Robot;
using CodingStrategy.Entities.Runtime.Abnormality;
using CodingStrategy.Entities.Runtime.Statement;

namespace CodingStrategy.Entities.Runtime.Command
{
    public class StackAddCommand : AbstractCommand
    {
        public StackAddCommand(int enhancedLevel = 1)
            : base(CommandLoader.Load(8), enhancedLevel, 0)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            return keepStatus
                ? new StackAddCommand(Info.EnhancedLevel)
                : new StackAddCommand();
        }

        protected override void AddStatementOnLevel1(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Append(
                new AddAbnormalitySpecificRobotStatement(robotDelegate, new Stack(robotDelegate), 2));
        }

        protected override void AddStatementOnLevel2(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Append(
                new AddAbnormalitySpecificRobotStatement(robotDelegate, new Stack(robotDelegate), 2));
        }

        protected override void AddStatementOnLevel3(IRobotDelegate robotDelegate)
        {
            _commandBuilder.AppendFirst(new SuperStatement(robotDelegate));
        }
    }
}
