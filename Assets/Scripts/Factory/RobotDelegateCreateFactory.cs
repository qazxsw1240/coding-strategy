#nullable enable

using CodingStrategy.Entities.Board;
using CodingStrategy.Entities.Player;
using CodingStrategy.Entities.Robot;

namespace CodingStrategy.Factory
{
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
            IRobotDelegate robotDelegate = new RobotDelegateImpl(
                _playerDelegate.Id,
                _boardDelegate,
                _playerDelegate.Algorithm,
                Strategy.HealthPoint,
                Strategy.EnergyPoint,
                Strategy.ArmorPoint,
                Strategy.AttackPoint);
            _playerDelegate.Robot = robotDelegate;
            return robotDelegate;
        }
    }
}
