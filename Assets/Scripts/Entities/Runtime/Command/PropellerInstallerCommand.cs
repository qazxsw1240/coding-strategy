#nullable enable

using System.Collections.Generic;

using CodingStrategy.Entities.BadSector;
using CodingStrategy.Entities.Board;
using CodingStrategy.Entities.Robot;
using CodingStrategy.Entities.Runtime.Statement;

namespace CodingStrategy.Entities.Runtime.Command
{
    public class PropellerInstallerCommand : AbstractCommand
    {
        public static int installNum;
        private readonly List<Coordinate> _coordinates = new List<Coordinate>();

        public PropellerInstallerCommand(
            string id = "14",
            string name = "추진체 설치",
            int enhancedLevel = 1,
            int grade = 1,
            string explanation = "사용시 공격 범위에 해당하는  칸에 로봇이 바라보는 방향의 반대 방향으로 로봇이 이동하게 만드는 배드섹터를 설치합니다. 에너지를 1 소모합니다.")
            : base(id, name, enhancedLevel, grade, 1, explanation)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            if (!keepStatus)
            {
                return new PropellerInstallerCommand();
            }
            return new PropellerInstallerCommand(Id, Info.Name, Info.EnhancedLevel, Info.Grade);
        }

        public IBadSectorDelegate InstallPropellerBadSector(IBoardDelegate boardDelegate, IRobotDelegate robotDelegate)
        {
            return new PropellerBadSector(robotDelegate.Id + "-" + installNum++, boardDelegate, robotDelegate);
        }

        protected override void AddStatementOnLevel1(IRobotDelegate robotDelegate)
        {
            _coordinates.Add(new Coordinate(0, 1));
            _commandBuilder.Append(new PointerStatement(robotDelegate, InstallPropellerBadSector, _coordinates));
        }

        protected override void AddStatementOnLevel2(IRobotDelegate robotDelegate) {}

        protected override void AddStatementOnLevel3(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Clear();
            _coordinates.Add(new Coordinate(-1, 1));
            _coordinates.Add(new Coordinate(1, 1));
            _commandBuilder.Append(new PointerStatement(robotDelegate, InstallPropellerBadSector, _coordinates));
        }
    }
}
