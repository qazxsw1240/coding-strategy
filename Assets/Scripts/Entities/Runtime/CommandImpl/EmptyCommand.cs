#nullable enable

using CodingStrategy.Entities.Robot;

namespace CodingStrategy.Entities.Runtime.CommandImpl
{
    public class EmptyCommand : AbstractCommand
    {
        public EmptyCommand(
            string id = "0",
            string name = "빈 커맨드",
            int enhancedLevel = 0,
            int grade = 0,
            string explanation = "")
            : base(id, name, enhancedLevel, grade, 0, explanation)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            if (!keepStatus)
            {
                return new EmptyCommand();
            }
            return new EmptyCommand(Id, Info.Name, Info.EnhancedLevel, Info.Grade);
        }

        protected override void AddStatementOnLevel1(IRobotDelegate robotDelegate) {}

        protected override void AddStatementOnLevel2(IRobotDelegate robotDelegate) {}

        protected override void AddStatementOnLevel3(IRobotDelegate robotDelegate) {}
    }
}
