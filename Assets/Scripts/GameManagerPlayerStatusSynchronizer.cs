#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

using CodingStrategy.Entities;
using CodingStrategy.Entities.CodingTime;
using CodingStrategy.Entities.Player;
using CodingStrategy.Entities.Robot;
using CodingStrategy.Network;
using CodingStrategy.UI.GameScene;

using Photon.Pun;
using Photon.Realtime;

using UnityEngine;
using UnityEngine.Events;

namespace CodingStrategy
{
    public class GameManagerPlayerStatusSynchronizer : MonoBehaviourPunCallbacks
    {
        private const string CurrencyKey = "currency";
        private const string PlayerHpKey = "playerHP";
        private const string ExpKey = "exp";
        private const string LevelKey = "level";
        private const string RobotHpKey = "robotHP";

        private static readonly IRequiredExp RequiredExp = new RequiredExpImpl();

        public GameManager GameManager { get; set; } = null!;

        public GameManagerUtil GameManagerUtil { get; set; } = null!;

        public GameMangerNetworkProcessor NetworkProcessor { get; set; } = null!;

        public void Start()
        {
            GameManagerUtil = GameManager.util;
            NetworkProcessor = GameManager.networkProcessor;

            foreach (IPlayerDelegate playerDelegate in GameManager.util.PlayerDelegatePool)
            {
                PlayerStatusUI? playerStatusUI = GameManager.FindPlayerStatusUI(playerDelegate);

                if (playerStatusUI == null)
                {
                    continue;
                }

                AttachPlayerStatusSynchronizer(playerDelegate, playerStatusUI);
                playerStatusUI.OnPlayerUIClickEvent.AddListener(
                    () =>
                    {
                        Player photonPlayer = PhotonNetwork.CurrentRoom.Players
                           .Select(pair => pair.Value)
                           .First(player => player.UserId == playerDelegate.ID);
                        IRobotDelegate robotDelegate = playerDelegate.Robot;
                        RobotStatusUI robotStatusUI = GameManager.inGameUI.statusUI.GetComponent<RobotStatusUI>();
                        robotStatusUI.SetCameraTexture(GameManager.PlayerIndexMap[photonPlayer.UserId]);
                        robotStatusUI.SetName(photonPlayer.NickName);
                        robotStatusUI.SetDescription(
                            robotDelegate.HealthPoint,
                            robotDelegate.AttackPoint,
                            robotDelegate.ArmorPoint,
                            robotDelegate.EnergyPoint);
                        robotStatusUI.SetState(
                            string.Join(
                                ",  ",
                                GameManager.GetAbnormalities()
                                   .Where(pair => pair.Key.StartsWith(playerDelegate.ID))
                                   .Select(pair => pair.Value)
                                   .Select(abnormality => abnormality.Name)));
                        robotStatusUI.SetCommandList(playerDelegate.Algorithm.AsArray());
                    });
            }

            foreach (PlayerStatusUI playerStatusUI in GameManager.inGameUI.playerStatusUI)
            {
                if (string.IsNullOrEmpty(playerStatusUI.GetUserID()))
                {
                    playerStatusUI.SetVisible(false);
                }
            }

            IPlayerDelegate localPlayerDelegate = GameManagerUtil.LocalPhotonPlayerDelegate;
            IRobotDelegate localRobotDelegate = GameManagerUtil.LocalPhotonRobotDelegate;

            CommandDetailEvent commandDetailEvent =
                GameManager.inGameUI.transform.GetComponentInChildren<CommandDetailEvent>();
            commandDetailEvent.OnCommandClickEvent.AddListener(
                id =>
                {
                    ICommand command = PhotonPlayerCommandCache.GetCachedCommands()[id];
                    commandDetailEvent.setCommandDetail.SetCommandName(command.Info.Name);
                    commandDetailEvent.setCommandDetail.SetCommandAttackRange(command.Info.EnhancedLevel);
                    commandDetailEvent.setCommandDetail.SetCommandDescription(command.Info.Explanation);
                });
        }

        private static int GetValidPlayerHp(int playerHp)
        {
            return Math.Max(0, Math.Min(5, playerHp));
        }

        private static int GetValidRobotHp(int robotHp)
        {
            return Math.Max(0, Math.Min(5, robotHp));
        }

