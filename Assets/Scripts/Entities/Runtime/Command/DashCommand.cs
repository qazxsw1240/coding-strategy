#nullable enable

using System.Collections.Generic;

using CodingStrategy.Entities.Robot;
using CodingStrategy.Entities.Runtime.Statement;

namespace CodingStrategy.Entities.Runtime.Command
{
    public class DashCommand : AbstractCommand
    {
        private readonly IList<Coordinate> _coordinates = new List<Coordinate>();

        public DashCommand(
            string id = "18",
            string name = "돌진",
            int enhancedLevel = 1,
            int grade = 2,
            string explanation = "바라보는 기준에서 앞으로 3칸 이동합니다. 에너지를 1 소모합니다.")
            : base(id, name, enhancedLevel, grade, 1, explanation)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            if (!keepStatus)
            {
                return new DashCommand();
            }
            return new DashCommand(Id, Info.Name, Info.EnhancedLevel, Info.Grade);
        }

        protected override void AddStatementOnLevel1(IRobotDelegate robotDelegate)
        {
            _commandBuilder
               .Append(new MoveStatement(robotDelegate, 1))
               .Append(new MoveStatement(robotDelegate, 1))
               .Append(new MoveStatement(robotDelegate, 1));
        }

        protected override void AddStatementOnLevel2(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Append(new MoveStatement(robotDelegate, 1));
        }

        protected override void AddStatementOnLevel3(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Append(new MoveStatement(robotDelegate, 1));
        }
    }
}
