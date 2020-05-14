namespace Snake.Common
{
    public interface IInputSystem
    {
        Direction GetMovementDirection();

        void ResetToDefaults();
    }
}
