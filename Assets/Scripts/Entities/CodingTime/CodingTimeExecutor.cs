#nullable enable


using System.Collections.Generic;
using System.Linq;
using CodingStrategy.Entities.Player;
using CodingStrategy.Entities.Runtime.CommandImpl;
using CodingStrategy.Entities.Shop;
using CodingStrategy.Network;
using CodingStrategy.UI.InGame;
using Photon.Pun;
using UnityEngine.Events;

namespace CodingStrategy.Entities.CodingTime
{
    using UnityEngine;
    using System.Collections;

    public class CodingTimeExecutor : LifeCycleMonoBehaviourBase, ILifeCycle
    {
        public static readonly IRerollProbability RerollProbability = new RerollProbabilityImpl();
        public static readonly IRequiredExp RequiredExp = new RequiredExpImpl();

        public int countdown = 10;

        private int _current = 0;

        public GameManagerUtil Util { get; set; } = null!;
        public GameMangerNetworkProcessor NetworkProcessor { get; set; } = null!;
        public InGameUI InGameUI { get; set; } = null!;
        public IPlayerPool PlayerPool { get; set; } = null!;
        public IPlayerCommandNetworkDelegate NetworkDelegate { get; set; } = null!;
        public IPlayerCommandCache CommandCache { get; set; } = null!;

        private IList<ICommand> _commands = new List<ICommand>();

        private InGameSoundManager _soundManager = null!;

        public void Awake()
        {
            LifeCycle = this;
            _soundManager = FindObjectOfType<InGameSoundManager>();
        }

        public UnityEvent<int, int> OnCountdownChange { get; } = new UnityEvent<int, int>();

        public void Initialize()
        {
            _current = countdown;
            InGameUI.shopUi.OnBuyCommandEvent.AddListener(BuyCommandListener);
            InGameUI.shopUi.OnSellCommandEvent.AddListener(SellCommandListener);
            InGameUI.shopUi.OnChangeCommandEvent.AddListener(SwapCommandListener);
            InGameUI.shopUi.OnShopRerollEvent.AddListener(() => RerollCommands(false));
            InGameUI.shopUi.OnShopLevelUpEvent.AddListener(LevelUpListener);

            DispenseLevelGuarantee();
            RerollCommands(true);
            UpdatePlayerAlgorithm();
        }

        private void DispenseLevelGuarantee()
        {
            IPlayerDelegate playerDelegate = Util.LocalPhotonPlayerDelegate;
            int roundBonus = playerDelegate.Currency / 10;
            playerDelegate.Currency += roundBonus + 3;
        }

        public bool MoveNext()
        {
            return _current >= 0;
        }

        public bool Execute()
        {
            _current -= 1;
            _soundManager.CodingTimeCountdown();
            return true;
        }

        public void Terminate()
        {
            InGameUI.shopUi.OnBuyCommandEvent.RemoveAllListeners();
            InGameUI.shopUi.OnSellCommandEvent.RemoveAllListeners();
            InGameUI.shopUi.OnChangeCommandEvent.RemoveAllListeners();
            InGameUI.shopUi.OnShopRerollEvent.RemoveAllListeners();
            InGameUI.shopUi.OnShopLevelUpEvent.RemoveAllListeners();
            InGameUI.shopUi.OnShopLevelUpEvent.RemoveAllListeners();
            InGameUI.shopUi.OnShopRerollEvent.RemoveAllListeners();

            if (_commands.Count == 0)
            {
                return;
            }

            foreach (ICommand command in _commands)
            {
                CommandCache.Sell(command);
            }
        }

        protected override IEnumerator OnAfterInitialization()
        {
            Debug.Log("CodingTimeExecutor Started.");
            yield return new WaitForSeconds(1f);
            InGameUI.GotoShop();
            yield return null;
        }

        protected override IEnumerator OnBeforeExecution()
        {
            if (_current == 1)
            {
                Debug.Log("1 Second Left.");
            }
            else
            {
                Debug.LogFormat("{0} Seconds Left.", _current);
            }

            yield return null;
        }

        protected override IEnumerator OnAfterFailExecution()
        {
            yield return null;
        }

        protected override IEnumerator OnAfterExecution()
        {
            yield return new WaitForSecondsRealtime(1.0f);
            OnCountdownChange.Invoke(_current + 1, _current);
        }

