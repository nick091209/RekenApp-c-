using System;
using System.Windows;

namespace MathGame3F
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    public partial class DifficultyWindow : Window
    {
        private string playerName;

        public DifficultyWindow(string name)
        {
            InitializeComponent();
            playerName = name;
            txtWelcome.Text = $"Welkom, {playerName}!";
        }

        private void btnEasy_Click(object sender, RoutedEventArgs e)
        {
            StartGame(Difficulty.Easy);
        }

        private void btnMedium_Click(object sender, RoutedEventArgs e)
        {
            StartGame(Difficulty.Medium);
        }

        private void btnHard_Click(object sender, RoutedEventArgs e)
        {
            StartGame(Difficulty.Hard);
        }

        private void StartGame(Difficulty difficulty)
        {
            GameWindow gameWindow = new GameWindow(playerName, difficulty);
            gameWindow.Show();
            this.Close();
        }
    }
}