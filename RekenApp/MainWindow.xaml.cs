using System.Windows;

namespace RekenApp
{
    public partial class MainWindow : Window
    {
        public static string UserName { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Please enter your name to continue.");
                return;
            }

            UserName = NameTextBox.Text;

            // Open difficulty selection window
            DifficultyWindow difficultyWindow = new DifficultyWindow();
            difficultyWindow.Show();
            this.Close();
        }
    }
}