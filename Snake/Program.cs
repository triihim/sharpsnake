using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace Snake
{
    enum CellType
    {
        Empty,
        Obstacle,
        Snake
    }

    enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    class Coordinate
    {
        public int Row { get; }
        public int Column { get; }

        public Coordinate(int row, int col)
        {
            Row = row;
            Column = col;
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
    }

    class Cell
    {
        public Coordinate Coordinate { get; }
        public CellType CellType { get; set; }

        public Cell(CellType cellType, Coordinate coordinate)
        {
            CellType = cellType;
            Coordinate = coordinate;
        }
    }

    class Snake
    {
        public Direction MovementDirection { get; set; }
        public List<Coordinate> Positions { get; private set; }

        public Snake(Coordinate startingLocation)
        {
            // TODO: Check location within grid.
            Positions = new List<Coordinate>();
            Positions.Add(startingLocation);

            // TODO: Settable starting length.
            for (int i = 0; i < 2; i++)
            {
                Positions.Add(Positions.Last().GetAdjacent(Direction.Down));
            }

        }

        public void Move()
        {
            Positions = CalculateNewPositions();
        }

        private List<Coordinate> CalculateNewPositions()
        {
            List<Coordinate> newPositions = new List<Coordinate>(Positions.Count);

            Coordinate newHeadPosition = CalculateNewHeadPosition();
            List<Coordinate> newBodyPositions = CalculateNewBodyPositions();

            newPositions.Add(newHeadPosition);
            newPositions.AddRange(newBodyPositions);

            return newPositions;
        }

        private List<Coordinate> CalculateNewBodyPositions()
        {
            List<Coordinate> bodyPositions = new List<Coordinate>();
            
            Coordinate previousPosition = Positions.First();
            foreach (Coordinate bodyPartPosition in Positions.Skip(1)) // Skip head in Positions.
            {
                bodyPositions.Add(previousPosition);
                previousPosition = bodyPartPosition;
            }

            return bodyPositions;
        }

        private Coordinate CalculateNewHeadPosition()
        {
            Coordinate oldHeadPosition = Positions.First();
            Coordinate newHeadPosition = oldHeadPosition.GetAdjacent(MovementDirection);
            return newHeadPosition;
        }
    }

    class Program
    {
        private const int gridSideLength = 20;
        private static Cell[,] grid;
        private static Snake snake;
        private static Timer timer;

        static void Main(string[] args)
        {
            Coordinate snakeStartLocation = GetGridCenter();
            snake = new Snake(snakeStartLocation);
            timer = new Timer(500);

            timer.Elapsed += UpdateGame;
            
            InitializeGame();
            
            timer.Start();

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        snake.MovementDirection = Direction.Up;
                        break;
                    case ConsoleKey.RightArrow:
                        snake.MovementDirection = Direction.Right;
                        break;
                    case ConsoleKey.DownArrow:
                        snake.MovementDirection = Direction.Down;
                        break;
                    case ConsoleKey.LeftArrow:
                        snake.MovementDirection = Direction.Left;
                        break;
                    default:
                        break;
                }
            }
        }

        private static void UpdateGame(object sender, ElapsedEventArgs e)
        {
            snake.Move();
            UpdateSnakePositionOnGrid();
            RenderGrid();
        }

        private static void InitializeGame()
        {
            Console.CursorVisible = false;
            CreateGrid();
            UpdateSnakePositionOnGrid();
            RenderGrid();
        }

        private static void UpdateSnakePositionOnGrid()
        {
            ClearSnakeFromGrid();
            PlaceSnakeOnGrid();
        }

        private static void PlaceSnakeOnGrid()
        {
            // TODO: Detect collisions.
            foreach (Coordinate snakePos in snake.Positions)
            {
                grid[snakePos.Row, snakePos.Column].CellType = CellType.Snake;
            }
        }

        private static void ClearSnakeFromGrid()
        {
            foreach (Cell cell in grid)
            {
                if (cell.CellType == CellType.Snake)
                {
                    cell.CellType = CellType.Empty;
                }
            }
        }

        private static Coordinate GetGridCenter()
        {
            return new Coordinate(gridSideLength / 2, gridSideLength / 2);
        }

        private static void CreateGrid()
        {
            grid = new Cell[gridSideLength, gridSideLength];

            for (int row = 0; row < gridSideLength; row++)
            {
                for (int col = 0; col < gridSideLength; col++)
                {
                    grid[row, col] = CreateGridCell(row, col);
                }
            }
        }

        private static Cell CreateGridCell(int row, int col)
        {
            Coordinate coordinate = new Coordinate(row, col);
            CellType cellType = IsGridBoundary(coordinate) ? CellType.Obstacle : CellType.Empty;
            return new Cell(cellType, coordinate);
        }

        private static bool IsGridBoundary(Coordinate coordinate)
        {
            if (coordinate.Row == 0 || coordinate.Row == gridSideLength - 1) return true;
            if (coordinate.Column == 0 || coordinate.Column == gridSideLength - 1) return true;
            return false;
        }

        private static void RenderGrid()
        {
            Console.SetCursorPosition(0, 0);

            for (int row = 0; row < 20; row++)
            {
                for (int col = 0; col < 20; col++)
                {
                    Console.Write(GetCellSymbol(grid[row, col].CellType));
                }
                Console.WriteLine();
            }
        }

        private static string GetCellSymbol(CellType cellType)
        {
            switch (cellType)
            {
                case CellType.Empty:
                    return "  ";
                case CellType.Obstacle:
                    return " X";
                case CellType.Snake:
                    return " O";
                default:
                    throw new ArgumentException("Invalid CellType");
            }
        }
    }
}
