#nullable enable


using CodingStrategy.Entities.Robot;

namespace CodingStrategy.Entities.Runtime.Statement
{
    public class AttackStatement : AbstractStatement
    {
        private readonly IRobotAttackStrategy _strategy;
        private readonly Coordinate[] _coordinates;

        public AttackStatement(
            IRobotDelegate robotDelegate,
            IRobotAttackStrategy strategy,
            Coordinate[] coordinates)
            : base(robotDelegate)
        {
            _strategy = strategy;
            _coordinates = coordinates;
        }

        public override void Execute(RuntimeExecutorContext context)
        {
            base.Execute(context);
            bool result = _robotDelegate.Attack(_strategy, _coordinates);
            if (!result)
            {
                throw new ExecutionException("Cannot robot attack");
            }
        }

        public override StatementPhase Phase => StatementPhase.Attack;

        public override IStatement Reverse =>
            new AttackStatement(_robotDelegate, new RobotAttackReverseStrategy(_strategy), _coordinates);

        private class RobotAttackReverseStrategy : IRobotAttackStrategy
        {
            private readonly IRobotAttackStrategy _strategy;

            public RobotAttackReverseStrategy(IRobotAttackStrategy strategy)
            {
                _strategy = strategy;
            }

            public int CalculateAttackPoint(IRobotDelegate attacker, IRobotDelegate target)
            {
                return -_strategy.CalculateAttackPoint(attacker, target);
            }
        }
    }
}
