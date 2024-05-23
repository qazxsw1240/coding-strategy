using System.Collections.Generic;

namespace CodingStrategy.Network
{
    public interface IPlayerCommandNetworkDelegate
    {
        public abstract void RequestRefresh();

        public abstract IDictionary<string, int> GetCachedCommandCount();
    }
}
