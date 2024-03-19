#nullable enable


namespace CodingStrategy.Entities
{
    using System;

    public interface IGameEntity : IComparable<IGameEntity>
    {
        public abstract string Id { get; }

        public TEntity As<TEntity>() where TEntity : class, IGameEntity
            => this as TEntity ?? throw new InvalidCastException();

        public new int CompareTo(IGameEntity other) => Id.CompareTo(other.Id);
    }
}
