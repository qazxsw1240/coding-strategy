#nullable enable


using System.Windows.Input;

namespace CodingStrategy.Network
{
    public interface IPlayerCommandCache
    {
        public abstract ICommand Buy(string id, int count);

        public abstract bool Sell(ICommand command);
    }
}
