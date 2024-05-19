#nullable enable


namespace CodingStrategy.Entities
{
    using System;
    using System.Collections.Generic;

    public interface IObjectPool<TEntity> : IEnumerable<TEntity> where TEntity : IGameEntity
    {
        public abstract TEntity this[string id] { get; set; }

        public abstract void Add(string id, Func<string, TEntity> generator);

        public abstract void Remove(string id);

        public abstract void Clear();
    }
}
