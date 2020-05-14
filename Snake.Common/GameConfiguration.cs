using System;

namespace Snake.Common
{
    public sealed class GameConfiguration
    {
        private const int minGridSideLength = 10;
        private const int maxGridSideLength = 30;
        private const int minUpdateIntervalMs = 300;
        private const int maxUpdateIntervalMs = 1000;

        public int GridSideLength { get; private set; }
        public int UpdateIntervalMs { get; private set; }
        public IInputSystem InputSystem { get; private set; }

        public GameConfiguration(int gridSideLength, int updateIntervalMs, IInputSystem inputSystem)
        {
            if(!IsValidGridSideLength(gridSideLength))
            {
                throw new ArgumentOutOfRangeException(
                    string.Format("Invalid gridSideLength. Allowed range: {0}..{1}", minGridSideLength, maxGridSideLength));
            }

            if(!IsValidUpdateInterval(updateIntervalMs))
            {
                throw new ArgumentOutOfRangeException(
                    string.Format("Invalid updateIntervalMs. Allowed range: {0}..{1}", minUpdateIntervalMs, maxUpdateIntervalMs));
            }

            GridSideLength = gridSideLength;
            UpdateIntervalMs = updateIntervalMs;
            InputSystem = inputSystem;
        }

        private bool IsValidGridSideLength(int gridSideLength)
        {
            return gridSideLength >= minGridSideLength && gridSideLength <= maxGridSideLength;
        }

        private bool IsValidUpdateInterval(int updateIntervalMs)
        {
            return updateIntervalMs >= minUpdateIntervalMs && updateIntervalMs <= maxUpdateIntervalMs;
        }

    }
}
