#nullable enable


namespace CodingStrategy.Entities.Robot
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class RobotDelegatePoolImpl : IRobotDelegatePool
    {
        private readonly IDictionary<string, IRobotDelegate> _pool;

        public RobotDelegatePoolImpl()
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

        public bool Contains(string id)
        {
            return _pool.ContainsKey(id);
        }

        public void Clear() => _pool.Clear();

        public IEnumerator<IRobotDelegate> GetEnumerator() => _pool.Values.GetEnumerator();

        public void Remove(string id) => _pool.Remove(id);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
