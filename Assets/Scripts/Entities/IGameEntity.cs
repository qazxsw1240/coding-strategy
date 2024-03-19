#nullable enable


using System;

namespace CodingStrategy.Entities
{
    public interface IGameEntity
    {
        public TEntity As<TEntity>() where TEntity : class, IGameEntity
            => this as TEntity ?? throw new InvalidCastException();
    }
}
