#nullable enable

using CodingStrategy.Entities.Robot;
using CodingStrategy.Entities.Runtime.Statement;

namespace CodingStrategy.Entities.Runtime.CommandImpl
{
    public class MoveRightForwardCommand : AbstractCommand
    {
        private static readonly Coordinate _coordinate = new Coordinate(1, 1);

        public MoveRightForwardCommand(
            string id = "7",
            string name = "우측 대각선 이동",
            int enhancedLevel = 1,
            int grade = 2,
            string explanation = "바라보는 기준에서 오른쪽 앞 대각선 방향으로 1칸 이동합니다.")
            : base(id, name, enhancedLevel, grade, 0, explanation)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            if (!keepStatus)
            {
                return new MoveRightForwardCommand();
            }
            return new MoveRightForwardCommand(Id, Info.Name, Info.EnhancedLevel, Info.Grade);
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
