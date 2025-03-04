#nullable enable

using System;

namespace CodingStrategy.Entities.Runtime.Statement
{
    public class RotateMatrix
    {
        private readonly Coordinate[] _coordinates;

        private RotateMatrix(Coordinate coordinate1, Coordinate coordinate2)
        {
            _coordinates = new Coordinate[2];
            _coordinates[0] = coordinate1;
            _coordinates[1] = coordinate2;
        }

        private RotateMatrix() : this(new Coordinate(1, 0), new Coordinate(0, 1)) {}

        public RotateMatrix(int num)
        {
            _coordinates = Rotate(num)._coordinates;
        }

        public static RotateMatrix operator *(RotateMatrix matrix1, RotateMatrix matrix2)
        {
            Coordinate coordinate1 = new Coordinate(
                matrix1._coordinates[0].X * matrix2._coordinates[0].X +
                matrix1._coordinates[0].Y * matrix2._coordinates[1].X,
                matrix1._coordinates[0].X * matrix2._coordinates[0].Y +
                matrix1._coordinates[0].Y * matrix2._coordinates[1].Y);
            Coordinate coordinate2 = new Coordinate(
                matrix1._coordinates[1].X * matrix2._coordinates[0].X +
                matrix1._coordinates[1].Y * matrix2._coordinates[1].X,
                matrix1._coordinates[1].X * matrix2._coordinates[0].Y +
                matrix1._coordinates[1].Y * matrix2._coordinates[1].Y);
            return new RotateMatrix(coordinate1, coordinate2);
        }

        public static Coordinate operator *(RotateMatrix matrix, Coordinate coordinate)
        {
            int x = matrix._coordinates[0].X * coordinate.X + matrix._coordinates[0].Y * coordinate.Y;
            int y = matrix._coordinates[1].X * coordinate.X + matrix._coordinates[1].Y * coordinate.Y;
            return new Coordinate(x, y);
        }

        public static RotateMatrix Rotate(int num)
        {
            RotateMatrix matrix = new RotateMatrix();
            RotateMatrix rotateMatrix = num > 0
                ? new RotateMatrix(new Coordinate(0, 1), new Coordinate(-1, 0))
                : new RotateMatrix(new Coordinate(0, -1), new Coordinate(1, 0));
            for (int i = 0; i < Math.Abs(num); i++)
            {
                matrix *= rotateMatrix;
            }

            return matrix;
        }
    }
}
