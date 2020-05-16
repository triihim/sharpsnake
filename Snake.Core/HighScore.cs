using System;
using System.IO;

namespace Snake.Core
{
    internal static class HighScore
    {
        private static string fileName = "snakescore";
        private static readonly string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        private static string GetFilePath()
        {
            return Path.Combine(folderPath, fileName);
        }

        public static int GetHighScore()
        {
            int highScore = 0;

            try
            {
                string filePath = GetFilePath();
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                {
                    using (BinaryReader binaryReader = new BinaryReader(fileStream))
                    {
                        highScore = binaryReader.ReadInt32();
                    }
                }
            }
            catch (Exception e)
            {
                // If file doesn't exist yet, it will be created upon high score update.
            }

            return highScore;
        }

        internal static void UpdateHighScore(int newHighScore)
        {
            string filePath = GetFilePath();

            using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
            {   
                using(BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                {
                    binaryWriter.Write(newHighScore);
                }
            }
        }
    }
}
