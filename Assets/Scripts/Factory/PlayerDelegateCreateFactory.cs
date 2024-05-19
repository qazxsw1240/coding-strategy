#nullable enable


using CodingStrategy.Entities.Player;

namespace CodingStrategy.Factory
{
    public class PlayerDelegateCreateFactory : IPlayerDelegateCreateFactory
    {
        public PlayerDelegateCreateFactory(IPlayerDelegateCreateStrategy strategy)
        {
            Strategy = strategy;
        }

        public IPlayerDelegateCreateStrategy Strategy { get; }

        public IPlayerDelegate Build()
        {
            return new PlayerDelegateImpl(Strategy.Id,
                Strategy.HealthPoint,
                Strategy.Level,
                Strategy.Exp,
                Strategy.Currency,
                null!,
                Strategy.Algorithm);
        }
    }
}
