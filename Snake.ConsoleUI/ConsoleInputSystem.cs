using System;
using Snake.Common;

namespace Snake
{
    class ConsoleInputSystem : IInputSystem
    {
        private readonly Direction defaultDirection;
        private Direction currentMovementDirection;

        public ConsoleInputSystem(Direction defaultDirection)
        {
            this.defaultDirection = defaultDirection;
            currentMovementDirection = defaultDirection;
        }

        public Direction GetMovementDirection()
        {
            if(Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        currentMovementDirection = Direction.Up;
                        break;
                    case ConsoleKey.RightArrow:
                        currentMovementDirection = Direction.Right;
                        break;
                    case ConsoleKey.DownArrow:
                        currentMovementDirection = Direction.Down;
                        break;
                    case ConsoleKey.LeftArrow:
                        currentMovementDirection = Direction.Left;
                        break;
                    default:
                        break;
                }
            }

            return currentMovementDirection;
        }

        public void ResetToDefaults()
        {
            currentMovementDirection = defaultDirection;
        }
    }
}
