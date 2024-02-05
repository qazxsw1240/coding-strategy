#nullable enable


namespace CodingStrategy.Entities
{
    public readonly struct Coordinate
    {
        public static readonly Coordinate Unit = new Coordinate(0, 0);

        private readonly int _x;
        private readonly int _y;

        public Coordinate(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public int X => _x;

        public int Y => _y;

        public override string ToString()
        {
            return $"Coordinate({_x}, {_y})";
        }

        public static Coordinate operator +(Coordinate lhs, Coordinate rhs) =>
            new Coordinate(lhs._x + rhs._x, lhs._y + rhs._y);

        public static Coordinate operator -(Coordinate lhs, Coordinate rhs) =>
            new Coordinate(lhs._x - rhs._x, lhs._y - rhs._y);

        public static Coordinate operator *(int scala, Coordinate rhs) =>
            new Coordinate(scala * rhs._x, scala * rhs._y);

        public static Coordinate operator *(Coordinate rhs, int scala) =>
            new Coordinate(scala * rhs._x, scala * rhs._y);

        public static bool operator ==(Coordinate lhs, Coordinate rhs) =>
            lhs.X == rhs.X && lhs.Y == rhs.Y;

        public static bool operator !=(Coordinate lhs, Coordinate rhs) =>
            lhs.X != rhs.X || lhs.Y != rhs.Y;
    }
}
