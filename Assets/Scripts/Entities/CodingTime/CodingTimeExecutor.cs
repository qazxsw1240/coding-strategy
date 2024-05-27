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

            RerollCommands();
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
        }

        private void BuyCommandListener(int shopIndex, int algorithmIndex)
        {
            Debug.LogFormat("{0} to {1}", shopIndex, algorithmIndex);
            IPlayerDelegate playerDelegate = PlayerPool[PhotonNetwork.LocalPlayer.UserId];
            ICommand command = _commands[shopIndex];
            _commands.RemoveAt(shopIndex);
            playerDelegate.Algorithm[algorithmIndex] = command;
            InGameUI.shopUi.SetShopCommandList(_commands.ToArray());
            InGameUI.shopUi.SetMyCommandList(playerDelegate.Algorithm.ToArray());
        }
    }
}
