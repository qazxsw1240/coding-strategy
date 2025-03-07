#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

using CodingStrategy.Entities.Board;
using CodingStrategy.Entities.Runtime;

using UnityEngine.Events;

namespace CodingStrategy.Entities.Robot
{
    public class RobotDelegateImpl : IRobotDelegate
    {
        private static readonly Coordinate[] DirectionVectors =
        {
            new Coordinate(0, 1),
            new Coordinate(1, 0),
            new Coordinate(0, -1),
            new Coordinate(-1, 0)
        };

        private readonly IBoardDelegate _boardDelegate;
        private readonly UnityEvent<IRobotDelegate, int, int> _maxAttackPointChangeEvents;

        private int _armorPoint;
        private int _attackPoint;
        private int _energyPoint;

        private int _healthPoint;
        private int _maxArmorPoint;
        private int _maxAttackPoint;
        private int _maxEnergyPoint;
        private int _maxHealthPoint;

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
            ID = id;
            _boardDelegate = boardDelegate;
            Algorithm = algorithm;
            _healthPoint = healthPoint;
            _maxHealthPoint = maxHealthPoint;
            _energyPoint = energyPoint;
            _maxEnergyPoint = maxEnergyPoint;
            _armorPoint = armorPoint;
            _maxArmorPoint = maxArmorPoint;
            _attackPoint = attackPoint;
            _maxAttackPoint = maxAttackPoint;
            OnRobotChangePosition = new UnityEvent<IRobotDelegate, Coordinate, Coordinate>();
            OnRobotChangeDirection = new UnityEvent<IRobotDelegate, RobotDirection, RobotDirection>();
            OnRobotAttack = new UnityEvent<IRobotDelegate, IList<Coordinate>>();
            OnHealthPointChange = new UnityEvent<IRobotDelegate, int, int>();
            OnMaxHealthPointChange = new UnityEvent<IRobotDelegate, int, int>();
            OnEnergyPointChange = new UnityEvent<IRobotDelegate, int, int>();
            OnMaxEnergyPointChange = new UnityEvent<IRobotDelegate, int, int>();
            OnArmorPointChange = new UnityEvent<IRobotDelegate, int, int>();
            OnMaxArmorPointChange = new UnityEvent<IRobotDelegate, int, int>();
            OnAttackPointChange = new UnityEvent<IRobotDelegate, int, int>();
            _maxAttackPointChangeEvents = new UnityEvent<IRobotDelegate, int, int>();

            _boardDelegate.OnRobotChangePosition.AddListener(InvokeRobotChangePositionEvents);
            _boardDelegate.OnRobotChangeDirection.AddListener(InvokeRobotChangeDirectionEvents);
        }

        public Coordinate[] Vectors => DirectionVectors;

        public string ID { get; }

