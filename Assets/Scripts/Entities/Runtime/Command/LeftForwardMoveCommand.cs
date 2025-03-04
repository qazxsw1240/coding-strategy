#nullable enable

using CodingStrategy.Entities.Robot;
using CodingStrategy.Entities.Runtime.Statement;

namespace CodingStrategy.Entities.Runtime.Command
{
    public class LeftForwardMoveCommand : AbstractCommand
    {
        private static readonly Coordinate Coordinate = new Coordinate(-1, 1);

        public LeftForwardMoveCommand(int enhancedLevel = 1)
            : base(CommandLoader.Load(6), enhancedLevel, 0)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            return keepStatus ? new LeftForwardMoveCommand(Info.EnhancedLevel) : new LeftForwardMoveCommand();
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
