#nullable enable

using CodingStrategy.Entities;

namespace CodingStrategy.Factory
{
    public interface IPlayerDelegateCreateStrategy
    {
        public abstract string Id { get; }

        public abstract IAlgorithm Algorithm { get; }

        public abstract int HealthPoint { get; }

        public abstract int Level { get; }

        public abstract int Exp { get; }

        public abstract int Currency { get; }
    }
}
