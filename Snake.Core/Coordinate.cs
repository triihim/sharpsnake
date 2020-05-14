using Snake.Common;

using System;

namespace Snake.Core
{
    internal class Coordinate
    {
        public int Row { get; }
        public int Column { get; }

        public Coordinate(int row, int col)
        {
            Row = row;
            Column = col;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType()) return false;

            Coordinate otherCoordinate = (Coordinate)obj;
            return otherCoordinate.Row == Row && otherCoordinate.Column == Column;
        }

        public Coordinate GetAdjacent(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return new Coordinate(Row - 1, Column);
                case Direction.Right:
                    return new Coordinate(Row, Column + 1);
                case Direction.Down:
                    return new Coordinate(Row + 1, Column);
                case Direction.Left:
                    return new Coordinate(Row, Column - 1);
                default:
                    throw new ArgumentException("Invalid Direction");
            }
        }

        public static Coordinate GetRandomCoordinate(int rowMin, int rowMax, int colMin, int colMax)
        {
            Random random = new Random();
            int row = random.Next(rowMin, rowMax);
            int col = random.Next(colMin, colMax);
            Coordinate randomCoordinate = new Coordinate(row, col);
            return randomCoordinate;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Column);
        }
    }
}
