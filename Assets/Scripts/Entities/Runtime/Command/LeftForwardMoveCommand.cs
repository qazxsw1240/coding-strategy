#nullable enable

using CodingStrategy.Entities.Robot;
using CodingStrategy.Entities.Runtime.Statement;

namespace CodingStrategy.Entities.Runtime.Command
{
    public class LeftForwardMoveCommand : AbstractCommand
    {
        private static readonly Coordinate _coordinate = new Coordinate(-1, 1);

        public LeftForwardMoveCommand(
            string id = "6",
            string name = "좌측 대각선 이동",
            int enhancedLevel = 1,
            int grade = 2,
            string explanation = "바라보는 기준에서 왼쪽 앞 대각선 방향으로 1칸 이동합니다.")
            : base(id, name, enhancedLevel, grade, 0, explanation)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            if (!keepStatus)
            {
                return new LeftForwardMoveCommand();
            }
            return new LeftForwardMoveCommand(Id, Info.Name, Info.EnhancedLevel, Info.Grade);
        }

        protected override void AddStatementOnLevel1(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Append(new MoveCoordinateStatement(robotDelegate, _coordinate));
        }

        protected override void AddStatementOnLevel2(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Append(new MoveCoordinateStatement(robotDelegate, _coordinate));
        }

        protected override void AddStatementOnLevel3(IRobotDelegate robotDelegate)
        {
            _commandBuilder.AppendFirst(new SuperStatement(robotDelegate));
        }
    }
}
