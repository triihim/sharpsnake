using Snake.Common;

namespace Snake.Core
{
    internal class Cell
    {
        public Coordinate Coordinate { get; }
        public CellType CellType { get; set; }

        public Cell(CellType cellType, Coordinate coordinate)
        {
            CellType = cellType;
            Coordinate = coordinate;
        }
    }
}
