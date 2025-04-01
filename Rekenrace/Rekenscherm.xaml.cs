using System;
using System.Windows;

namespace RekenApplicatie
{
    public partial class Rekenscherm : Window
    {
        private Random _random = new Random();
        private int _correctAntwoord;
        private int _score = 0;
        private int _vragenStelling = 0;
        private string _spelerNaam;

        public Rekenscherm(string spelerNaam)
        {
            InitializeComponent();
            _spelerNaam = spelerNaam;
            SpelerNaamTextBlock.Text = $"Welkom, {_spelerNaam}!";
        }

        private void StartOefeningenButton_Click(object sender, RoutedEventArgs e)
        {
            _score = 0;
            _vragenStelling = 0;
            GenerateVraag();
        }

        private void GenerateVraag()
        {
            if (_vragenStelling >= 10)
            {
                VraagTextBlock.Text = $"Je score is {_score} van de 10.";
                return;
            }

            _vragenStelling++;
            int niveau = MoeilijkheidsgraadComboBox.SelectedIndex;

            int eersteGetal = _random.Next(1, 10 * (niveau + 1));
            int tweedeGetal = _random.Next(1, 10 * (niveau + 1));
            int operatie = _random.Next(0, 4); // 0: +, 1: -, 2: *, 3: /

            switch (operatie)
            {
                case 0:
                    VraagTextBlock.Text = $"{eersteGetal} + {tweedeGetal}";
                    _correctAntwoord = eersteGetal + tweedeGetal;
                    break;
                case 1:
                    VraagTextBlock.Text = $"{eersteGetal} - {tweedeGetal}";
                    _correctAntwoord = eersteGetal - tweedeGetal;
                    break;
                case 2:
                    VraagTextBlock.Text = $"{eersteGetal} * {tweedeGetal}";
                    _correctAntwoord = eersteGetal * tweedeGetal;
                    break;
                case 3:
                    VraagTextBlock.Text = $"{eersteGetal} / {tweedeGetal}";
                    _correctAntwoord = eersteGetal / tweedeGetal;
                    break;
            }
        }

        private void AntwoordButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(AntwoordTextBox.Text, out int antwoord))
            {
                if (antwoord == _correctAntwoord)
                {
                    FeedbackTextBlock.Text = "Correct!";
                    _score++;
                }
                else
                {
                    FeedbackTextBlock.Text = "Onjuist!";
                }
                GenerateVraag();
                AntwoordTextBox.Clear();
            }
            else
            {
                MessageBox.Show("Vul een geldig nummer in!");
            }
        }
    }
}