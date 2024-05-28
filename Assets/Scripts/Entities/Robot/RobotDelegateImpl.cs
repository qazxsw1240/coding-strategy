#nullable enable


using System.Linq;

namespace CodingStrategy.Entities.Robot
{
    using System;
    using Board;
    using Runtime;
    using UnityEngine.Events;

    public class RobotDelegateImpl : IRobotDelegate
    {
        private static readonly Coordinate[] _vectors = new Coordinate[]
        {
            new Coordinate(0, 1),
            new Coordinate(1, 0),
            new Coordinate(0, -1),
            new Coordinate(-1, 0)
        };

        private readonly string _id;
        private readonly IBoardDelegate _boardDelegate;
        private readonly IAlgorithm _algorithm;

        private int _healthPoint;
        private int _maxHealthPoint;
        private int _energyPoint;
        private int _maxEnergyPoint;
        private int _armorPoint;
        private int _maxArmorPoint;
        private int _attackPoint;
        private int _maxAttackPoint;

        private readonly UnityEvent<IRobotDelegate, Coordinate, Coordinate> _robotChangePositionEvents;
        private readonly UnityEvent<IRobotDelegate, RobotDirection, RobotDirection> _robotChangeDirectionEvents;
        private readonly UnityEvent<IRobotDelegate, int, int> _healthPointChangeEvents;
        private readonly UnityEvent<IRobotDelegate, int, int> _maxHealthPointChangeEvents;
        private readonly UnityEvent<IRobotDelegate, int, int> _energyPointChangeEvents;
        private readonly UnityEvent<IRobotDelegate, int, int> _maxEnergyPointChangeEvents;
        private readonly UnityEvent<IRobotDelegate, int, int> _armorPointChangeEvents;
        private readonly UnityEvent<IRobotDelegate, int, int> _maxArmorPointChangeEvents;
        private readonly UnityEvent<IRobotDelegate, int, int> _attackPointChangeEvents;
        private readonly UnityEvent<IRobotDelegate, int, int> _maxAttackPointChangeEvents;

        public RobotDelegateImpl(
            string id,
            IBoardDelegate boardDelegate,
            IAlgorithm algorithm,
            int healthPoint,
            int energyPoint,
            int armorPoint,
            int attackPoint,
            int maxHealthPoint = 5,
            int maxEnergyPoint = 10,
            int maxArmorPoint = 5,
            int maxAttackPoint = 5)
        {
            _id = id;
            _boardDelegate = boardDelegate;
            _algorithm = algorithm;
            _healthPoint = healthPoint;
            _maxHealthPoint = maxHealthPoint;
            _energyPoint = energyPoint;
            _maxEnergyPoint = maxEnergyPoint;
            _armorPoint = armorPoint;
            _maxArmorPoint = maxArmorPoint;
            _attackPoint = attackPoint;
            _maxAttackPoint = maxAttackPoint;
            _robotChangePositionEvents = new UnityEvent<IRobotDelegate, Coordinate, Coordinate>();
            _robotChangeDirectionEvents = new UnityEvent<IRobotDelegate, RobotDirection, RobotDirection>();
            _healthPointChangeEvents = new UnityEvent<IRobotDelegate, int, int>();
            _maxHealthPointChangeEvents = new UnityEvent<IRobotDelegate, int, int>();
            _energyPointChangeEvents = new UnityEvent<IRobotDelegate, int, int>();
            _maxEnergyPointChangeEvents = new UnityEvent<IRobotDelegate, int, int>();
            _armorPointChangeEvents = new UnityEvent<IRobotDelegate, int, int>();
            _maxArmorPointChangeEvents = new UnityEvent<IRobotDelegate, int, int>();
            _attackPointChangeEvents = new UnityEvent<IRobotDelegate, int, int>();
            _maxAttackPointChangeEvents = new UnityEvent<IRobotDelegate, int, int>();

            _boardDelegate.OnRobotChangePosition.AddListener(InvokeRobotChangePositionEvents);
            _boardDelegate.OnRobotChangeDirection.AddListener(InvokeRobotChangeDirectionEvents);
        }
        public Coordinate[] Vectors => _vectors;

        public string Id => _id;

        public IAlgorithm Algorithm => _algorithm;

        public Coordinate Position
        {
            get => _boardDelegate.GetPosition(this);
            set => _boardDelegate.Place(this, value);
        }

        public RobotDirection Direction
        {
            get => _boardDelegate.GetDirection(this);
            set => _boardDelegate.Rotate(this, value);
        }

        public int HealthPoint
        {
            get => _healthPoint;
            set
            {
                int previousHealthPoint = _healthPoint;
                _healthPoint = value;
                if(_healthPoint > _maxHealthPoint)
                {
                    _healthPoint = _maxHealthPoint;
                }
                _healthPointChangeEvents.Invoke(this, previousHealthPoint, _healthPoint);
            }
        }

        public int MaxHealthPoint
        {
            get => _maxHealthPoint;
            set
            {
                int previousMaxHealthPoint = _maxHealthPoint;
                _maxHealthPoint = value;
                _maxHealthPointChangeEvents.Invoke(this, previousMaxHealthPoint, _maxHealthPoint);
            }
        }

