using System.Collections.Generic;
using Snake.Common;

namespace Snake.GridSystem
{
    public class SquareGrid
    {
        private readonly int sideLength;

        private Cell[,] grid;

        public SquareGrid(int sideLength)
        {
            this.sideLength = sideLength;
            CreateGrid();
        }

        public Cell[,] GetGridCopy()
        {
            return (Cell[,])grid.Clone();
        }

        public List<Coordinate> GetEmptyGridCellCoordinates()
        {
            List<Coordinate> coordsOfEmpties = new List<Coordinate>();
            foreach (Cell cell in grid)
            {
                if (cell.CellType == CellType.Empty)
                {
                    coordsOfEmpties.Add(cell.Coordinate);
                }
            }
            return coordsOfEmpties;
        }

        public Cell GetGridCell(Coordinate coordinate)
        {
            return grid[coordinate.Row, coordinate.Column];
        }

        public int GetGridSideLength()
        {
            return sideLength;
        }

        public void SetGridCellType(Coordinate coordinate, CellType cellType)
        {
            grid[coordinate.Row, coordinate.Column].CellType = cellType;
        }

        public void ClearGridCell(Cell cell)
        {
            cell.CellType = CellType.Empty;
        }

        public void ClearCellsOfType(CellType cellType)
        {
            foreach (Cell cell in grid)
            {
                if (cell.CellType == cellType)
                {
                    cell.CellType = CellType.Empty;
                }
            }
        }

        public Coordinate GetGridCenter()
        {
            return new Coordinate(sideLength / 2, sideLength / 2);
        }

        private void CreateGrid()
        {
            grid = new Cell[sideLength, sideLength];

            for (int row = 0; row < sideLength; row++)
            {
                for (int col = 0; col < sideLength; col++)
                {
                    grid[row, col] = CreateGridCell(row, col);
                }
            }
        }

        private Cell CreateGridCell(int row, int col)
        {
            Coordinate coordinate = new Coordinate(row, col);
            CellType cellType = IsGridBoundary(coordinate) ? CellType.Obstacle : CellType.Empty;
            return new Cell(cellType, coordinate);
        }

        private bool IsGridBoundary(Coordinate coordinate)
        {
            if (coordinate.Row == 0 || coordinate.Row == sideLength - 1) return true;
            if (coordinate.Column == 0 || coordinate.Column == sideLength - 1) return true;
            return false;
        }


    }
}
