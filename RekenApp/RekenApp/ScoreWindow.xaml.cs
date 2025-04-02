using System;
using System.Windows;

namespace MathGame3F
{
    public partial class ScoreWindow : Window
    {
        private string playerName;
        private Difficulty difficulty;
        private int score;
        private int totalQuestions;

        public ScoreWindow(string name, Difficulty diff, int playerScore, int totalQ)
        {
            InitializeComponent();
            playerName = name;
            difficulty = diff;
            score = playerScore;
            totalQuestions = totalQ;

            // Update UI met speler informatie
            txtPlayerName.Text = playerName;
            txtDifficulty.Text = $"Moeilijkheidsgraad: {GetDifficultyText(difficulty)}";
            txtScore.Text = $"{score}/{totalQuestions}";

            // Bepaal feedback op basis van score
            double percentage = (double)score / totalQuestions * 100;
            if (percentage >= 90)
            {
                txtFeedback.Text = "Uitstekend! Je beheerst dit niveau perfect!";
            }
            else if (percentage >= 70)
            {
                txtFeedback.Text = "Goed gedaan! Je bent op de goede weg!";
            }
            else if (percentage >= 50)
            {
                txtFeedback.Text = "Niet slecht. Blijf oefenen om beter te worden!";
            }
            else
            {
                txtFeedback.Text = "Blijf oefenen! Probeer misschien een lager niveau.";
            }

            // Sla de score op in de highscores
            SaveHighScore();
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

        private void SaveHighScore()
        {
            try
            {
                HighScore newScore = new HighScore(playerName, difficulty, score, totalQuestions);
                HighScore.SaveHighScore(newScore);
            }
            catch (Exception ex)
            {
                // Log de fout of toon een bericht, maar laat de applicatie doorgaan
                Console.WriteLine($"Fout bij opslaan van highscore: {ex.Message}");
            }
        }

        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {
            // Start een nieuw spel met dezelfde speler
            DifficultyWindow difficultyWindow = new DifficultyWindow(playerName);
            difficultyWindow.Show();
            this.Close();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            // Ga terug naar het homescreen
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void btnHighScores_Click(object sender, RoutedEventArgs e)
        {
            // Open het highscores venster
            HighScoresWindow highScoresWindow = new HighScoresWindow(playerName);
            highScoresWindow.Show();
            this.Close();
        }
    }
}