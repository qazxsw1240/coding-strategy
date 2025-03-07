#nullable enable

using CodingStrategy.Entities.Robot;
using CodingStrategy.Entities.Runtime.Statement;

namespace CodingStrategy.Entities.Runtime.Command
{
    public class RightRightCommand : AbstractCommand
    {
        private static readonly Coordinate Coordinate = new Coordinate(1, 0);

        public RightRightCommand(int enhancedLevel = 1)
            : base(CommandLoader.Load(3), enhancedLevel, 0)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            return keepStatus ? new RightRightCommand(Info.EnhancedLevel) : new RightRightCommand();
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
