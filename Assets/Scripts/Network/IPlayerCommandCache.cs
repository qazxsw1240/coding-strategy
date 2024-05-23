#nullable enable


using CodingStrategy.Entities;

namespace CodingStrategy.Network
{
    public interface IPlayerCommandCache
    {
        public abstract ICommand? Buy(string id, int count);

        public abstract bool Sell(ICommand command);
    }
}
