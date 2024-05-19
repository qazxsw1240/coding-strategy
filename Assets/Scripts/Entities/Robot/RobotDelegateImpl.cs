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
        private static readonly Coordinate[] Vectors = new Coordinate[]
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
        private int _energyPoint;
        private int _armorPoint;
        private int _attackPoint;

        private readonly UnityEvent<IRobotDelegate, Coordinate, Coordinate> _robotChangePositionEvents;
        private readonly UnityEvent<IRobotDelegate, RobotDirection, RobotDirection> _robotChangeDirectionEvents;
        private readonly UnityEvent<IRobotDelegate, int, int> _healthPointChangeEvents;
        private readonly UnityEvent<IRobotDelegate, int, int> _energyPointChangeEvents;
        private readonly UnityEvent<IRobotDelegate, int, int> _armorPointChangeEvents;
        private readonly UnityEvent<IRobotDelegate, int, int> _attackPointChangeEvents;

        public RobotDelegateImpl(
            string id,
            IBoardDelegate boardDelegate,
            IAlgorithm algorithm,
            int healthPoint,
            int energyPoint,
            int armorPoint,
            int attackPoint)
        {
            _id = id;
            _boardDelegate = boardDelegate;
            _algorithm = algorithm;
            _healthPoint = healthPoint;
            _energyPoint = energyPoint;
            _armorPoint = armorPoint;
            _attackPoint = attackPoint;
            _robotChangePositionEvents = new UnityEvent<IRobotDelegate, Coordinate, Coordinate>();
            _robotChangeDirectionEvents = new UnityEvent<IRobotDelegate, RobotDirection, RobotDirection>();
            _healthPointChangeEvents = new UnityEvent<IRobotDelegate, int, int>();
            _energyPointChangeEvents = new UnityEvent<IRobotDelegate, int, int>();
            _armorPointChangeEvents = new UnityEvent<IRobotDelegate, int, int>();
            _attackPointChangeEvents = new UnityEvent<IRobotDelegate, int, int>();

            _boardDelegate.OnRobotChangePosition.AddListener(InvokeRobotChangePositionEvents);
            _boardDelegate.OnRobotChangeDirection.AddListener(InvokeRobotChangeDirectionEvents);
        }

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
                _healthPointChangeEvents.Invoke(this, previousHealthPoint, _healthPoint);
            }
        }

        public int EnergyPoint
        {
            get => _energyPoint;
            set
            {
                int previousEnergyPoint = _energyPoint;
                _energyPoint = value;
                _energyPointChangeEvents.Invoke(this, previousEnergyPoint, _energyPoint);
            }
        }

        public int ArmorPoint
        {
            get => _armorPoint;
            set
            {
                int previousArmorPoint = _armorPoint;
                _armorPoint = value;
                _armorPointChangeEvents.Invoke(this, previousArmorPoint, _armorPoint);
            }
        }

        public int AttackPoint
        {
            get => _attackPoint;
            set
            {
                int previousAttackPoint = _attackPoint;
                _attackPoint = value;
                _attackPointChangeEvents.Invoke(this, previousAttackPoint, _attackPoint);
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

        public UnityEvent<IRobotDelegate, int, int> OnEnergyPointChange => _energyPointChangeEvents;

        public UnityEvent<IRobotDelegate, int, int> OnArmorPointChange => _armorPointChangeEvents;

        public UnityEvent<IRobotDelegate, int, int> OnAttackPointChange => _attackPointChangeEvents;

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
