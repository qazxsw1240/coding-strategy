#nullable enable


using CodingStrategy.Entities.Robot;

namespace CodingStrategy.Entities.Runtime.Statement
{
    public class AttackStatement : IStatement
    {
        private readonly IRobotAttackStrategy _strategy;
        private readonly IRobotDelegate _robotDelegate;
        private readonly Coordinate[] _coordinates;

        public AttackStatement(
            IRobotAttackStrategy strategy,
            IRobotDelegate robotDelegate,
            Coordinate[] coordinates)
        {
            _strategy = strategy;
            _robotDelegate = robotDelegate;
            _coordinates = coordinates;
        }

        public void Execute(RuntimeExecutorContext context)
        {
            bool result = _robotDelegate.Attack(_strategy, _coordinates);
            if (!result)
            {
                throw new ExecutionException("Cannot robot attack");
            }
        }

        public StatementPhase Phase => StatementPhase.Attack;

        public IStatement Reverse =>
            new AttackStatement(new RobotAttackReverseStrategy(_strategy), _robotDelegate, _coordinates);

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
