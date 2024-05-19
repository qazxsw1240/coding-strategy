namespace CodingStrategy.Entities.Player
{
    using System;
    using UnityEngine.Events;
    using Robot;

    public class PlayerDelegateImpl : IPlayerDelegate
    {
        private int _healthPoint;
        private int _level;
        private int _exp;
        private int _currency;
        private IRobotDelegate _robot;

        public PlayerDelegateImpl(
            string id,
            int healthPoint,
            int level,
            int exp,
            int currency,
            IRobotDelegate robot,
            IAlgorithm algorithm)
        {
            Id = id;
            _healthPoint = healthPoint;
            _level = level;
            _exp = exp;
            _currency = currency;
            Robot = robot;
            Algorithm = algorithm;
            OnHealthPointChange = new UnityEvent<int, int>();
            OnLevelChange = new UnityEvent<int, int>();
            OnExpChange = new UnityEvent<int, int>();
            OnCurrencyChange = new UnityEvent<int, int>();
        }

        public string Id { get; }

        public int HealthPoint
        {
            get => _healthPoint;
            set
            {
                int previousHealthPoint = _healthPoint;
                _healthPoint = value;
                OnHealthPointChange.Invoke(previousHealthPoint, _healthPoint);
            }
        }

        public int Level
        {
            get => _level;
            set
            {
                int previousLevel = _level;
                _level = value;
                OnLevelChange.Invoke(previousLevel, _level);
            }
        }

        public int Exp
        {
            get => _exp;
            set
            {
                int previousExp = _exp;
                _exp = value;
                OnExpChange.Invoke(previousExp, _exp);
            }
        }

        public int Currency
        {
            get => _currency;
            set
            {
                int previousCurrency = _currency;
                _currency = value;
                OnCurrencyChange.Invoke(previousCurrency, _currency);
            }
        }

        public IRobotDelegate Robot
        {
            get => _robot ?? throw new NullReferenceException("Robot not attached.");
            set => _robot = value;
        }

        public IAlgorithm Algorithm { get; }

        public UnityEvent<int, int> OnHealthPointChange { get; }

        public UnityEvent<int, int> OnLevelChange { get; }

        public UnityEvent<int, int> OnExpChange { get; }

        public UnityEvent<int, int> OnCurrencyChange { get; }

        public int CompareTo(IGameEntity other)
        {
            return string.Compare(Id, other.Id, StringComparison.Ordinal);
        }
    }
}
