using System;
using System.Collections.Generic;
using System.Timers;
using Snake.Common;
using Snake.GridSystem;

namespace Snake.Core
{
    public sealed class Game
    {
        private readonly Timer timer;
        private readonly GameConfiguration configuration;
        private GameState gameState;
        private SquareGrid grid;
        private Snake snake;
        private IInputSystem inputSystem;

        public event Action OnUpdate;
        public event Action OnGameOver;

        public Game(GameConfiguration configuration)
        {
            this.configuration = configuration;
            inputSystem = configuration.InputSystem;
            grid = new SquareGrid(configuration.GridSideLength);
            timer = new Timer(configuration.UpdateIntervalMs);
            gameState = new GameState();
        }

        public void Initialize()
        {
            gameState.IsGameOver = false;
            Coordinate snakeStartingPosition = grid.GetGridCenter();
            snake = new Snake(snakeStartingPosition);
            timer.Elapsed += Update;
        }

        public void Reset()
        {
            grid = new SquareGrid(configuration.GridSideLength);
            gameState = new GameState();
            Coordinate snakeStartingPosition = grid.GetGridCenter();
            snake = new Snake(snakeStartingPosition);
            inputSystem.ResetToDefaults();
        }

        public bool IsRunning()
        {
            return !gameState.IsGameOver;
        }

        public int GetCurrentScore()
        {
            return gameState.Score;
        }

        public int GetHighScore()
        {
            return gameState.HighScore;
        }

        public void StartGameLoop()
        {
            timer.Start();
            
            while (gameState.IsGameOver == false)
            {
                snake.MovementDirection = inputSystem.GetMovementDirection();
            }

            OnGameOver?.Invoke();
        }
        
        public CellType[,] GetCurrentGameGridState()
        {
            Cell[,] gridCopy = grid.GetGridCopy();
            int gridSideLength = grid.GetGridSideLength();

            CellType[,] gridRepresentation = new CellType[gridSideLength, gridSideLength];

            foreach (Cell cell in gridCopy)
            {
                int row = cell.Coordinate.Row;
                int col = cell.Coordinate.Column;
                gridRepresentation[row, col] = cell.CellType;
            }

            return gridRepresentation;
        }

        private void Update(object sender, ElapsedEventArgs e)
        {
            UpdateSnakePositionOnGrid();
            UpdateScorablesOnGrid();
            OnUpdate?.Invoke();
        }

        private void UpdateSnakePositionOnGrid()
        {
            // Order matters: 
            // Move snake > Collide? > Clear old snake > Place new snake.
            snake.Move();
            CheckForCollision();
            ClearSnakeFromGrid();
            PlaceSnakeOnGrid();
        }

        private void ClearSnakeFromGrid()
        {
            grid.ClearCellsOfType(CellType.Snake);
        }

        private void PlaceSnakeOnGrid()
        {
            foreach (Coordinate snakePos in snake.Positions)
            {
                grid.SetGridCellType(snakePos, CellType.Snake);
            }
        }

        private void CheckForCollision()
        {
            Coordinate movedPosition = snake.GetHeadPosition();
            Cell collidedCell = grid.GetGridCell(movedPosition);
            if (collidedCell.CellType != CellType.Empty)
            {
                HandleCollision(collidedCell);
            }
        }

        private void UpdateScorablesOnGrid()
        {
            if (gameState.IsMaxNumberOfScorablesPlaced() == false)
            {
                Coordinate randomCoordinate = GetRandomEmptyGridCoordinate();
                grid.SetGridCellType(randomCoordinate, CellType.Scorable);
                gameState.IncrementScorableCount();
            }
        }

        private Coordinate GetRandomEmptyGridCoordinate()
        {
            List<Coordinate> empties = grid.GetEmptyGridCellCoordinates();
            Random random = new Random();
            int randomIndex = random.Next(0, empties.Count);
            Coordinate randomCoordinate = empties[randomIndex];
            return randomCoordinate;
        }

        private void HandleCollision(Cell collidedCell)
        {
            switch (collidedCell.CellType)
            {
                case CellType.Obstacle:
                case CellType.Snake:
                    EndGame();
                    break;
                case CellType.Scorable:
                    AwardScore();
                    GrowSnake();
                    grid.ClearGridCell(collidedCell);
                    break;
                default:
                    break;
            }
        }

        private void AwardScore()
        {
            gameState.IncrementScore();
            gameState.DecrementScorableCount();
        }

        private void GrowSnake()
        {
            snake.Grow();
        }

        private void EndGame()
        {
            timer.Stop();
            gameState.IsGameOver = true;
            UpdateHighScore();
        }

        private void UpdateHighScore()
        {
            int currentHighScore = HighScore.GetHighScore();
            
            if(gameState.Score > currentHighScore)
            {
                HighScore.UpdateHighScore(gameState.Score);
            }
        }
    }
}
