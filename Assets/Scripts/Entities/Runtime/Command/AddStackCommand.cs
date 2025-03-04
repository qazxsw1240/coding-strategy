#nullable enable

using CodingStrategy.Entities.Robot;
using CodingStrategy.Entities.Runtime.Abnormality;
using CodingStrategy.Entities.Runtime.Statement;

namespace CodingStrategy.Entities.Runtime.CommandImpl
{
    public class AddStackCommand : AbstractCommand
    {
        public AddStackCommand(
            string id = "8",
            string name = "스택 추가",
            int enhancedLevel = 1,
            int grade = 2,
            string explanation = "자신의 캐릭터에게 스택 2만큼 부여합니다.")
            : base(id, name, enhancedLevel, grade, 0, explanation)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            return keepStatus
                ? new AddStackCommand(Id, Info.Name, Info.EnhancedLevel, Info.Grade)
                : new AddStackCommand();
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
