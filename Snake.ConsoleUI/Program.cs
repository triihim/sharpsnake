using Snake.Common;
using System;

namespace Snake
{

    public class Program
    {
        static void Main()
        {
            int gridSideLength = 18;
            int gameUpdateIntervalMs = 300;
            IInputSystem inputSystem = new ConsoleInputSystem(Direction.Up);
            GameConfiguration configuration = new GameConfiguration(gridSideLength, gameUpdateIntervalMs, inputSystem);

            ConsoleSnake game = new ConsoleSnake(configuration);
            game.Start();
        }

    }
}
