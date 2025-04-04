using System;
using System.Windows;

namespace MathGame3F
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Focus op het naamveld bij opstarten
            txtPlayerName.Focus();
        }

        private void btnStartGame_Click(object sender, RoutedEventArgs e)
        {
            StartGame();
        }

        // Add this method to your MainWindow class
        private void btnViewHighScores_Click(object sender, RoutedEventArgs e)
        {
            // Open high scores window without requiring a player name
            HighScoresWindow highScoresWindow = new HighScoresWindow();
            highScoresWindow.Show();
        }

        private void StartGame()
        {
            // Valideer dat er een naam is ingevuld
            if (string.IsNullOrWhiteSpace(txtPlayerName.Text))
            {
                MessageBox.Show("Voer alsjeblieft je naam in.", "Naam vereist", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPlayerName.Focus();
                return;
            }

            // Open het moeilijkheidsgraad selectie venster
            DifficultyWindow difficultyWindow = new DifficultyWindow(txtPlayerName.Text);
            difficultyWindow.Show();
            this.Close();
        }

        private void ShowHighScores()
        {
            // Voor highscores hebben we ook een naam nodig voor als de gebruiker een nieuw spel wil starten
            if (string.IsNullOrWhiteSpace(txtPlayerName.Text))
            {
                MessageBox.Show("Voer alsjeblieft je naam in voordat je de highscores bekijkt.", "Naam vereist", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPlayerName.Focus();
                return;
            }

            // Open het highscores venster
            HighScoresWindow highScoresWindow = new HighScoresWindow(txtPlayerName.Text);
            highScoresWindow.Show();
            this.Close();
        }
    }
}