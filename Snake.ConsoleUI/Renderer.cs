using Snake.Common;
using Snake.Core;
using System;

namespace Snake
{
    internal class Renderer
    {
        private Game game;

        public Renderer(Game gameReference)
        {
            game = gameReference;
            Console.CursorVisible = false;
        }

        public void ClearScreen()
        {
            Console.Clear();
        }

        public void RenderGameScreen()
        {
            Console.SetCursorPosition(0, 0);
            RenderHeader();
            RenderGrid();
            RenderFooter();
        }

        public void RenderGameOverScreen()
        {
            Console.Clear();

            Console.WriteLine(UIElements.GameOverTitle);
            Console.WriteLine("Final score: " + game.GetCurrentScore());

            if (game.GetCurrentScore() > game.GetHighScore())
            {
                Console.WriteLine("NEW HIGH SCORE!");
            }

            Console.WriteLine("\nPress any key to play again.");
            Console.ReadKey();
        }

        private void RenderFooter()
        {
            int currentScore = game.GetCurrentScore();
            int highScore = game.GetHighScore();
            Console.Write("\n\tScore: " + currentScore);
            Console.WriteLine("\tHigh Score: " + highScore);
        }

        private void RenderHeader()
        {
            Console.WriteLine(UIElements.GameTitle);
        }

        private void RenderGrid()
        {

            CellType[,] grid = game.GetCurrentGameGridState();
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);

            for (int row = 0; row < rows; row++)
            {
                Console.Write("\t");
                for (int col = 0; col < cols; col++)
                {
                    Console.Write(GetPrintSymbol(grid[row, col]));
                }
                Console.WriteLine();
            }
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
    }
}
