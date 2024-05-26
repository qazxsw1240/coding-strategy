#nullable enable


using System.Collections.Generic;
using CodingStrategy.Entities;

namespace CodingStrategy.Network
{
    public class PhotonPlayerCommandCache : IPlayerCommandCache
    {
        private static readonly int[] SellAmounts = new int[] { 1, 3, 9 };

        private static readonly IDictionary<string, ICommand> CommandCache = new Dictionary<string, ICommand>();

        private readonly IPlayerCommandNetworkDelegate _networkDelegate;

        public PhotonPlayerCommandCache() : this(new PhotonPlayerCommandNetworkDelegate()) {}

        public PhotonPlayerCommandCache(IPlayerCommandNetworkDelegate networkDelegate)
        {
            _networkDelegate = networkDelegate;
            _networkDelegate.RequestRefresh();
        }

        public static IDictionary<string, ICommand> GetCachedCommands()
        {
            return CommandCache;
        }

        public static void AttachCommand(ICommand command)
        {
            CommandCache[command.Id] = command;
        }

        public static void AttachCommands(IDictionary<string, ICommand> commands)
        {
            foreach ((string key, ICommand value) in commands)
            {
                CommandCache[key] = value;
            }
        }

        public static void AttachCommands(IEnumerable<ICommand> commands)
        {
            foreach (ICommand command in commands)
            {
                CommandCache[command.Id] = command;
            }
        }

        public ICommand? Buy(string id, int count)
        {
            int currentCount = _networkDelegate.GetCachedCommandCount()[id];

            if (currentCount < count)
            {
                return null;
            }

            _networkDelegate.ModifyCommandCount(id, currentCount - count);

            return CommandCache[id];
        }

        public bool Sell(ICommand command)
        {
            string id = command.Id;
            int enhancedLevel = command.Info.EnhancedLevel;
            int currentCount = _networkDelegate.GetCachedCommandCount()[id];
            int sellAmount = SellAmounts[enhancedLevel - 1];
            _networkDelegate.ModifyCommandCount(id, currentCount + sellAmount);
            return true;
        }
    }
}
