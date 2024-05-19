#nullable enable


namespace CodingStrategy.Factory
{
    using Entities.Board;
    using Entities.Player;
    using Entities.Robot;

    public class RobotDelegateCreateFactory : IRobotDelegateCreateFactory
    {
        private readonly IBoardDelegate _boardDelegate;
        private readonly IPlayerDelegate _playerDelegate;

        public RobotDelegateCreateFactory(
            IRobotDelegateCreateStrategy strategy,
            IBoardDelegate boardDelegate,
            IPlayerDelegate playerDelegate)
        {
            Strategy = strategy;
            _boardDelegate = boardDelegate;
            _playerDelegate = playerDelegate;
        }

        public IRobotDelegateCreateStrategy Strategy { get; }

        public IRobotDelegate Build()
        {
            IRobotDelegate robotDelegate = new RobotDelegateImpl(_playerDelegate.Id,
                _boardDelegate,
                _playerDelegate.Algorithm,
                healthPoint: Strategy.HealthPoint,
                energyPoint: Strategy.EnergyPoint,
                armorPoint: Strategy.ArmorPoint,
                attackPoint: Strategy.AttackPoint);
            _playerDelegate.Robot = robotDelegate;
            return robotDelegate;
        }
    }
}
