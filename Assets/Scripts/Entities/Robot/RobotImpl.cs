#nullable enable


namespace CodingStrategy.Entities.Robot
{
    using UnityEngine.Events;

    public class RobotImpl : IRobot
    {
        private IRobotDelegate _delegate;
        private readonly UnityEvent<Coordinate, Coordinate> _robotChangePositionEvents;
        private readonly UnityEvent<RobotDirection, RobotDirection> _robotChangeDirectionEvents;
        private readonly UnityEvent<int, int> _healthPointChangeEvents;
        private readonly UnityEvent<int, int> _energyPointChangeEvents;
        private readonly UnityEvent<int, int> _armorPointChangeEvents;
        private readonly UnityEvent<int, int> _attackPointChangeEvents;

        public RobotImpl(IRobotDelegate robotDelegate)
        {
            _delegate = robotDelegate;
            _robotChangePositionEvents = new UnityEvent<Coordinate, Coordinate>();
            _robotChangeDirectionEvents = new UnityEvent<RobotDirection, RobotDirection>();
            _healthPointChangeEvents = new UnityEvent<int, int>();
            _energyPointChangeEvents = new UnityEvent<int, int>();
            _armorPointChangeEvents = new UnityEvent<int, int>();
            _attackPointChangeEvents = new UnityEvent<int, int>();

            AttachListeners();
        }

        public int Id => _delegate.Id;

        public IAlgorithm Algorithm => _delegate.Algorithm;

        public Coordinate Position => _delegate.Position;

        public RobotDirection Direction => _delegate.Direction;

        public int HealthPoint => _delegate.HealthPoint;

        public int EnergyPoint => _delegate.EnergyPoint;

        public int ArmorPoint => _delegate.ArmorPoint;

        public int AttackPoint => _delegate.AttackPoint;

        public bool Move(int count) => _delegate.Move(count);

        public bool Move(Coordinate position) => _delegate.Move(position);

        public bool Rotate(int count) => _delegate.Rotate(count);

        public bool Rotate(RobotDirection direction) => _delegate.Rotate(direction);

        public UnityEvent<Coordinate, Coordinate> OnRobotChangePosition => _robotChangePositionEvents;

        public UnityEvent<RobotDirection, RobotDirection> OnRobotChangeDirection => _robotChangeDirectionEvents;

        public UnityEvent<int, int> OnHealthPointChange => _healthPointChangeEvents;

        public UnityEvent<int, int> OnEnergyPointChange => _energyPointChangeEvents;

        public UnityEvent<int, int> OnArmorPointChange => _armorPointChangeEvents;

        public UnityEvent<int, int> OnAttackPointChange => _attackPointChangeEvents;

        public IRobotDelegate Delegate
        {
            get => _delegate;
            set
            {
                _delegate = value;
                AttachListeners();
            }
        }

        private void AttachListeners()
        {
            _delegate.OnRobotChangePosition.AddListener((_, previousPosition, newPosition) => _robotChangePositionEvents.Invoke(previousPosition, newPosition));
            _delegate.OnRobotChangeDirection.AddListener((_, previousDirection, newDirection) => _robotChangeDirectionEvents.Invoke(previousDirection, newDirection));
            _delegate.OnHealthPointChange.AddListener((_, previousHealthPoint, newHealthPoint) => _healthPointChangeEvents.Invoke(previousHealthPoint, newHealthPoint));
            _delegate.OnEnergyPointChange.AddListener((_, previousEnergyPoint, newEnergyPoint) => _energyPointChangeEvents.Invoke(previousEnergyPoint, newEnergyPoint));
            _delegate.OnArmorPointChange.AddListener((_, previousArmorPoint, newArmorPoint) => _armorPointChangeEvents.Invoke(previousArmorPoint, newArmorPoint));
            _delegate.OnAttackPointChange.AddListener((_, previousAttackPoint, newAttackPoint) => _attackPointChangeEvents.Invoke(previousAttackPoint, newAttackPoint));
        }
    }
}
