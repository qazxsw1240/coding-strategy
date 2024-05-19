#nullable enable


namespace CodingStrategy.Entities
{
    using CodingStrategy.Entities.BadSector;
    using CodingStrategy.Entities.Board;
    using CodingStrategy.Entities.Robot;
    using UnityEngine.Events;

    public class RuntimeDelegateControllerImpl : IRuntimeDelegateController
    {
        private readonly IBoardDelegate _boardDelegate;
        private readonly IRobotDelegatePool _robotDelegatePool;
        private readonly IBadSectorDelegatePool _badSectorDelegatePool;

        public RuntimeDelegateControllerImpl(
            IBoardDelegate boardDelegate,
            IRobotDelegatePool robotDelegatePool,
            IBadSectorDelegatePool badSectorDelegatePool)
        {
            _boardDelegate = boardDelegate;
            _robotDelegatePool = robotDelegatePool;
            _badSectorDelegatePool = badSectorDelegatePool;
        }

        public IBoardDelegate BoardDelegate => _boardDelegate;

        public IRobotDelegatePool RobotDelegatePool => _robotDelegatePool;

        public IBadSectorDelegatePool BadSectorDelegatePool => _badSectorDelegatePool;

        public UnityEvent<IRobotDelegate> OnRobotAdd => _boardDelegate.OnRobotAdd;

        public UnityEvent<IRobotDelegate> OnRobotRemove => _boardDelegate.OnRobotRemove;

        public UnityEvent<IRobotDelegate, Coordinate, Coordinate> OnRobotChangePosition => _boardDelegate.OnRobotChangePosition;

        public UnityEvent<IRobotDelegate, RobotDirection, RobotDirection> OnRobotChangeDirection => _boardDelegate.OnRobotChangeDirection;

        // TODO: implement this
        public UnityEvent<IRobotDelegate, int, int> OnHealthPointChange => throw new System.NotImplementedException();

        // TODO: implement this
        public UnityEvent<IRobotDelegate, int, int> OnEnergyPointChange => throw new System.NotImplementedException();

        // TODO: implement this
        public UnityEvent<IRobotDelegate, int, int> OnArmorPointChange => throw new System.NotImplementedException();

        // TODO: implement this
        public UnityEvent<IRobotDelegate, int, int> OnAttackPointChange => throw new System.NotImplementedException();

        public UnityEvent<IBadSectorDelegate> OnBadSectorAdd => _boardDelegate.OnBadSectorAdd;

        public UnityEvent<IBadSectorDelegate> OnBadSectorRemove => _boardDelegate.OnBadSectorRemove;
    }
}
