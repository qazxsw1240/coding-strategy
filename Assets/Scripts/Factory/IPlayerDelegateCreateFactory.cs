using CodingStrategy.Entities.Player;

namespace CodingStrategy.Factory
{
    public interface IPlayerDelegateCreateFactory
    {
        public abstract IPlayerDelegateCreateStrategy Strategy { get; }

        public abstract IPlayerDelegate Build();
    }
}
