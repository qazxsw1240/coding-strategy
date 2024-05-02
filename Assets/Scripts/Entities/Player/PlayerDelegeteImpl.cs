namespace CodingStrategy.Entities.Player
{
    using CodingStrategy.Entities.Robot;
    public class PlayerDelegeteImpl : IPlayerDelegate
    {
        private readonly string _id;
        private int _healthPoint;
        private int _level;
        private int _exp;
        private int _currency;
        private IRobotDelegate _robot;
        private IAlgorithm _algorithm;
        public PlayerDelegeteImpl(
            string id,
            int healthPoint,
            int level,
            int exp,
            int currency,
            IRobotDelegate robot,
            IAlgorithm algorithm)
        {
            _id = id;
            _healthPoint = healthPoint;
            _level = level;
            _exp = exp;
            _currency = currency;
            _robot = robot;
            _algorithm = algorithm;
        }

        public string Id => _id;

        public int HealthPoint
        {
            get => _healthPoint;
            set => _healthPoint=value;
        }
        public int Level
        {
            get => _level;
            set => _level=value;
        }
        public int Exp
        {
            get => _exp;
            set => _exp=value;
        }
        public int Currency
        {
            get => _currency;
            set => _currency=value;
        }
        public IRobotDelegate Robot => _robot;
        public IAlgorithm Algorithm => _algorithm;
    }
}