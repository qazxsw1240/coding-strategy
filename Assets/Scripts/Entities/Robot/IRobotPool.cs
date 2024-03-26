#nullable enable


using System;

namespace CodingStrategy.Entities.Robot
{
    [Obsolete("Deprecated", true)]
    public interface IRobotPool : IObjectPool<IRobot>
    {
    }
}
