using CodingStrategy.Entities;
using CodingStrategy.Entities.Runtime.Command;
using CodingStrategy.Network;

namespace CodingStrategy
{
    public static class GameInitializer
    {
        public const int DefaultGrade1CommandStockCount = 128;
        public const int DefaultGrade2CommandStockCount = 108;
        public const int DefaultGrade3CommandStockCount = 72;
        public const int DefaultGrade4CommandStockCount = 48;
        public const int DefaultGrade5CommandStockCount = 36;

        public static void AddCommand(ICommand command, int count)
        {
            string id = command.Id;

            PhotonPlayerCommandCache.AttachCommand(command);
            PhotonPlayerCommandNetworkDelegate.AttachCommandIdCount(id, count);
        }

        public static void Initialize()
        {
            AddCommand(new ForwardMoveCommand(), DefaultGrade1CommandStockCount);
            AddCommand(new LeftMoveCommand(), DefaultGrade1CommandStockCount);
            AddCommand(new RightRightCommand(), DefaultGrade1CommandStockCount);
            AddCommand(new LeftRotationCommand(), DefaultGrade1CommandStockCount);
            AddCommand(new RightRotationCommand(), DefaultGrade1CommandStockCount);
            AddCommand(new LeftForwardMoveCommand(), DefaultGrade2CommandStockCount);
            AddCommand(new RightForwardMoveCommand(), DefaultGrade2CommandStockCount);
            AddCommand(new StackAddCommand(), DefaultGrade2CommandStockCount);
            AddCommand(new GlobalWormAddCommand(), DefaultGrade2CommandStockCount);
            AddCommand(new MalwareInstallerCommand(), DefaultGrade3CommandStockCount);
            AddCommand(new CoinMiningCommand(), DefaultGrade4CommandStockCount);
            AddCommand(new BotnetsCommand(), DefaultGrade5CommandStockCount);
            AddCommand(new SchanzeInstallerCommand(), DefaultGrade1CommandStockCount);
            AddCommand(new PropellerInstallerCommand(), DefaultGrade1CommandStockCount);
            AddCommand(new ForwardAttackCommand(), DefaultGrade1CommandStockCount);
            AddCommand(new BackwardAttackCommand(), DefaultGrade1CommandStockCount);
            AddCommand(new EnergyChargeCommand(), DefaultGrade1CommandStockCount);
            AddCommand(new DashCommand(), DefaultGrade2CommandStockCount);
            AddCommand(new EnergyStorageIncreaseCommand(), DefaultGrade2CommandStockCount);
            AddCommand(new ReinforcementCommand(), DefaultGrade2CommandStockCount);
            AddCommand(new SelfRepairCommand(), DefaultGrade4CommandStockCount);
            AddCommand(new RestorationCommand(), DefaultGrade5CommandStockCount);
        }
    }
}
