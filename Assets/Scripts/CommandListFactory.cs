using System.Collections.Generic;

using CodingStrategy.Entities;
using CodingStrategy.Entities.Runtime.Command;

namespace CodingStrategy
{
    public static class CommandListFactory
    {
        public static IEnumerable<ICommand> Commands => new List<ICommand> {
            new ForwardMoveCommand(),
            new LeftMoveCommand(),
            new RightRightCommand(),
            new LeftRotationCommand(),
            new RightRotationCommand(),
            new LeftForwardMoveCommand(),
            new RightForwardMoveCommand(),
            new StackAddCommand(),
            new GlobalWormAddCommand(),
            new MalwareInstallerCommand(),
            new CoinMiningCommand(),
            new BotnetsCommand(),
            new SchanzeInstallerCommand(),
            new PropellerInstallerCommand(),
            new ForwardAttackCommand(),
            new BackwardAttackCommand(),
            new EnergyChargeCommand(),
            new DashCommand(),
            new EnergyStorageIncreaseCommand(),
            new ReinforcementCommand(),
            new SelfRepairCommand(),
            new RestorationCommand()
        };
    }
}
