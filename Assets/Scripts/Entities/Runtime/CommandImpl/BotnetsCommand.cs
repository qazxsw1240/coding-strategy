#nullable enable

using CodingStrategy.Entities.Robot;
using CodingStrategy.Entities.Runtime.Statement;

namespace CodingStrategy.Entities.Runtime.CommandImpl
{
    public class BotnetsCommand : AbstractCommand
    {
        private static readonly Coordinate[] relativePosition =
        {
            new Coordinate(1, 0),
            new Coordinate(-1, 0),
            new Coordinate(0, 1),
            new Coordinate(0, -1),
            new Coordinate(1, 1),
            new Coordinate(1, -1),
            new Coordinate(-1, 1),
            new Coordinate(-1, -1)
        };

        public BotnetsCommand(
            string id = "12",
            string name = "봇네츠",
            int enhancedLevel = 1,
            int grade = 5,
            string explanation = "사용시 공격 범위에 해당하는 로봇의 체력을 1칸 깎습니다.")
            : base(id, name, enhancedLevel, grade, 0, explanation)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            if (!keepStatus)
            {
                return new BotnetsCommand();
            }
            return new BotnetsCommand(Id, Info.Name, Info.EnhancedLevel, Info.Grade);
        }

        protected override void AddStatementOnLevel1(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Append(
                new AttackStatement(robotDelegate, new AttackStrategy(Info.EnhancedLevel), relativePosition));
        }

        protected override void AddStatementOnLevel2(IRobotDelegate robotDelegate) {}

        protected override void AddStatementOnLevel3(IRobotDelegate robotDelegate) {}

        private class AttackStrategy : IRobotAttackStrategy
        {
            private static readonly int[] _damageArray = { 0, 1, 2, 4 };
            private readonly int _damage;

            public AttackStrategy(int enhancedLevel)
            {
                _damage = _damageArray[enhancedLevel];
            }

            public int CalculateAttackPoint(IRobotDelegate attacker, IRobotDelegate target)
            {
                return _damage;
            }
        }
    }
}
