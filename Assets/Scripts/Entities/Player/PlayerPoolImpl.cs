#nullable enable


namespace CodingStrategy.Entities.Player
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class PlayerPoolImpl : IPlayerPool
    {
        private readonly IDictionary<string, IPlayerDelegate> _pool;

        public PlayerPoolImpl()
        {
            _pool = new Dictionary<string, IPlayerDelegate>();
        }

        public IPlayerDelegate this[string id]
        {
            get => _pool[id];
            set => _pool[id] = value;
        }
        IPlayerDelegate IObjectPool<IPlayerDelegate>.this[string id] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Add(string id, Func<string, IPlayerDelegate> generator)
        {
            IPlayerDelegate obj = generator(id);
            _pool[id] = obj;
        }

        public void Clear() => _pool.Clear();

        public IEnumerator<IPlayerDelegate> GetEnumerator() => _pool.Values.GetEnumerator();

        public void Remove(string id) => _pool.Remove(id);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        IEnumerator<IPlayerDelegate> IEnumerable<IPlayerDelegate>.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
