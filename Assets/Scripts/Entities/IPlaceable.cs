#nullable enable


namespace CodingStrategy.Entities
{
    using UnityEngine.Events;

    public interface IPlaceable
    {
        public abstract Coordinate Position { get; }

        public abstract bool Move(Coordinate destination);

        public abstract UnityEvent<Coordinate> OnPositionChanged { get; }
    }
}
