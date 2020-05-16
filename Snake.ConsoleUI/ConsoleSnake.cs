using System;
using System.Diagnostics;
using Snake.Common;
using Snake.Core;

namespace Snake
{
    internal class ConsoleSnake
    {
        private Game game;

        public ConsoleSnake(GameConfiguration configuration)
        {
            game = new Game(configuration);
            Initialize();
        }

        public void Start()
        {
            game.StartGameLoop();
        }

        private void Initialize()
        {
            Console.Clear();
            Console.CursorVisible = false;
            game.OnUpdate += Update;
            game.OnGameOver += GameOver;
            game.Initialize();
        }

        private void Update()
        {
            if(game.IsRunning())
            {
                Console.SetCursorPosition(0, 0);
                RenderHeader();
                RenderGrid();
                RenderScore();
            }
        }

        private void RenderHeader()
        {
            // Although ugly, the formatting must not be changed.
            string headerTitle = @"   ____               __    __    _ 
  / __/__  __ ____ __/ /__ / /__ (_)
 _\ \/ _ \/ // / // /  '_//  '_// / 
/___/_//_/\_,_/\_,_/_/\_\/_/\_\/_/  
                                    ";

            Console.WriteLine(headerTitle);
        }

        private void GameOver()
        {
            ShowGameOverMenu();
        }

        private void ShowGameOverMenu()
        {
            Console.Clear();
            string gameOverText = @"  _____                 ____              
 / ___/__ ___ _  ___   / __ \_  _____ ____
/ (_ / _ `/  ' \/ -_) / /_/ / |/ / -_) __/
\___/\_,_/_/_/_/\__/  \____/|___/\__/_/   
                                          ";

            Console.WriteLine(gameOverText);
            RenderScore();
            Console.WriteLine("\nPress any key to play again.");

            Console.ReadKey();
            Restart();
        }

        private void Restart()
        {
            Console.Clear();
            game.Reset();
            game.StartGameLoop();
        }

        private void RenderScore()
        {
            Console.WriteLine("Score: " + game.GetCurrentScore());
        }

        private string GetPrintSymbol(CellType cellType)
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

        private void RenderGrid()
        {

            CellType[,] grid = game.GetCurrentGameGridState();
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    Console.Write(GetPrintSymbol(grid[row, col]));
                }
                Console.WriteLine();
            }
        }
    }
}
