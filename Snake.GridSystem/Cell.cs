using Snake.Common;

namespace Snake.GridSystem
{
    public class Cell
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
