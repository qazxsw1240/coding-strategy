#nullable enable


namespace CodingStrategy.Entities.Robot
{
    using UnityEngine.Events;

    public class RobotDelegateImpl : IRobotDelegate
    {
        private readonly string _id;
        private readonly IAlgorithm _algorithm;

        private RobotDirection _direction;
        private Coordinate _position;
        private int _healthPoint;
        private int _energyPoint;
        private int _armorPoint;
        private int _attackPoint;

        private readonly UnityEvent<Coordinate> _onPositionChanged;

        public RobotDelegateImpl(
            string id,
            IAlgorithm algorithm,
            RobotDirection direction,
            Coordinate position,
            int healthPoint,
            int energyPoint,
            int armorPoint,
            int attackPoint)
        {
            _id = id;
            _algorithm = algorithm;
            _direction = direction;
            _position = position;
            _healthPoint = healthPoint;
            _energyPoint = energyPoint;
            _armorPoint = armorPoint;
            _attackPoint = attackPoint;
            _onPositionChanged = new UnityEvent<Coordinate>();
        }

        public string Id => _id;

        public IAlgorithm Algorithm => _algorithm;

        public Coordinate Position => _position;

        public RobotDirection Direction
        {
            get => _direction;
            set => _direction = value;
        }

        public int HealthPoint
        {
            get => _healthPoint;
            set => _healthPoint = value;
        }

        public int EnergyPoint
        {
            get => _energyPoint;
            set => _energyPoint = value;
        }

        public int ArmorPoint
        {
            get => _armorPoint;
            set => _armorPoint = value;
        }

        public int AttackPoint
        {
            get => _attackPoint;
            set => _attackPoint = value;
        }

        public UnityEvent<Coordinate> OnPositionChanged => _onPositionChanged;

        public bool Move(int count)
        {
            // TODO
            return true;
        }

        public bool Move(Coordinate destination)
        {
            // TODO
            return true;
        }

        public bool Rotate(int count)
        {
            // TODO
            return true;
        }
    }
}
