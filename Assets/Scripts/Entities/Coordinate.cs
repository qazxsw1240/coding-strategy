#nullable enable

using System;

namespace CodingStrategy.Entities
{
    public readonly struct Coordinate
        : IEquatable<Coordinate>
    {
        public static readonly Coordinate Unit = new Coordinate(0, 0);

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }

        public int Y { get; }

        public override bool Equals(object? obj)
        {
            return obj != null && obj is Coordinate coord && this == coord;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public override string ToString()
        {
            return $"Coordinate({X}, {Y})";
        }

        public static Coordinate operator +(Coordinate lhs, Coordinate rhs)
        {
            return new Coordinate(lhs.X + rhs.X, lhs.Y + rhs.Y);
        }

        public static Coordinate operator -(Coordinate lhs, Coordinate rhs)
        {
            return new Coordinate(lhs.X - rhs.X, lhs.Y - rhs.Y);
        }

        public static Coordinate operator *(int scala, Coordinate rhs)
        {
            return new Coordinate(scala * rhs.X, scala * rhs.Y);
        }

        public static Coordinate operator *(Coordinate rhs, int scala)
        {
            return new Coordinate(scala * rhs.X, scala * rhs.Y);
        }

        public static bool operator ==(Coordinate lhs, Coordinate rhs)
        {
            return lhs.X == rhs.X && lhs.Y == rhs.Y;
        }

        public static bool operator !=(Coordinate lhs, Coordinate rhs)
        {
            return lhs.X != rhs.X || lhs.Y != rhs.Y;
        }

        public bool Equals(Coordinate other)
        {
            return X == other.X && Y == other.Y;
        }
    }
}
