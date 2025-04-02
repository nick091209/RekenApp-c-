using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace MathGame3F
{
    public class MathProblem
    {
        public string Question { get; set; }
        public double Answer { get; set; }
        public string AnswerFormatted => Math.Abs(Answer - Math.Round(Answer)) < 0.001 ? Answer.ToString("0") : Answer.ToString("0.##");
        public string OperatorType { get; set; }
    }

    public partial class GameWindow : Window
    {
        private string playerName;
        private Difficulty difficulty;
        private List<MathProblem> problems;
        private int currentProblemIndex;
        private int score;
        private Random random;

        public GameWindow(string name, Difficulty diff)
        {
            InitializeComponent();
            playerName = name;
            difficulty = diff;
            random = new Random();

            // Initialiseer het spel
            InitializeGame();
        }

        private void InitializeGame()
        {
            // Update speler info en voortgang
            string difficultyText = "";
            if (difficulty == Difficulty.Easy)
                difficultyText = "Makkelijk";
            else if (difficulty == Difficulty.Medium)
                difficultyText = "Normaal";
            else if (difficulty == Difficulty.Hard)
                difficultyText = "Moeilijk";
            else
                difficultyText = "Onbekend";

            txtPlayerInfo.Text = $"Speler: {playerName} | Moeilijkheidsgraad: {difficultyText}";

            // Genereer 10 problemen
            problems = GenerateProblems(10);
            currentProblemIndex = 0;
            score = 0;

            // Toon het eerste probleem
            DisplayCurrentProblem();
        }

        private List<MathProblem> GenerateProblems(int count)
        {
            List<MathProblem> problemList = new List<MathProblem>();
            string[] operators = { "+", "-", "*", "/" };

            for (int i = 0; i < count; i++)
            {
                // Kies willekeurig een operator, maar zorg ervoor dat we verschillende operators gebruiken
                string op = operators[i % 4];

                // Genereer getallen op basis van moeilijkheidsgraad
                MathProblem problem = GenerateProblem(op);
                problemList.Add(problem);
            }

            return problemList;
        }

        private MathProblem GenerateProblem(string op)
        {
            int num1, num2;
            double answer;
            string question;

            if (difficulty == Difficulty.Easy)
            {
                num1 = random.Next(1, 11); // 1-10
                num2 = random.Next(1, 11); // 1-10
            }
            else if (difficulty == Difficulty.Medium)
            {
                num1 = random.Next(1, 51); // 1-50
                num2 = random.Next(1, 51); // 1-50
            }
            else if (difficulty == Difficulty.Hard)
            {
                num1 = random.Next(1, 101); // 1-100
                num2 = random.Next(1, 101); // 1-100
            }
            else
            {
                num1 = random.Next(1, 11);
                num2 = random.Next(1, 11);
            }

            // Zorg ervoor dat delingen netjes uitkomen bij lagere moeilijkheidsgraden
            if (op == "/" && (difficulty == Difficulty.Easy || difficulty == Difficulty.Medium))
            {
                num2 = random.Next(1, 11);
                num1 = num2 * random.Next(1, (difficulty == Difficulty.Easy ? 10 : 20));
            }

            // Bij moeilijk niveau, soms een breuk toevoegen
            if (difficulty == Difficulty.Hard && random.Next(100) < 40 && op != "/")
            {
                double fraction = Math.Round(random.NextDouble(), 1);
                if (op == "+")
                {
                    question = $"Wat is {num1} + {num2} + {fraction}?";
                    answer = num1 + num2 + fraction;
                }
                else if (op == "-")
                {
                    question = $"Wat is {num1} - {num2} - {fraction}?";
                    answer = num1 - num2 - fraction;
                }
                else // op == "*"
                {
                    question = $"Wat is {num1} * {fraction}?";
                    answer = num1 * fraction;
                }
            }
            else
            {
                // Standaard berekening op basis van operator
                if (op == "+")
                {
                    question = $"Wat is {num1} + {num2}?";
                    answer = num1 + num2;
                }
                else if (op == "-")
                {
                    // Zorg ervoor dat het antwoord niet negatief is bij lagere niveaus
                    if ((difficulty == Difficulty.Easy || difficulty == Difficulty.Medium) && num1 < num2)
                    {
                        int temp = num1;
                        num1 = num2;
                        num2 = temp;
                    }
                    question = $"Wat is {num1} - {num2}?";
                    answer = num1 - num2;
                }
                else if (op == "*")
                {
                    question = $"Wat is {num1} * {num2}?";
                    answer = num1 * num2;
                }
                else if (op == "/")
                {
                    question = $"Wat is {num1} / {num2}?";
                    answer = (double)num1 / num2;
                    // Rond het antwoord af naar 2 decimalen
                    answer = Math.Round(answer, 2);
                }
                else
                {
                    question = $"Wat is {num1} + {num2}?";
                    answer = num1 + num2;
                }
            }

            return new MathProblem
            {
                Question = question,
                Answer = answer,
                OperatorType = op
            };
        }

        private void DisplayCurrentProblem()
        {
            if (currentProblemIndex < problems.Count)
            {
                txtProgress.Text = $"Vraag {currentProblemIndex + 1} van {problems.Count}";
                txtQuestion.Text = problems[currentProblemIndex].Question;
                txtAnswer.Text = string.Empty;
                resultBorder.Visibility = Visibility.Collapsed;
                btnNext.IsEnabled = false;
                txtAnswer.Focus();
            }
            else
            {
                // Alle vragen zijn beantwoord, toon de score
                ShowFinalScore();
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            CheckAnswer();
        }

        private void CheckAnswer()
        {
            if (string.IsNullOrWhiteSpace(txtAnswer.Text))
            {
                MessageBox.Show("Voer een antwoord in.", "Antwoord nodig", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool isCorrect = false;
            try
            {
                double userAnswer = Convert.ToDouble(txtAnswer.Text.Replace(',', '.'));
                double correctAnswer = problems[currentProblemIndex].Answer;

                // Controleer met een kleine marge voor afrondingsfouten
                isCorrect = Math.Abs(userAnswer - correctAnswer) < 0.01;

                if (isCorrect)
                {
                    score++;
                }

                // Toon feedback
                resultBorder.BorderBrush = isCorrect ? Brushes.Green : Brushes.Red;
                resultBorder.Background = isCorrect ? new SolidColorBrush(Color.FromArgb(50, 0, 255, 0)) : new SolidColorBrush(Color.FromArgb(50, 255, 0, 0));
                txtResult.Text = isCorrect
                    ? "Correct! Goed gedaan."
                    : $"Helaas, het correcte antwoord is {problems[currentProblemIndex].AnswerFormatted}.";
                txtResult.Foreground = isCorrect ? Brushes.DarkGreen : Brushes.DarkRed;
                resultBorder.Visibility = Visibility.Visible;

                // Activeer de 'Volgende' knop
                btnNext.IsEnabled = true;
                btnSubmit.IsEnabled = false;
            }
            catch
            {
                MessageBox.Show("Voer een geldig getal in.", "Ongeldig antwoord", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            currentProblemIndex++;
            btnSubmit.IsEnabled = true;
            DisplayCurrentProblem();
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Weet je zeker dat je het spel wilt stoppen?", "Bevestiging", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Toon score als ze halverwege stoppen
                ShowFinalScore();
            }
        }

        private void ShowFinalScore()
        {
            ScoreWindow scoreWindow = new ScoreWindow(playerName, difficulty, score, problems.Count);
            scoreWindow.Show();
            this.Close();
        }
    }
}