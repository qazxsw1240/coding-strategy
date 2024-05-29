using System;
using System.Linq;
using CodingStrategy.Entities;
using CodingStrategy.Entities.Player;
using CodingStrategy.Entities.Runtime.CommandImpl;
using CodingStrategy.Network;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

namespace CodingStrategy
{
    public class GameMangerNetworkProcessor : MonoBehaviourPunCallbacks
    {
        public const string CurrencyKey = "currency";
        public const string PlayerHpKey = "playerHP";
        public const string ExpKey = "exp";
        public const string LevelKey = "level";
        public const string RobotHpKey = "robotHP";
        public const string AlgorithmUpdateKey = "algorithmUpdateKey";

        public GameManagerUtil GameManagerUtil { get; set; } = null!;

        private GameManagerUtil GetGameManagerUtilAssert()
        {
            return GameManagerUtil ?? throw new NullReferenceException("GameManagerUtil is not yet attached.");
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            GameManagerUtil gameManagerUtil = GetGameManagerUtilAssert();

            if (targetPlayer.UserId == gameManagerUtil.LocalPhotonPlayerDelegate.Id)
            {
                // duplicate event handling
                return;
            }

            IPlayerDelegate targetPlayerDelegate = gameManagerUtil.GetPlayerDelegate(targetPlayer);

            if (changedProps.TryGetValue(CurrencyKey, out object value))
            {
                int currency = (int) value;
                targetPlayerDelegate.Currency = currency;
            }

            if (changedProps.TryGetValue(PlayerHpKey, out value))
            {
                int healthPoint = (int) value;
                targetPlayerDelegate.HealthPoint = healthPoint;
            }

            if (changedProps.TryGetValue(ExpKey, out value))
            {
                int exp = (int) value;
                targetPlayerDelegate.Exp = exp;
            }

            if (changedProps.TryGetValue(LevelKey, out value))
            {
                int level = (int) value;
                targetPlayerDelegate.Level = level;
            }

            if (changedProps.TryGetValue(RobotHpKey, out value))
            {
                int robotHealthPoint = (int) value;
                targetPlayerDelegate.Robot.HealthPoint = robotHealthPoint;
            }

            if (changedProps.TryGetValue(AlgorithmUpdateKey, out value))
            {
                object[] response = (object[]) value;
                string target = (string) response[0];
                string algorithmResponse = (string) response[1];
                ICommand[] commands = ParseCommands(algorithmResponse);
                IPlayerDelegate playerDelegate = gameManagerUtil.GetPlayerDelegateById(target);
                IAlgorithm algorithm = playerDelegate.Algorithm;

                for (int i = 0; i < commands.Length; i++)
                {
                    algorithm[i] = commands[i];
                }
            }
        }

        public void NotifyLocalPlayerDelegateAlgorithmChange()
        {
            IPlayerDelegate playerDelegate = GameManagerUtil.LocalPhotonPlayerDelegate;
            IAlgorithm algorithm = playerDelegate.Algorithm;
            string algorithmRequest = string.Join(',',
                algorithm.Select(command => $"{command.Id}-{command.Info.EnhancedLevel}"));
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable
            {
                { AlgorithmUpdateKey, new object[] { playerDelegate.Id, algorithmRequest } }
            });
        }

        private static ICommand[] ParseCommands(string algorithmResponse)
        {
            string[] tokens = algorithmResponse.Split(",");
            ICommand[] commands = new ICommand[tokens.Length];
            for (int i = 0; i < commands.Length; i++)
            {
                string token = tokens[i];
                string[] args = token.Split('-');
                string id = args[0];
                int enhancedLevel = int.Parse(args[1]);
                if (id == "0")
                {
                    commands[i] = new EmptyCommand();
                    continue;
                }
                ICommand command = PhotonPlayerCommandCache.GetCachedCommands()[id].Copy();
                command.Info.EnhancedLevel = enhancedLevel;
                commands[i] = command;
            }

            return commands;
        }
    }
}
