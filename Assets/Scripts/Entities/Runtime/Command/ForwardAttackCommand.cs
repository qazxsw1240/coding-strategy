#nullable enable

using System.Collections.Generic;
using System.Linq;

using CodingStrategy.Entities.Robot;
using CodingStrategy.Entities.Runtime.Statement;

namespace CodingStrategy.Entities.Runtime.Command
{
    public class ForwardAttackCommand : AbstractCommand
    {
        private readonly IList<Coordinate> _coordinates = new List<Coordinate>();

        public ForwardAttackCommand(int enhancedLevel = 1)
            : base(CommandLoader.Load(15), enhancedLevel, 1)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            return keepStatus
                ? new ForwardAttackCommand(Info.EnhancedLevel)
                : new ForwardAttackCommand();
        }

        protected override void AddStatementOnLevel1(IRobotDelegate robotDelegate)
        {
            _coordinates.Add(new Coordinate(0, 1));
            _commandBuilder.Append(new AttackStatement(robotDelegate, new AttackStrategy(), _coordinates.ToArray()));
        }

        protected override void AddStatementOnLevel2(IRobotDelegate robotDelegate) {}

        protected override void AddStatementOnLevel3(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Clear();
            _coordinates.Add(new Coordinate(1, 1));
            _coordinates.Add(new Coordinate(-1, 1));
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
