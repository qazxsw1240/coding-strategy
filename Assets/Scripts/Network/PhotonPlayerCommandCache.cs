#nullable enable

using System.Collections.Generic;

using CodingStrategy.Entities;

using UnityEngine;

namespace CodingStrategy.Network
{
    public class PhotonPlayerCommandCache : IPlayerCommandCache
    {
        private static readonly int[] SellAmounts = { 1, 3, 9 };

        private static readonly IDictionary<string, ICommand> CommandCache = new Dictionary<string, ICommand>();

        private readonly IPlayerCommandNetworkDelegate _networkDelegate;

        public PhotonPlayerCommandCache() : this(new PhotonPlayerCommandNetworkDelegate()) {}

        public PhotonPlayerCommandCache(IPlayerCommandNetworkDelegate networkDelegate)
        {
            _networkDelegate = networkDelegate;
            _networkDelegate.RequestRefresh();
        }

        public ICommand? Buy(string id, int count)
        {
            int currentCount = _networkDelegate.GetCachedCommandCount()[id];

            if (currentCount < count)
            {
                return null;
            }

            Debug.LogFormat("Buy {0} of Command {1}", count, id);
            _networkDelegate.ModifyCommandCount(id, currentCount - count);

            return CommandCache[id];
        }

        public bool Sell(ICommand command)
        {
            string id = command.ID;
            if (!_networkDelegate.GetCachedCommandCount().ContainsKey(id))
            {
                return false;
            }
            int enhancedLevel = command.Info.EnhancedLevel;
            int currentCount = _networkDelegate.GetCachedCommandCount()[id];
            int sellAmount = SellAmounts[enhancedLevel - 1];
            Debug.LogFormat("Sell {0} of Command {1}", sellAmount, id);
            _networkDelegate.ModifyCommandCount(id, currentCount + sellAmount);
            return true;
        }

        public static IDictionary<string, ICommand> GetCachedCommands()
        {
            return CommandCache;
        }

        public static void AttachCommand(ICommand command)
        {
            CommandCache[command.ID] = command;
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
                CommandCache[command.ID] = command;
            }
        }
    }
}
