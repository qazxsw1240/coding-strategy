#nullable enable


using System;
using CodingStrategy.Entities.BadSector;
using CodingStrategy.Entities.Board;
using CodingStrategy.Entities.Robot;
using UnityEngine.Events;

namespace CodingStrategy.Entities
{
    [Obsolete]
    public interface IRuntimeDelegateController
    {
        public abstract IBoardDelegate BoardDelegate { get; }

        public abstract IRobotDelegatePool RobotDelegatePool { get; }

        public abstract IBadSectorDelegatePool BadSectorDelegatePool { get; }

        // TODO: PlayerDelegatePool

        public abstract UnityEvent<IRobotDelegate> OnRobotAdd { get; }

        public abstract UnityEvent<IRobotDelegate> OnRobotRemove { get; }

        public abstract UnityEvent<IRobotDelegate, Coordinate, Coordinate> OnRobotChangePosition { get; }

        public abstract UnityEvent<IRobotDelegate, RobotDirection, RobotDirection> OnRobotChangeDirection { get; }

        public abstract UnityEvent<IRobotDelegate, int, int> OnHealthPointChange { get; }

        public abstract UnityEvent<IRobotDelegate, int, int> OnEnergyPointChange { get; }

        public abstract UnityEvent<IRobotDelegate, int, int> OnArmorPointChange { get; }

        public abstract UnityEvent<IRobotDelegate, int, int> OnAttackPointChange { get; }

        public abstract UnityEvent<IBadSectorDelegate> OnBadSectorAdd { get; }

        public abstract UnityEvent<IBadSectorDelegate> OnBadSectorRemove { get; }

        // TODO: PlayerDelegate Events
    }
}
