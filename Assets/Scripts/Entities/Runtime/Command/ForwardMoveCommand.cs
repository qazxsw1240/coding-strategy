#nullable enable

using CodingStrategy.Entities.Robot;
using CodingStrategy.Entities.Runtime.Statement;

namespace CodingStrategy.Entities.Runtime.Command
{
    public class ForwardMoveCommand : AbstractCommand
    {
        private static readonly Coordinate Coordinate = new Coordinate(0, 1);

        public ForwardMoveCommand(int enhancedLevel = 1)
            : base(CommandLoader.Load(1), enhancedLevel, 0)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            return keepStatus
                ? new ForwardMoveCommand(Info.EnhancedLevel)
                : new ForwardMoveCommand();
        }

        protected override void AddStatementOnLevel1(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Append(new MoveCoordinateStatement(robotDelegate, Coordinate));
        }

        protected override void AddStatementOnLevel2(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Append(new MoveCoordinateStatement(robotDelegate, Coordinate));
        }

        protected override void AddStatementOnLevel3(IRobotDelegate robotDelegate)
        {
            _commandBuilder.AppendFirst(new SuperStatement(robotDelegate));
        }
    }
}