        public IAlgorithm Algorithm { get; }

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
                if (_healthPoint > _maxHealthPoint)
                {
                    _healthPoint = _maxHealthPoint;
                }
                OnHealthPointChange.Invoke(this, previousHealthPoint, _healthPoint);
            }
        }

        public int MaxHealthPoint
        {
            get => _maxHealthPoint;
            set
            {
                int previousMaxHealthPoint = _maxHealthPoint;
                _maxHealthPoint = value;
                OnMaxHealthPointChange.Invoke(this, previousMaxHealthPoint, _maxHealthPoint);
            }
        }

        public int EnergyPoint
        {
            get => _energyPoint;
            set
            {
                int previousEnergyPoint = _energyPoint;
                _energyPoint = value;
                if (_energyPoint > _maxEnergyPoint)
                {
                    _energyPoint = _maxEnergyPoint;
                }
                OnEnergyPointChange.Invoke(this, previousEnergyPoint, _energyPoint);
            }
        }

        public int MaxEnergyPoint
        {
            get => _maxEnergyPoint;
            set
            {
                int previousMaxEnergyPoint = _maxEnergyPoint;
                _maxEnergyPoint = value;
                OnMaxEnergyPointChange.Invoke(this, previousMaxEnergyPoint, _maxEnergyPoint);
            }
        }

        public int ArmorPoint
        {
            get => _armorPoint;
            set
            {
                int previousArmorPoint = _armorPoint;
                _armorPoint = value;
                if (_armorPoint > _maxArmorPoint)
                {
                    _armorPoint = _maxArmorPoint;
                }
                OnArmorPointChange.Invoke(this, previousArmorPoint, _armorPoint);
            }
        }

        public int MaxArmorPoint
        {
            get => _maxArmorPoint;
            set
            {
                int previousMaxArmorPoint = _maxArmorPoint;
                _maxArmorPoint = value;
                OnMaxArmorPointChange.Invoke(this, previousMaxArmorPoint, _maxArmorPoint);
            }
        }

        public int AttackPoint
        {
            get => _attackPoint;
            set
            {
                int previousAttackPoint = _attackPoint;
                _attackPoint = value;
                if (_attackPoint > _maxAttackPoint)
                {
                    _attackPoint = _maxAttackPoint;
                }
                OnAttackPointChange.Invoke(this, previousAttackPoint, _attackPoint);
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
            Coordinate destination = Position + vector * count;
            return Move(destination);
        }

        public bool Move(Coordinate position)
        {
            return _boardDelegate.Place(this, position);
        }

        public bool Rotate(int count)
        {
            int range = Enum.GetValues(typeof(RobotDirection)).Length;
            RobotDirection direction = Direction;
            RobotDirection nextDirection = (RobotDirection) (((int) direction + count + range) % range);
            return Rotate(nextDirection);
        }

        public bool Rotate(RobotDirection direction)
        {
            return _boardDelegate.Rotate(this, direction);
        }

        public bool Attack(IRobotAttackStrategy strategy, params Coordinate[] relativePositions)
        {
            Coordinate currentPosition = Position;
            int checksum = 0;
            IList<Coordinate> targetPositions = new List<Coordinate>();
            IList<IRobotDelegate> robotDelegates = new List<IRobotDelegate>();
            foreach (Coordinate relativePosition in relativePositions)
            {
                Coordinate targetPosition = currentPosition + relativePosition;
                if (!IsValidCoordinate(targetPosition))
                {
                    continue;
                }

                targetPositions.Add(targetPosition);
                ICellDelegate cellDelegate = _boardDelegate[targetPosition];
                foreach (IRobotDelegate robotDelegate in cellDelegate.Placeables.Where(p => p is IRobotDelegate)
                            .Cast<IRobotDelegate>())
                {
                    robotDelegates.Add(robotDelegate);
                    checksum++;
                }
            }
            if (targetPositions.Count == 0)
            {
                return checksum != 0;
            }
            OnRobotAttack.Invoke(this, targetPositions);
            foreach (IRobotDelegate robotDelegate in robotDelegates)
            {
                robotDelegate.HealthPoint -= strategy.CalculateAttackPoint(this, robotDelegate);
            }
            return checksum != 0;
        }

        public int CompareTo(IGameEntity other)
        {
            return ID.CompareTo(other);
        }

        public UnityEvent<IRobotDelegate, Coordinate, Coordinate> OnRobotChangePosition { get; }

        public UnityEvent<IRobotDelegate, RobotDirection, RobotDirection> OnRobotChangeDirection { get; }

        public UnityEvent<IRobotDelegate, IList<Coordinate>> OnRobotAttack { get; }

        public UnityEvent<IRobotDelegate, int, int> OnHealthPointChange { get; }

        public UnityEvent<IRobotDelegate, int, int> OnMaxHealthPointChange { get; }

        public UnityEvent<IRobotDelegate, int, int> OnEnergyPointChange { get; }

        public UnityEvent<IRobotDelegate, int, int> OnMaxEnergyPointChange { get; }

        public UnityEvent<IRobotDelegate, int, int> OnArmorPointChange { get; }

        public UnityEvent<IRobotDelegate, int, int> OnMaxArmorPointChange { get; }

        public UnityEvent<IRobotDelegate, int, int> OnAttackPointChange { get; }

        public UnityEvent<IRobotDelegate, int, int> OnMaxAttackPointChange => OnMaxArmorPointChange;

        private bool IsValidCoordinate(Coordinate coordinate)
        {
            return coordinate.X >= 0
                && coordinate.X < _boardDelegate.Width
                && coordinate.Y >= 0
                && coordinate.Y < _boardDelegate.Height;
        }

        private void InvokeRobotChangePositionEvents(
            IRobotDelegate robotDelegate,
            Coordinate previousPosition,
            Coordinate newPosition)
        {
            if (robotDelegate == this)
            {
                OnRobotChangePosition.Invoke(this, previousPosition, newPosition);
            }
        }

        private void InvokeRobotChangeDirectionEvents(
            IRobotDelegate robotDelegate,
            RobotDirection previousDirection,
            RobotDirection newDirection)
        {
            if (robotDelegate == this)
            {
                OnRobotChangeDirection.Invoke(this, previousDirection, newDirection);
            }
        }
    }
}
