#nullable enable

using System.Collections.Generic;

using CodingStrategy.Entities.Robot;
using CodingStrategy.Entities.Runtime.Statement;

namespace CodingStrategy.Entities.Runtime.Command
{
    public class EnergyStorageIncreaseCommand : AbstractCommand
    {
        private readonly IList<Coordinate> _coordinates = new List<Coordinate>();

        public EnergyStorageIncreaseCommand(
            string id = "19",
            string name = "에너지 스토리지 확보",
            int enhancedLevel = 1,
            int grade = 2,
            string explanation = "사용시 모든 에너지를 소모합니다. 소모된 에너지가 1 이상인 경우 최대 에너지 량을 1칸 늘립니다.")
            : base(id, name, enhancedLevel, grade, 0, explanation)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            if (!keepStatus)
            {
                return new EnergyStorageIncreaseCommand();
            }
            return new EnergyStorageIncreaseCommand(Id, Info.Name, Info.EnhancedLevel, Info.Grade);
        }

        protected override void AddStatementOnLevel1(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Append(new AddMaxEnergyStatement(robotDelegate, Info.EnhancedLevel));
        }

        protected override void AddStatementOnLevel2(IRobotDelegate robotDelegate) {}

        protected override void AddStatementOnLevel3(IRobotDelegate robotDelegate) {}
    }
}
