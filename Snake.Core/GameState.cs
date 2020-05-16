namespace Snake.Core
{
    internal class GameState
    {
        private const int maxNumberOfScorables = 1;

        public int CurrentNumberOfScorables { get; private set; }
        public bool IsGameOver { get; set; }
        public int Score { get; private set; }
        public int HighScore { get; private set; }

        public GameState()
        {
            HighScore = Core.HighScore.GetHighScore();
        }

        public bool IsMaxNumberOfScorablesPlaced()
        {
            return CurrentNumberOfScorables >= maxNumberOfScorables;
        }

        public void IncrementScorableCount()
        {
            CurrentNumberOfScorables++;
        }

        public void DecrementScorableCount()
        {
            CurrentNumberOfScorables--;
        }

        public void IncrementScore()
        {
            Score++;
        }

    }
}
