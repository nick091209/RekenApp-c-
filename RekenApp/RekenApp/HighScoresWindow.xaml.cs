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

        // Modify constructor to make name parameter optional
        public HighScoresWindow(string name = null)
        {
            InitializeComponent();
            playerName = name;

            // If no name is provided, hide or disable the "New Game" button
            if (string.IsNullOrEmpty(playerName))
            {
                btnNewGame.Visibility = Visibility.Collapsed;
            }

            // Ensure controls are properly initialized before loading data
            Loaded += (s, e) => {
                LoadHighScores();
            };
        }

        private void LoadHighScores(string difficultyFilter = null)
        {
            try
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

                // Check if scores is null to prevent further exceptions
                if (scores == null)
                {
                    scores = new List<HighScore>();
                    MessageBox.Show("Er konden geen scores worden geladen. Probeer het later opnieuw.", "Waarschuwing", MessageBoxButton.OK, MessageBoxImage.Warning);
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

                // Ensure dgHighScores is not null before setting ItemsSource
                if (dgHighScores != null)
                {
                    dgHighScores.ItemsSource = viewModels;
                }
                else
                {
                    MessageBox.Show("De scorelijst kon niet worden getoond. Probeer de applicatie opnieuw op te starten.", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Er is een fout opgetreden bij het laden van de scores: {ex.Message}", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cmbDifficulty_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbDifficulty?.SelectedItem != null)
            {
                string selectedDifficulty = (cmbDifficulty.SelectedItem as ComboBoxItem)?.Content.ToString();
                LoadHighScores(selectedDifficulty);
            }
        }

        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {
            // Only proceed if player name is available
            if (!string.IsNullOrEmpty(playerName))
            {
                DifficultyWindow difficultyWindow = new DifficultyWindow(playerName);
                difficultyWindow.Show();
                this.Close();
            }
            else
            {
                // If somehow this button is clicked without a name, go back to main window
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }

        private void btnMainMenu_Click(object sender, RoutedEventArgs e)
        {
            // Ga terug naar het beginscherm en geef de spelersnaam door
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            // Optioneel: Stel de spelersnaam in op het beginscherm als die bekend is
            if (!string.IsNullOrEmpty(playerName))
            {
                try
                {
                    if (mainWindow.GetType().GetProperty("txtPlayerName") != null)
                    {
                        var textBox = mainWindow.GetType().GetProperty("txtPlayerName").GetValue(mainWindow) as TextBox;
                        if (textBox != null)
                        {
                            textBox.Text = playerName;
                        }
                    }
                }
                catch (Exception)
                {
                    // Silently ignore property access errors
                }
            }

            this.Close();
        }
    }
}