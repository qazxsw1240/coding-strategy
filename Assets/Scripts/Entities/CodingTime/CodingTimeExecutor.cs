#nullable enable


using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private static readonly IRerollProbability RerollProbability = new RerollProbabilityImpl();

        public int countdown = 40;

        private int _current = 0;

        public InGameUI InGameUI { get; set; } = null!;
        public IPlayerPool PlayerPool { get; set; } = null!;
        public IPlayerCommandNetworkDelegate NetworkDelegate { get; set; } = null!;
        public IPlayerCommandCache CommandCache { get; set; } = null!;

        private IList<ICommand> _commands = new List<ICommand>();

        public void Awake()
        {
            LifeCycle = this;
        }

        public UnityEvent<int, int> OnCountdownChange { get; } = new UnityEvent<int, int>();

        public void Initialize()
        {
            _current = countdown;
            InGameUI.shopUi.OnBuyCommandEvent.AddListener(BuyCommandListener);
            InGameUI.shopUi.OnSellCommandEvent.AddListener(SellCommandListener);
            InGameUI.shopUi.OnChangeCommandEvent.AddListener(SwapCommandListener);
            InGameUI.shopUi.OnShopRerollEvent.AddListener(RerollCommands);
            // InGameUI.shopUi.OnShopLevelUpEvent

            IPlayerDelegate playerDelegate = PlayerPool[PhotonNetwork.LocalPlayer.UserId];
            IAlgorithm algorithm = playerDelegate.Algorithm;

            algorithm.Capacity = 4;

            if (algorithm.Count == 0)
            {
                for (int i = 0; i < algorithm.Capacity; i++)
                {
                    algorithm[i] = new EmptyCommand();
                }
            }
            else
            {
                for (int i = 0; i < algorithm.Capacity; i++)
                {
                    if (algorithm[i] == null)
                    {
                        algorithm[i] = new EmptyCommand();
                    }
                }
            }

            RerollCommands();
            UpdatePlayerAlgorithm();
        }

        public bool MoveNext()
        {
            return _current >= 0;
        }

        public bool Execute()
        {
            _current -= 1;
            return true;
        }

        public void Terminate()
        {
            InGameUI.shopUi.OnBuyCommandEvent.RemoveAllListeners();
            InGameUI.shopUi.OnSellCommandEvent.RemoveAllListeners();
            InGameUI.shopUi.OnChangeCommandEvent.RemoveAllListeners();
            InGameUI.shopUi.OnShopRerollEvent.RemoveAllListeners();
            InGameUI.shopUi.OnShopLevelUpEvent.RemoveAllListeners();

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
            yield return null;
        }

        private void RerollCommands()
        {
            if (_commands.Count != 0)
            {
                foreach (ICommand command in _commands)
                {
                    CommandCache.Sell(command);
                }
            }

            System.Random random = new System.Random();
            IPlayerDelegate playerDelegate = PlayerPool[PhotonNetwork.LocalPlayer.UserId];
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
            IPlayerDelegate playerDelegate = PlayerPool[PhotonNetwork.LocalPlayer.UserId];
            IAlgorithm algorithm = playerDelegate.Algorithm;
            InGameUI.shopUi.SetMyCommandList(algorithm.ToArray());
        }

        private void BuyCommandListener(int shopIndex, int algorithmIndex)
        {
            Debug.LogFormat("Buy event {0} to {1}", shopIndex, algorithmIndex);
            IPlayerDelegate playerDelegate = PlayerPool[PhotonNetwork.LocalPlayer.UserId];
            ICommand command = _commands[shopIndex];
            _commands.RemoveAt(shopIndex);
            playerDelegate.Algorithm[algorithmIndex] = command;
            Debug.Log(BuildAlgorithmCommandsMessage());
            UpdateShopCommandList();
            UpdatePlayerAlgorithm();
        }

        private void SellCommandListener(int algorithmIndex)
        {
            Debug.LogFormat("Sell event: {0}", algorithmIndex);
            IPlayerDelegate playerDelegate = PlayerPool[PhotonNetwork.LocalPlayer.UserId];
            IAlgorithm algorithm = playerDelegate.Algorithm;
            ICommand command = algorithm[algorithmIndex];
            if (command.Id == "0")
            {
                return;
            }
            algorithm[algorithmIndex] = new EmptyCommand();
            // UpdateShopCommandList();
            Debug.Log(BuildAlgorithmCommandsMessage());
            UpdatePlayerAlgorithm();
        }

        private void SwapCommandListener(int x, int y)
        {
            Debug.LogFormat("Swap event: {0}-{1}", x, y);
            IPlayerDelegate playerDelegate = PlayerPool[PhotonNetwork.LocalPlayer.UserId];
            IAlgorithm algorithm = playerDelegate.Algorithm;
            (algorithm[x], algorithm[y]) = (algorithm[y], algorithm[x]);
            UpdatePlayerAlgorithm();
        }

        private string BuildAlgorithmCommandsMessage()
        {
            IPlayerDelegate playerDelegate = PlayerPool[PhotonNetwork.LocalPlayer.UserId];
            IAlgorithm algorithm = playerDelegate.Algorithm;
            IEnumerator<ICommand> enumerator = algorithm.GetEnumerator();
            // enumerator.Reset();
            StringBuilder builder = new StringBuilder('[');
            while (enumerator.MoveNext())
            {
                ICommand current = enumerator.Current!;
                builder.Append($"{current.Info.Name}, ");
            }

            enumerator.Dispose();
            builder.Append(']');
            return builder.ToString();
        }
    }
}
