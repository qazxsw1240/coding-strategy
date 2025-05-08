#nullable enable

using CodingStrategy.Entities;
using CodingStrategy.Entities.Robot;

using UnityEngine;

namespace CodingStrategy
{
    public readonly struct PlayerPosition
    {
        public PlayerPosition(RobotDirection direction, Coordinate position, Color color)
        {
            Direction = direction;
            Position = position;
            Color = color;
        }

        public RobotDirection Direction { get; }

        public Coordinate Position { get; }

        public Color Color { get; }
    }
}
