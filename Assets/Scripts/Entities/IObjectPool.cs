#nullable enable


namespace CodingStrategy.Entities
{
    using System;
    using System.Collections.Generic;

    public interface IObjectPool<T> : IEnumerable<T>
    {
        public abstract T this[string id] { get; set; }

        public abstract void Add(string id, Func<string, T> generator);

        public abstract void Remove(string id);

        public abstract void Clear();
    }
}
