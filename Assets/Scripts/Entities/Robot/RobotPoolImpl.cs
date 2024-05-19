#nullable enable


using System.Collections.Generic;

namespace CodingStrategy.Entities.Robot
{
    public class RobotPoolImpl : IRobotPool
    {
        private readonly IDictionary<string, IRobotDelegate> _pool;

        public RobotPoolImpl()
        {
            _pool = new Dictionary<string, IRobotDelegate>();
        }

        // TODO
        public IRobotDelegate this[string id] => _pool[id];
    }
}
