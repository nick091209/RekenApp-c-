using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace MathGame3F
{
    public class HighScore
    {
        public string PlayerName { get; set; }
        public string DifficultyLevel { get; set; }
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public double Percentage { get; set; }
        public DateTime Date { get; set; }

        // Leeg constructor voor serialisatie
        public HighScore() { }

        public HighScore(string playerName, Difficulty difficulty, int score, int totalQuestions)
        {
            PlayerName = playerName;
            DifficultyLevel = GetDifficultyText(difficulty);
            Score = score;
            TotalQuestions = totalQuestions;
            Percentage = (double)score / totalQuestions * 100;
            Date = DateTime.Now;
        }

        private string GetDifficultyText(Difficulty diff)
        {
            if (diff == Difficulty.Easy)
                return "Makkelijk";
            else if (diff == Difficulty.Medium)
                return "Normaal";
            else if (diff == Difficulty.Hard)
                return "Moeilijk";
            else
                return "Onbekend";
        }

        // Static methoden voor het beheren van highscores
        private static string GetHighScoresFilePath()
        {
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string gameFolder = Path.Combine(appDataFolder, "MathGame3F");

            if (!Directory.Exists(gameFolder))
            {
                Directory.CreateDirectory(gameFolder);
            }

            return Path.Combine(gameFolder, "highscores.xml");
        }

        public static List<HighScore> LoadHighScores()
        {
            string filePath = GetHighScoresFilePath();

            if (!File.Exists(filePath))
            {
                return new List<HighScore>();
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<HighScore>));
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    return (List<HighScore>)serializer.Deserialize(fs);
                }
            }
            catch
            {
                return new List<HighScore>();
            }
        }

        public static void SaveHighScore(HighScore newScore)
        {
            List<HighScore> highScores = LoadHighScores();
            highScores.Add(newScore);

            // Behoud alleen de top 10 scores per moeilijkheidsgraad
            var filteredScores = highScores
                .GroupBy(h => h.DifficultyLevel)
                .SelectMany(g => g.OrderByDescending(h => h.Percentage).ThenByDescending(h => h.Date).Take(10))
                .ToList();

            XmlSerializer serializer = new XmlSerializer(typeof(List<HighScore>));
            using (FileStream fs = new FileStream(GetHighScoresFilePath(), FileMode.Create))
            {
                serializer.Serialize(fs, filteredScores);
            }
        }

        public static List<HighScore> GetTopScores(int count = 10, string difficultyFilter = null)
        {
            List<HighScore> allScores = LoadHighScores();

            if (!string.IsNullOrEmpty(difficultyFilter))
            {
                allScores = allScores.Where(s => s.DifficultyLevel == difficultyFilter).ToList();
            }

            return allScores
                .OrderByDescending(s => s.Percentage)
                .ThenByDescending(s => s.Date)
                .Take(count)
                .ToList();
        }
    }
}