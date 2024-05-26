#nullable enable


using System;
using CodingStrategy.Entities.Player;
using CodingStrategy.Entities.Robot;
using CodingStrategy.UI.InGame;
using ExitGames.Client.Photon;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using Photon.Realtime;
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


        public GameManager GameManager { get; set; } = null!;

        private static int GetValidPlayerHp(int playerHp)
        {
            return Math.Max(0, Math.Min(5, playerHp));
        }

        private static int GetValidRobotHp(int robotHp)
        {
            return Math.Max(0, Math.Min(5, robotHp));
        }

        public void Start()
        {
            foreach (IPlayerDelegate playerDelegate in GameManager.PlayerPool)
            {
                PlayerStatusUI? playerStatusUI = GameManager.FindPlayerStatusUI(playerDelegate);

                if (playerStatusUI == null)
                {
                    continue;
                }

                AttachPlayerStatusSynchronizer(playerDelegate, playerStatusUI);
            }
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            IPlayerDelegate playerDelegate = GameManager.PlayerPool[targetPlayer.UserId];

            if (playerDelegate.Id == targetPlayer.UserId)
            {
                return;
            }

            PlayerStatusUI? playerStatusUI = GameManager.FindPlayerStatusUI(playerDelegate);

            if (playerStatusUI == null)
            {
                return;
            }

            // private const string CurrencyKey = "currency";
            // private const string PlayerHpKey = "playerHP";
            // private const string ExpKey = "exp";
            // private const string LevelKey = "level";
            // private const string RobotHpKey = "robotHP";

            if (changedProps.TryGetValue(CurrencyKey, out int currency))
            {
                playerStatusUI.SetMoney(currency);
            }

            if (changedProps.TryGetValue(PlayerHpKey, out int healthPoint))
            {
                playerStatusUI.SetPlayerHP(healthPoint);
            }

            if (changedProps.TryGetValue(RobotHpKey, out int robotHealthPoint))
            {
                playerStatusUI.SetRobotHP(robotHealthPoint);
            }
        }

        private void AttachPlayerStatusSynchronizer(IPlayerDelegate playerDelegate, PlayerStatusUI statusUI)
        {
            IRobotDelegate robotDelegate = GameManager.RobotDelegatePool[playerDelegate.Id];

            playerDelegate.OnCurrencyChange.AddListener(GetPlayerCurrencyUpdater(playerDelegate, statusUI));
            playerDelegate.OnExpChange.AddListener(GetPlayerExpUpdater(playerDelegate));
            playerDelegate.OnLevelChange.AddListener(GetPlayerLevelUpdater(playerDelegate));
            playerDelegate.OnHealthPointChange.AddListener(GetPlayerHealthPointUpdater(playerDelegate, statusUI));

            robotDelegate.OnHealthPointChange.AddListener(GetRobotHealthPointUpdater(statusUI));
        }

        private UnityAction<int, int> GetPlayerCurrencyUpdater(
            IPlayerDelegate playerDelegate,
            PlayerStatusUI playerStatusUI)
        {
            Player currentPhotonPlayer = PhotonNetwork.LocalPlayer;
            string currentPlayerId = currentPhotonPlayer.UserId;
            return (_, next) =>
            {
                if (playerDelegate.Id != currentPlayerId)
                {
                    return;
                }

                playerStatusUI.SetMoney(next);
                currentPhotonPlayer.SetCustomProperties(new Hashtable
                {
                    { CurrencyKey, next }
                });
            };
        }

        private UnityAction<int, int> GetPlayerHealthPointUpdater(
            IPlayerDelegate playerDelegate,
            PlayerStatusUI playerStatusUI)
        {
            Player currentPhotonPlayer = PhotonNetwork.LocalPlayer;
            string currentPlayerId = currentPhotonPlayer.UserId;
            return (_, next) =>
            {
                if (playerDelegate.Id != currentPlayerId)
                {
                    return;
                }

                playerStatusUI.SetPlayerHP(next);
                currentPhotonPlayer.SetCustomProperties(new Hashtable
                {
                    { PlayerHpKey, next }
                });
            };
        }


        private UnityAction<int, int> GetPlayerExpUpdater(IPlayerDelegate playerDelegate)
        {
            Player currentPhotonPlayer = PhotonNetwork.LocalPlayer;
            string currentPlayerId = currentPhotonPlayer.UserId;
            return (_, next) =>
            {
                if (playerDelegate.Id != currentPlayerId)
                {
                    return;
                }

                currentPhotonPlayer.SetCustomProperties(new Hashtable
                {
                    { ExpKey, next }
                });
            };
        }


        private UnityAction<int, int> GetPlayerLevelUpdater(IPlayerDelegate playerDelegate)
        {
            Player currentPhotonPlayer = PhotonNetwork.LocalPlayer;
            string currentPlayerId = currentPhotonPlayer.UserId;
            return (_, next) =>
            {
                if (playerDelegate.Id != currentPlayerId)
                {
                    return;
                }

                currentPhotonPlayer.SetCustomProperties(new Hashtable
                {
                    { LevelKey, next }
                });
            };
        }

        private UnityAction<IRobotDelegate, int, int> GetRobotHealthPointUpdater(PlayerStatusUI playerStatusUI)
        {
            Player currentPhotonPlayer = PhotonNetwork.LocalPlayer;
            return (robotDelegate, _, next) =>
            {
                if (robotDelegate.Id != currentPhotonPlayer.UserId)
                {
                    return;
                }

                playerStatusUI.SetRobotHP(next);
                currentPhotonPlayer.SetCustomProperties(new Hashtable
                {
                    { RobotHpKey, next }
                });
            };
        }
    }
}