        protected override IEnumerator OnAfterTermination()
        {
            Debug.Log("CodingTimeExecutor Terminated.");
            InGameUI.GotoGame();
            yield return null;
        }

        private void RerollCommands(bool init)
        {
            IPlayerDelegate playerDelegate = Util.LocalPhotonPlayerDelegate;

            if (playerDelegate.Currency < 3)
            {
                Debug.Log("not enough currency");
                return;
            }

            if (!init)
            {
                playerDelegate.Currency -= 3;
            }


            if (_commands.Count != 0)
            {
                foreach (ICommand command in _commands)
                {
                    CommandCache.Sell(command);
                }
            }

            System.Random random = new System.Random();
            int grade = RerollProbability.GetRandomGradeFromLevel(playerDelegate.Level);
            int count = 0;
            IList<ICommand> commands = new List<ICommand>
            {
                new EmptyCommand(),
                new EmptyCommand(),
                new EmptyCommand(),
                new EmptyCommand(),
                new EmptyCommand(),
            };
            foreach ((string _, ICommand value) in PhotonPlayerCommandCache.GetCachedCommands())
            {
                if (count == commands.Count)
                {
                    break;
                }

                int probability = random.Next(0, 100);
                if (probability > 30)
                {
                    commands[count++] = value;
                }
            }

            foreach (ICommand command in commands)
            {
                if (command.Id == "0")
                {
                    continue;
                }

                CommandCache.Buy(command.Id, 1);
            }

            _commands = commands;
            UpdateShopCommandList();
        }

        private void UpdateShopCommandList()
        {
            InGameUI.shopUi.SetShopCommandList(_commands.ToArray());
        }

        private void UpdatePlayerAlgorithm()
        {
            IPlayerDelegate playerDelegate = Util.LocalPhotonPlayerDelegate;
            IAlgorithm algorithm = playerDelegate.Algorithm;
            InGameUI.shopUi.SetMyCommandList(algorithm.AsArray());
        }

        private void BuyCommandListener(int shopIndex, int algorithmIndex)
        {
            Debug.LogFormat("Buy event {0} to {1}", shopIndex, algorithmIndex);
            IPlayerDelegate playerDelegate = Util.LocalPhotonPlayerDelegate;

            if (playerDelegate.Currency < 3)
            {
                Debug.Log("not enough currency");
                return;
            }

            playerDelegate.Currency -= 3;

            ICommand command = _commands[shopIndex];
            _commands.RemoveAt(shopIndex);
            ICommand previousCommand = playerDelegate.Algorithm[algorithmIndex];
            if (previousCommand.Id != "0")
            {
                CommandCache.Sell(previousCommand);
            }

            playerDelegate.Algorithm[algorithmIndex] = command;
            UpdateShopCommandList();
            UpdatePlayerAlgorithm();
            NetworkProcessor.NotifyLocalPlayerDelegateAlgorithmChange();
        }

        private void SellCommandListener(int algorithmIndex)
        {
            Debug.LogFormat("Sell event: {0}", algorithmIndex);
            IPlayerDelegate playerDelegate = Util.LocalPhotonPlayerDelegate;
            IAlgorithm algorithm = playerDelegate.Algorithm;
            ICommand command = algorithm[algorithmIndex];

            if (command.Id == "0")
            {
                //  ignore if the command to sell is emtpy.
                return;
            }

            playerDelegate.Currency += 1;

            algorithm[algorithmIndex] = new EmptyCommand();
            UpdatePlayerAlgorithm();
            NetworkProcessor.NotifyLocalPlayerDelegateAlgorithmChange();
        }

        private void SwapCommandListener(int x, int y)
        {
            Debug.LogFormat("Swap event: {0}-{1}", x, y);
            IPlayerDelegate playerDelegate = PlayerPool[PhotonNetwork.LocalPlayer.UserId];
            IAlgorithm algorithm = playerDelegate.Algorithm;
            (algorithm[x], algorithm[y]) = (algorithm[y], algorithm[x]);
            UpdatePlayerAlgorithm();
            NetworkProcessor.NotifyLocalPlayerDelegateAlgorithmChange();
        }

        private void LevelUpListener()
        {
            IPlayerDelegate playerDelegate = Util.LocalPhotonPlayerDelegate;

            if (playerDelegate.Currency < 4)
            {
                Debug.Log("not enough currency");
                return;
            }

            playerDelegate.Currency -= 4;
            playerDelegate.Exp += 4;
        }
    }
}
