#nullable enable


using CodingStrategy.Entities.Robot;
using UnityEngine.Events;

namespace CodingStrategy.Entities.Placeable
{
    public interface IBitDelegate : IPlaceable
    {
        public int Amount { get; }

        public bool IsTaken { get; }

        public abstract UnityEvent<IRobotDelegate> OnRobotTakeInEvents { get; }

        public abstract UnityEvent<IRobotDelegate> OnRobotTakeAwayEvents { get; }
    }
}
