#nullable enable


namespace CodingStrategy.Entities.Robot
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class RobotPoolImpl : IRobotPool
    {
        private readonly IDictionary<string, IRobotDelegate> _pool;

        public RobotPoolImpl()
        {
            _pool = new Dictionary<string, IRobotDelegate>();
        }

        public IRobotDelegate this[string id]
        {
            get => _pool[id];
            set => _pool[id] = value;
        }

        public void Add(string id, Func<string, IRobotDelegate> generator)
        {
            IRobotDelegate obj = generator(id);
            _pool[id] = obj;
        }

        public void Clear() => _pool.Clear();

        public IEnumerator<IRobotDelegate> GetEnumerator() => _pool.Values.GetEnumerator();

        public void Remove(string id) => _pool.Remove(id);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