        private void AttachPlayerStatusSynchronizer(IPlayerDelegate playerDelegate, PlayerStatusUI statusUI)
        {
            IRobotDelegate robotDelegate = GameManager.RobotDelegatePool[playerDelegate.ID];

            playerDelegate.OnCurrencyChange.AddListener(GetPlayerCurrencyUpdater(playerDelegate, statusUI));
            playerDelegate.OnExpChange.AddListener(GetPlayerExpUpdater(playerDelegate, statusUI));
            playerDelegate.OnLevelChange.AddListener(GetPlayerLevelUpdater(playerDelegate));
            playerDelegate.OnHealthPointChange.AddListener(GetPlayerHealthPointUpdater(playerDelegate, statusUI));
            robotDelegate.OnHealthPointChange.AddListener(GetRobotHealthPointUpdater(statusUI));
        }

        private UnityAction<int, int> GetPlayerCurrencyUpdater(
            IPlayerDelegate playerDelegate,
            PlayerStatusUI playerStatusUI)
        {
            return (previous, next) =>
            {
                if (playerDelegate.ID != GameManagerUtil.LocalPhotonPlayerDelegate.ID)
                {
                    return;
                }

                Debug.LogFormat("{2} currency change event occurred: {0}->{1}", previous, next, playerDelegate.ID);
                playerStatusUI.SetMoney(next);
                GameManager.inGameUI.shopUi.SetBit(next);
            };
        }

        private UnityAction<int, int> GetPlayerHealthPointUpdater(
            IPlayerDelegate playerDelegate,
            PlayerStatusUI playerStatusUI)
        {
            return (previous, next) =>
            {
                if (playerDelegate.ID != GameManagerUtil.LocalPhotonPlayerDelegate.ID)
                {
                    return;
                }

                Debug.LogFormat("{2} HP change event occurred: {0}->{1}", previous, next, playerDelegate.ID);
                int validPlayerHp = GetValidPlayerHp(next);
                playerStatusUI.SetPlayerHP(validPlayerHp);
            };
        }

        private UnityAction<int, int> GetPlayerExpUpdater(IPlayerDelegate playerDelegate, PlayerStatusUI playerStatusUI)
        {
            return (previous, next) =>
            {
                if (playerDelegate.ID != GameManagerUtil.LocalPhotonPlayerDelegate.ID)
                {
                    return;
                }

                int level = playerDelegate.Level;
                int exp = playerDelegate.Exp;
                int requiredExp = CodingTimeExecutor.RequiredExp[level];

                if (exp >= requiredExp)
                {
                    int nextExp = exp - requiredExp;
                    playerDelegate.Exp = nextExp;
                    playerDelegate.Level += 1;
                    return;
                }

                Debug.LogFormat("{2} exp change event occurred: {0}->{1}", previous, next, playerDelegate.ID);
                GameManager.inGameUI.shopUi.SetExp(next, RequiredExp[playerDelegate.Level]);
            };
        }

        private UnityAction<int, int> GetPlayerLevelUpdater(IPlayerDelegate playerDelegate)
        {
            return (previous, next) =>
            {
                if (playerDelegate.ID != GameManagerUtil.LocalPhotonPlayerDelegate.ID)
                {
                    return;
                }

                Debug.LogFormat("rerender algorithm: {0}", next);

                IAlgorithm algorithm = playerDelegate.Algorithm;
                algorithm.Capacity = next;
                GameManager.inGameUI.shopUi.SetShopLevel(next);
                ICommand[] commands = algorithm.AsArray();
                Debug.Log(string.Join(", ", (IEnumerable<ICommand>) commands));
                Debug.LogFormat("{2} level change event occurred: {0}->{1}", previous, next, playerDelegate.ID);
                GameManager.inGameUI.shopUi.SetMyCommandList(commands);
                NetworkProcessor.NotifyLocalPlayerDelegateAlgorithmChange();
            };
        }

        private UnityAction<IRobotDelegate, int, int> GetRobotHealthPointUpdater(PlayerStatusUI playerStatusUI)
        {
            return (robotDelegate, _, next) =>
            {
                if (GameManagerUtil.PlayerDelegatePool.Contains(robotDelegate.ID))
                {
                    IPlayerDelegate playerDelegate = GameManagerUtil.GetPlayerDelegateById(robotDelegate.ID);
                    if (playerDelegate.ID != GameManagerUtil.LocalPhotonPlayerDelegate.ID)
                    {
                        return;
                    }
                }

                int validRobotHp = GetValidRobotHp(next);
                playerStatusUI.SetRobotHP(validRobotHp);
            };
        }
    }
}
