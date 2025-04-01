using System.Windows;

namespace RekenApplicatie
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            string spelerNaam = NaamTextBox.Text.Trim();
            if (string.IsNullOrEmpty(spelerNaam))
            {
                MessageBox.Show("Vul een naam in!");
                return;
            }

            // Open het rekenscherm en sluit het huidige venster
            Rekenscherm rekenscherm = new Rekenscherm(spelerNaam);
            rekenscherm.Show();
            this.Close();
        }
    }
}