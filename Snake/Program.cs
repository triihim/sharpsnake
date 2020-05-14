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
        Scorable,
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
        private Direction movementDirection = Direction.Up;
        private bool shouldGrow = false;

        public List<Coordinate> Positions { get; private set; }

        public Direction MovementDirection 
        {
            get { return movementDirection; }
            set 
            { 
                if(IsNotOppositeDirection(value))
                {
                    movementDirection = value;
                }
            }
        }

        private bool IsNotOppositeDirection(Direction direction)
        {
            if (direction == Direction.Up && movementDirection == Direction.Down) return false;
            if (direction == Direction.Right && movementDirection == Direction.Left) return false;
            if (direction == Direction.Left && movementDirection == Direction.Right) return false;
            if (direction == Direction.Down && movementDirection == Direction.Up) return false;
            return true;
        }


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

        public Coordinate GetHeadPosition()
        {
            return Positions.First();
        }

        public void Move()
        {
            Positions = CalculateNewPositions();
        }

        public void Grow()
        {
            shouldGrow = true;
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

            if(shouldGrow)
            {
                bodyPositions.Add(previousPosition);
                shouldGrow = false;
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
        private const int maxNumberOfScorables = 1;
        private static int numberOfScorables = 0;
        private static int score = 0;
        private static bool gameOver = false;

        static void Main(string[] args)
        {
            timer = new Timer(500);

            timer.Elapsed += UpdateGame;

            while (true)
            {
                InitializeGame();
                StartGameLoop();
            }
        }

        private static void StartGameLoop()
        {
            timer.Start();

            while (!gameOver)
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
            UpdateScorables();
            RenderGrid();
            Console.WriteLine("Score: " + score);
        }

        private static void UpdateScorables()
        {
            if (numberOfScorables < maxNumberOfScorables)
            {
                // TODO: Refactor to cleaner solution.
                List<Cell> emptyCells = GetEmptyGridCells();
                int rowLimit = emptyCells.Max(cell => cell.Coordinate.Row) + 1;
                int colLimit = emptyCells.Max(cell => cell.Coordinate.Column) + 1;
                Random random = new Random();
                int row = random.Next(1, rowLimit);
                int col = random.Next(1, colLimit);
                grid[row, col].CellType = CellType.Scorable;
                numberOfScorables++;
            }
        }

        private static List<Cell> GetEmptyGridCells()
        {
            List<Cell> emptyCells = new List<Cell>();
            foreach (Cell cell in grid)
            {
                if (cell.CellType == CellType.Empty)
                {
                    emptyCells.Add(cell);
                }
            }
            return emptyCells;
        }

        private static void InitializeGame()
        {
            Console.Clear();
            Console.CursorVisible = false;
            gameOver = false;
            snake = new Snake(GetGridCenter());
            numberOfScorables = 0;
            CreateGrid();
            UpdateSnakePositionOnGrid();
            UpdateScorables();
            RenderGrid();
        }

        private static void UpdateSnakePositionOnGrid()
        {
            // Order matters.
            CheckForCollision();    
            ClearSnakeFromGrid();
            PlaceSnakeOnGrid();
        }

        private static void CheckForCollision()
        {
            Coordinate movedPosition = snake.GetHeadPosition();
            Cell collidedCell = GetGridCell(movedPosition);
            if(collidedCell.CellType != CellType.Empty)
            {
                HandleCollision(collidedCell);
            }
        }

        private static Cell GetGridCell(Coordinate movedPosition)
        {
            return grid[movedPosition.Row, movedPosition.Column];
        }

        private static void HandleCollision(Cell collidedCell)
        {
            switch (collidedCell.CellType)
            {
                case CellType.Obstacle:
                case CellType.Snake:
                    GameOver();
                    break;
                case CellType.Scorable:
                    AwardScore();
                    ClearGridCell(collidedCell);
                    break;
                default:
                    break;
            }
        }

        private static void AwardScore()
        {
            score++;
            numberOfScorables--;
            snake.Grow();
        }

        private static void ClearGridCell(Cell cell)
        {
            cell.CellType = CellType.Empty;
        }

        private static void GameOver()
        {
            timer.Stop();
            gameOver = true;
            // TODO: Move elsewhere to avoid coupling with console.
            Console.WriteLine("Game over :(");
            Console.WriteLine("Press any key to play again.");
        }

        private static CellType GetCellType(Coordinate coordinate)
        {
            return grid[coordinate.Row, coordinate.Column].CellType;
        }

        private static void PlaceSnakeOnGrid()
        {
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
                case CellType.Scorable:
                    return " ?";
                case CellType.Snake:
                    return " O";
                default:
                    throw new ArgumentException("Invalid CellType");
            }
        }
    }
}
