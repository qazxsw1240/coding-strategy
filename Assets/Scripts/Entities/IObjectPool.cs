using System;
using System.Collections.Generic;

namespace CodingStrategy.Entities
{
    public interface IObjectPool<TEntity> : IEnumerable<TEntity> where TEntity : IGameEntity
    {
        public abstract TEntity this[string id] { get; set; }

        public abstract void Add(string id, Func<string, TEntity> generator);

        public abstract bool Contains(string id);

        public abstract void Remove(string id);

        public abstract void Clear();
    }
}
