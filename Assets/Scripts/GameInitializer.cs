using System.Collections.Generic;

using CodingStrategy.Entities;
using CodingStrategy.Network;

namespace CodingStrategy
{
    public static class GameInitializer
    {
        public static readonly List<int> StockCountsByCommandGrade = new List<int>() { 0, 128, 108, 72, 48, 36 };

        public const int DefaultGrade1CommandStockCount = 128;
        public const int DefaultGrade2CommandStockCount = 108;
        public const int DefaultGrade3CommandStockCount = 72;
        public const int DefaultGrade4CommandStockCount = 48;
        public const int DefaultGrade5CommandStockCount = 36;

        private static void AddCommand(ICommand command, int count)
        {
            string id = command.ID;
            PhotonPlayerCommandCache.AttachCommand(command);
            PhotonPlayerCommandNetworkDelegate.AttachCommandIdCount(id, count);
        }

        public static void AddCommand(ICommand command)
        {
            int stockCount = StockCountsByCommandGrade[command.Info.Grade];
            AddCommand(command, stockCount);
        }

        public static void Initialize()
        {
            foreach (ICommand command in CommandListFactory.GetCommands)
            {
                AddCommand(command);
            }
        }
    }
}
