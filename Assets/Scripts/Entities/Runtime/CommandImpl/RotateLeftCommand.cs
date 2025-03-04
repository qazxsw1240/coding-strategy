#nullable enable

using CodingStrategy.Entities.Robot;
using CodingStrategy.Entities.Runtime.Statement;

namespace CodingStrategy.Entities.Runtime.CommandImpl
{
    public class RotateLeftCommand : AbstractCommand
    {
        public RotateLeftCommand(
            string id = "4",
            string name = "좌회전",
            int enhancedLevel = 1,
            int grade = 1,
            string explanation = "바라보는 기준에서 왼쪽으로 90도 회전합니다.")
            : base(id, name, enhancedLevel, grade, 0, explanation)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            if (!keepStatus)
            {
                return new RotateLeftCommand();
            }
            return new RotateLeftCommand(Id, Info.Name, Info.EnhancedLevel, Info.Grade);
        }

        protected override void AddStatementOnLevel1(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Append(new RotateStatement(robotDelegate, -1));
        }

        protected override void AddStatementOnLevel2(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Append(new RotateStatement(robotDelegate, -1));
        }

        protected override void AddStatementOnLevel3(IRobotDelegate robotDelegate)
        {
            _commandBuilder.AppendFirst(new SuperStatement(robotDelegate));
        }
    }
}
