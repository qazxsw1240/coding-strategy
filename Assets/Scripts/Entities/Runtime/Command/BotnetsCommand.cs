#nullable enable

using CodingStrategy.Entities.Robot;
using CodingStrategy.Entities.Runtime.Statement;

namespace CodingStrategy.Entities.Runtime.Command
{
    public class BotnetsCommand : AbstractCommand
    {
        private static readonly Coordinate[] RelativePosition =
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

        public BotnetsCommand(int enhancedLevel = 1)
            : base(CommandLoader.Load(12), enhancedLevel, 0)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            return keepStatus ? new BotnetsCommand(Info.EnhancedLevel) : new BotnetsCommand();
        }

        protected override void AddStatementOnLevel1(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Append(
                new AttackStatement(robotDelegate, new AttackStrategy(Info.EnhancedLevel), RelativePosition));
        }

        protected override void AddStatementOnLevel2(IRobotDelegate robotDelegate) {}

        protected override void AddStatementOnLevel3(IRobotDelegate robotDelegate) {}

        private class AttackStrategy : IRobotAttackStrategy
        {
            private static readonly int[] DamageArray = { 0, 1, 2, 4 };

            private readonly int _damage;

            public AttackStrategy(int enhancedLevel)
            {
                _damage = DamageArray[enhancedLevel];
            }

            public int CalculateAttackPoint(IRobotDelegate attacker, IRobotDelegate target)
            {
                return _damage;
            }
        }
    }
}
