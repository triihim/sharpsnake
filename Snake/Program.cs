using Snake.Common;

namespace Snake
{
    public class Program
    {
        static void Main()
        {
            int gridSideLength = 20;
            int gameUpdateIntervalMs = 500;
            IInputSystem inputSystem = new ConsoleInputSystem(Direction.Up);
            GameConfiguration configuration = new GameConfiguration(gridSideLength, gameUpdateIntervalMs, inputSystem);

            ConsoleSnake game = new ConsoleSnake(configuration);
            game.Start();
        }

       
    }
}
