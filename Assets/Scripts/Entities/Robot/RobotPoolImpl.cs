#nullable enable


namespace CodingStrategy.Entities.Robot
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class RobotPoolImpl : IRobotPool
    {
        private readonly IDictionary<string, IRobot> _pool;

        public RobotPoolImpl()
        {
            _pool = new Dictionary<string, IRobot>();
        }

        public IRobot this[string id]
        {
            get => _pool[id];
            set => _pool[id] = value;
        }

        public void Add(string id, Func<string, IRobot> generator)
        {
            IRobot obj = generator(id);
            _pool[id] = obj;
        }

        public void Remove(string id) => _pool.Remove(id);

        public void Clear() => _pool.Clear();

        public IEnumerator<IRobot> GetEnumerator() => _pool.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
