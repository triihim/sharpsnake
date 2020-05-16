namespace Snake
{
    internal static class UIElements
    {
        private const string gameTitle = @"   
       ______                 ____          __      
      / __/ /  ___ ________  / __/__  ___ _/ /_____ 
     _\ \/ _ \/ _ `/ __/ _ \_\ \/ _ \/ _ `/  '_/ -_)
    /___/_//_/\_,_/_/ / .__/___/_//_/\_,_/_/\_\\__/ 
                     /_/                            
    ";

        private const string gameOverTitle = @"
      _____                 ____               
     / ___/__ ___ _  ___   / __ \_  _____ ____
    / (_ / _ `/  ' \/ -_) / /_/ / |/ / -_) __/
    \___/\_,_/_/_/_/\__/  \____/|___/\__/_/   
                                          
    ";

        public static string GameTitle
        {
            get
            {
                return gameTitle;
            }
        }

        public static string GameOverTitle
        {
            get
            {
                return gameOverTitle;
            }
        }
    }
}
