using System;

using CodingStrategy.Entities.Board;
using CodingStrategy.Entities.Player;
using CodingStrategy.Entities.Robot;

using UnityEngine.Events;

namespace CodingStrategy.Entities.Placeable
{
    public class BitDelegateImpl : IBitDelegate
    {
        private readonly IBoardDelegate _boardDelegate;
        private readonly IPlayerPool _playerPool;

        private IRobotDelegate _taker;

        public BitDelegateImpl(IPlayerPool playerPool, IBoardDelegate boardDelegate, int amount)
        {
            _playerPool = playerPool;
            _boardDelegate = boardDelegate;
            Amount = amount;
            OnRobotTakeInEvents = new UnityEvent<IRobotDelegate>();
            OnRobotTakeAwayEvents = new UnityEvent<IRobotDelegate>();

            _boardDelegate.OnRobotChangePosition.AddListener(InvokeRobotTakeBitEvents);
        }

        public Coordinate Position => _boardDelegate.GetPosition(this);

        public int Amount { get; }

        public bool IsTaken => _taker != null;

        public UnityEvent<IRobotDelegate> OnRobotTakeInEvents { get; }

        public UnityEvent<IRobotDelegate> OnRobotTakeAwayEvents { get; }

        private void InvokeRobotTakeBitEvents(IRobotDelegate robotDelegate, Coordinate previous, Coordinate next)
        {
            try
            {
                if (next != Position)
                {
                    return;
                }
            }
            catch (Exception)
            {
                return;
            }
            if (_taker == null)
            {
                _taker = robotDelegate;
                OnRobotTakeInEvents.Invoke(robotDelegate);
                IPlayerDelegate playerDelegate = _playerPool[robotDelegate.ID];
                ProvidePlayerWithCurrency(playerDelegate, false);
            }
            else
            {
                IRobotDelegate previousTaker = _taker;
                _taker = null;
                OnRobotTakeAwayEvents.Invoke(previousTaker);
                IPlayerDelegate playerDelegate = _playerPool[robotDelegate.ID];
                ProvidePlayerWithCurrency(playerDelegate, true);
            }
        }

        private void ProvidePlayerWithCurrency(IPlayerDelegate playerDelegate, bool rollback)
        {
            int amount = rollback ? -Amount : Amount;
            playerDelegate.Currency += amount;
        }
    }
}
