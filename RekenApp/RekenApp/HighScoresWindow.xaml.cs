using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MathGame3F
{
    public partial class HighScoresWindow : Window
    {
        private string playerName;

        public class HighScoreViewModel
        {
            public int Rank { get; set; }
            public string PlayerName { get; set; }
            public string DifficultyLevel { get; set; }
            public int Score { get; set; }
            public int TotalQuestions { get; set; }
            public string ScoreText => $"{Score}/{TotalQuestions}";
            public double Percentage { get; set; }
            public DateTime Date { get; set; }
        }

        public HighScoresWindow(string name)
        {
            InitializeComponent();
            playerName = name;
            LoadHighScores();
        }

        private void LoadHighScores(string difficultyFilter = null)
        {
            // Haal highscores op
            List<HighScore> scores;

            if (difficultyFilter == "Alle niveaus" || string.IsNullOrEmpty(difficultyFilter))
            {
                scores = HighScore.GetTopScores();
            }
            else
            {
                scores = HighScore.GetTopScores(10, difficultyFilter);
            }

            // Converteer naar ViewModel met rangen
            var viewModels = scores.Select((s, index) => new HighScoreViewModel
            {
                Rank = index + 1,
                PlayerName = s.PlayerName,
                DifficultyLevel = s.DifficultyLevel,
                Score = s.Score,
                TotalQuestions = s.TotalQuestions,
                Percentage = s.Percentage,
                Date = s.Date
            }).ToList();

            dgHighScores.ItemsSource = viewModels;
        }

        private void cmbDifficulty_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbDifficulty.SelectedItem != null)
            {
                string selectedDifficulty = (cmbDifficulty.SelectedItem as ComboBoxItem)?.Content.ToString();
                LoadHighScores(selectedDifficulty);
            }
        }

        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {
            DifficultyWindow difficultyWindow = new DifficultyWindow(playerName);
            difficultyWindow.Show();
            this.Close();
        }

        private void btnMainMenu_Click(object sender, RoutedEventArgs e)
        {
            // Ga terug naar het beginscherm en geef de spelersnaam door
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            // Optioneel: Stel de spelersnaam in op het beginscherm
            // Dit vereist aanpassingen aan MainWindow om een publieke methode toe te voegen
            if (mainWindow.GetType().GetProperty("txtPlayerName") != null)
            {
                var textBox = mainWindow.GetType().GetProperty("txtPlayerName").GetValue(mainWindow) as TextBox;
                if (textBox != null)
                {
                    textBox.Text = playerName;
                }
            }

            this.Close();
        }
    }
}