#nullable enable


using System;

namespace CodingStrategy.Entities.Runtime.Statement
{
    public class RotateMatrix
    {
        private readonly Coordinate[] Coordinates;
        public RotateMatrix(Coordinate coordinate1, Coordinate coordinate2)
        {
            Coordinates=new Coordinate[2];
            Coordinates[0] = coordinate1;
            Coordinates[1] = coordinate2;
        }
        public RotateMatrix() : this(new Coordinate(1,0), new Coordinate(0,1))
        {
        }
        public RotateMatrix(int num)
        {
            Coordinates=Rotate(num).Coordinates;
        }
        public static RotateMatrix operator *(RotateMatrix matrix1, RotateMatrix matrix2)
        {
            Coordinate coordinate1=new(
                matrix1.Coordinates[0].X*matrix2.Coordinates[0].X+matrix1.Coordinates[0].Y*matrix2.Coordinates[1].X,
                matrix1.Coordinates[0].X*matrix2.Coordinates[0].Y+matrix1.Coordinates[0].Y*matrix2.Coordinates[1].Y
                );
            Coordinate coordinate2=new(
                matrix1.Coordinates[1].X*matrix2.Coordinates[0].X+matrix1.Coordinates[1].Y*matrix2.Coordinates[1].X,
                matrix1.Coordinates[1].X*matrix2.Coordinates[0].Y+matrix1.Coordinates[1].Y*matrix2.Coordinates[1].Y
                );
            return new RotateMatrix(coordinate1, coordinate2);
        }
        public static Coordinate operator *(RotateMatrix matrix, Coordinate coordinate)
        {
            int x=matrix.Coordinates[0].X*coordinate.X+matrix.Coordinates[0].Y*coordinate.Y;
            int y=matrix.Coordinates[1].X*coordinate.X+matrix.Coordinates[1].Y*coordinate.Y;
            return new Coordinate(x,y);
        }
        public static RotateMatrix Rotate(int num)
        {
            RotateMatrix matrix=new RotateMatrix();
            RotateMatrix rotateMatrix=num>0?new RotateMatrix(new Coordinate(0,1), new Coordinate(-1,0))
                                            :new RotateMatrix(new Coordinate(0,-1), new Coordinate(1,0));
            for(int i=0;i<Math.Abs(num);i++)
            {
                matrix*=rotateMatrix;
            }
            return matrix;
        }
    }
}