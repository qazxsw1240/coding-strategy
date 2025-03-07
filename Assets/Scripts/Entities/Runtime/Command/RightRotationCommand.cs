#nullable enable

using CodingStrategy.Entities.Robot;
using CodingStrategy.Entities.Runtime.Statement;

namespace CodingStrategy.Entities.Runtime.Command
{
    public class RightRotationCommand : AbstractCommand
    {
        public RightRotationCommand(int enhancedLevel = 1)
            : base(CommandLoader.Load(5), enhancedLevel, 0)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            return keepStatus ? new RightRotationCommand(Info.EnhancedLevel) : new RightRotationCommand();
        }

        protected override void AddStatementOnLevel1(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Append(new RotateStatement(robotDelegate, 1));
        }

        protected override void AddStatementOnLevel2(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Append(new RotateStatement(robotDelegate, 1));
        }

        protected override void AddStatementOnLevel3(IRobotDelegate robotDelegate)
        {
            _commandBuilder.AppendFirst(new SuperStatement(robotDelegate));
        }
    }
}
