#nullable enable

using System.Collections.Generic;
using System.Linq;

using CodingStrategy.Entities.Robot;
using CodingStrategy.Entities.Runtime.Statement;

namespace CodingStrategy.Entities.Runtime.Command
{
    public class AttackBackCommand : AbstractCommand
    {
        private readonly IList<Coordinate> _coordinates = new List<Coordinate>();

        public AttackBackCommand(
            string id = "16",
            string name = "뒷 공격",
            int enhancedLevel = 1,
            int grade = 1,
            string explanation = "사용시 공격 범위에 해당하는  칸에 있는 로봇을 본인 공격력 수치만큼 공격합니다. 에너지를 1 소모합니다.")
            : base(id, name, enhancedLevel, grade, 1, explanation)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            if (!keepStatus)
            {
                return new AttackBackCommand();
            }
            return new AttackBackCommand(Id, Info.Name, Info.EnhancedLevel, Info.Grade);
        }

        protected override void AddStatementOnLevel1(IRobotDelegate robotDelegate)
        {
            _coordinates.Add(new Coordinate(0, -1));
            _commandBuilder.Append(new AttackStatement(robotDelegate, new AttackStrategy(), _coordinates.ToArray()));
        }

        protected override void AddStatementOnLevel2(IRobotDelegate robotDelegate) {}

        protected override void AddStatementOnLevel3(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Clear();
            _coordinates.Add(new Coordinate(1, -1));
            _coordinates.Add(new Coordinate(-1, -1));
            _commandBuilder.Append(new AttackStatement(robotDelegate, new AttackStrategy(), _coordinates.ToArray()));
        }

        private class AttackStrategy : IRobotAttackStrategy
        {
            public int CalculateAttackPoint(IRobotDelegate attacker, IRobotDelegate target)
            {
                return attacker.AttackPoint;
            }
        }
    }
}