        public int EnergyPoint
        {
            get => _energyPoint;
            set
            {
                int previousEnergyPoint = _energyPoint;
                _energyPoint = value;
                if(_energyPoint > _maxEnergyPoint)
                {
                    _energyPoint = _maxEnergyPoint;
                }
                _energyPointChangeEvents.Invoke(this, previousEnergyPoint, _energyPoint);
            }
        }

        public int MaxEnergyPoint
        {
            get => _maxEnergyPoint;
            set
            {
                int previousMaxEnergyPoint = _maxEnergyPoint;
                _maxEnergyPoint = value;
                _maxEnergyPointChangeEvents.Invoke(this, previousMaxEnergyPoint, _maxEnergyPoint);
            }
        }

        public int ArmorPoint
        {
            get => _armorPoint;
            set
            {
                int previousArmorPoint = _armorPoint;
                _armorPoint = value;
                if(_armorPoint > _maxArmorPoint)
                {
                    _armorPoint = _maxArmorPoint;
                }
                _armorPointChangeEvents.Invoke(this, previousArmorPoint, _armorPoint);
            }
        }

        public int MaxArmorPoint
        {
            get => _maxArmorPoint;
            set
            {
                int previousMaxArmorPoint = _maxArmorPoint;
                _maxArmorPoint = value;
                _maxArmorPointChangeEvents.Invoke(this, previousMaxArmorPoint, _maxArmorPoint);
            }
        }

        public int AttackPoint
        {
            get => _attackPoint;
            set
            {
                int previousAttackPoint = _attackPoint;
                _attackPoint = value;
                if(_attackPoint > _maxAttackPoint)
                {
                    _attackPoint = _maxAttackPoint;
                }
                _attackPointChangeEvents.Invoke(this, previousAttackPoint, _attackPoint);
            }
        }

        public int MaxAttackPoint
        {
            get => _maxAttackPoint;
            set
            {
                int previousMaxAttackPoint = _maxAttackPoint;
                _maxAttackPoint = value;
                _maxAttackPointChangeEvents.Invoke(this, previousMaxAttackPoint, _maxAttackPoint);
            }
        }
        
        public bool Move(int count)
        {
            Coordinate vector = Vectors[(int) Direction];
            Coordinate destination = Position + (vector * count);
            return Move(destination);
        }

        public bool Move(Coordinate position) => _boardDelegate.Place(this, position);

        public bool Rotate(int count)
        {
            int range = Enum.GetValues(typeof(RobotDirection)).Length;
            RobotDirection direction = Direction;
            RobotDirection nextDirection = (RobotDirection) (((int) direction + count + range) % range);
            return Rotate(nextDirection);
        }

        public bool Rotate(RobotDirection direction) => _boardDelegate.Rotate(this, direction);

        public bool Attack(IRobotAttackStrategy strategy, params Coordinate[] relativePositions)
        {
            Coordinate currentPosition = Position;
            int checksum = 0;
            foreach (Coordinate relativePosition in relativePositions)
            {
                Coordinate targetPosition = currentPosition + relativePosition;
                ICellDelegate cellDelegate = _boardDelegate[targetPosition];
                foreach (IRobotDelegate robotDelegate in cellDelegate.Placeables.Where(p => p is IRobotDelegate).Cast<IRobotDelegate>())
                {
                    robotDelegate.HealthPoint -= strategy.CalculateAttackPoint(this, robotDelegate);
                    checksum++;
                }
            }

            return checksum != 0;
        }

        public int CompareTo(IGameEntity other) => _id.CompareTo(other);

        public UnityEvent<IRobotDelegate, Coordinate, Coordinate> OnRobotChangePosition => _robotChangePositionEvents;

        public UnityEvent<IRobotDelegate, RobotDirection, RobotDirection> OnRobotChangeDirection =>
            _robotChangeDirectionEvents;

        public UnityEvent<IRobotDelegate, int, int> OnHealthPointChange => _healthPointChangeEvents;

        public UnityEvent<IRobotDelegate, int, int> OnMaxHealthPointChange => _maxHealthPointChangeEvents;

        public UnityEvent<IRobotDelegate, int, int> OnEnergyPointChange => _energyPointChangeEvents;

        public UnityEvent<IRobotDelegate, int, int> OnMaxEnergyPointChange => _maxEnergyPointChangeEvents;

        public UnityEvent<IRobotDelegate, int, int> OnArmorPointChange => _armorPointChangeEvents;

        public UnityEvent<IRobotDelegate, int, int> OnMaxArmorPointChange => _maxArmorPointChangeEvents;

        public UnityEvent<IRobotDelegate, int, int> OnAttackPointChange => _attackPointChangeEvents;

        public UnityEvent<IRobotDelegate, int, int> OnMaxAttackPointChange => _maxArmorPointChangeEvents;

        private void InvokeRobotChangePositionEvents(IRobotDelegate robotDelegate, Coordinate previousPosition,
            Coordinate newPosition)
        {
            if (robotDelegate == this)
            {
                _robotChangePositionEvents.Invoke(this, previousPosition, newPosition);
            }
        }

        private void InvokeRobotChangeDirectionEvents(IRobotDelegate robotDelegate, RobotDirection previousDirection,
            RobotDirection newDirection)
        {
            if (robotDelegate == this)
            {
                _robotChangeDirectionEvents.Invoke(this, previousDirection, newDirection);
            }
        }
    }
}
