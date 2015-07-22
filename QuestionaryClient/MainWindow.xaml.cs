using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuestionaryClient
{
    public partial class MainWindow : Window
    {
        Storyboard fadeout;
        Storyboard fadein;
        Quiz currentQuiz = null;
        Question[] currentQuizQuestions;
        UIMode curMode = UIMode.Login;
        User curUser = null;

        public MainWindow()
        {
            InitializeComponent();
            fadeout = Resources["titleFadeOut"] as Storyboard;
            fadeout.Completed += sb_Completed;
            fadein = Resources["titleFadeIn"] as Storyboard;
            DB.InitializeConnection();
            Quiz[] quizes;
            DB.LoadQuizes(out quizes);
            foreach (Quiz q in quizes)
            {
                List.Items.Add(new ListBoxItem() { Content = q.Description, Tag = q, });
                DB.LoadQuestions(q.ID, out currentQuizQuestions);
            }
            curMode = UIMode.Login;
            sb_Completed(this, new EventArgs());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SwitchMode();
        }

        void SwitchMode()
        {
            if (curMode == UIMode.Login)
            {
                LoginPage loginPage = contentPanel.Children[0] as LoginPage;
                curUser = loginPage.login();
                if (curUser != null)
                    if (!loginPage.Wait)
                        curMode = UIMode.QuizSelection;
                else
                    return;
            }
            else if (curMode == UIMode.QuizSelection)
            {
                if (List.SelectedItem != null)
                {
                    curMode = UIMode.Questionary;
                    currentQuiz = (List.SelectedItem as ListBoxItem).Tag as Quiz;
                }
            }
            else if (curMode == UIMode.Questionary)
            {
                curMode = UIMode.ResultsView;
            }
            else if (curMode == UIMode.ResultsView)
            {
                curMode = UIMode.QuizSelection;
            }
            fadeout.Begin();
        }

        StackPanel quizViewer;
        void sb_Completed(object sender, EventArgs e)
        {
            if (curMode == UIMode.Questionary)
            {
                title.Text = currentQuiz.Description;
                button.Content = "Submit";
                ScrollViewer scrollViewer = new ScrollViewer();
                scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                Border border = new Border();
                border.Padding = new Thickness(1);
                /*border.BorderThickness = new Thickness(1);
                border.BorderBrush = List.BorderBrush;
                border.Background = Brushes.White;*/
                quizViewer = new StackPanel();
                quizViewer.Orientation = Orientation.Vertical;
                for (int i = 0; i < currentQuizQuestions.Length; i++)
                {
                    StackPanel answers = new StackPanel();
                    answers.Orientation = Orientation.Vertical;
                    answers.Margin = new Thickness(0, 0, 0, 5);
                    Answer[] a;
                    DB.LoadAnswers(currentQuizQuestions[i].ID, out a);
                    if (a != null)
                    {
                        answers.Children.Add(new TextBlock() { Text = currentQuizQuestions[i].Description, FontFamily = new FontFamily("Segoe UI"), FontSize = 16, Tag = currentQuizQuestions[i] });
                        for (int j = 0; j < a.Length; j++)
                        {
                            answers.Children.Add(new RadioButton() { Content = a[j].Description, FontFamily = new FontFamily("Segoe UI"), Tag = a[j] });
                        }
                        quizViewer.Children.Add(answers);
                    }
                }
                scrollViewer.Content = quizViewer;
                border.Child = scrollViewer;
                contentPanel.Children.Clear();
                contentPanel.Children.Add(border);
            }
            else if (curMode == UIMode.ResultsView)
            {
                Results results = new Results(currentQuiz.ID, curUser.ID);
                DB.InsertResults(ref results);
                foreach (object o in quizViewer.Children)
                {
                    if (o is StackPanel)
                    {
                        StackPanel answers = o as StackPanel;
                        Question question = null;
                        for (int i = 0; i < answers.Children.Count; i++)
                        {
                            object obj = answers.Children[i];
                            if (obj is TextBlock)
                            {
                                TextBlock tb = obj as TextBlock;
                                question = (Question)tb.Tag;
                            }
                            else if (obj is RadioButton)
                            {
                                RadioButton rb = obj as RadioButton;
                                if (rb.IsChecked.Value)
                                {
                                    Answer answer = (Answer)rb.Tag;
                                    QuestionResult result = new QuestionResult(answer.ID, results.ID);
                                    DB.InsertQuestionResults(result);
                                }
                            }
                        }
                    }
                }
                contentPanel.Children.Clear();
                Chart ch = new Chart();
                ch.LoadQuiz(currentQuiz.ID, curUser.ID);
                contentPanel.Children.Add(ch);
                title.Text = "Results:";
                button.Content = "Ok";
            }
            else if (curMode == UIMode.QuizSelection)
            {
                contentPanel.Children.Clear();
                contentPanel.Children.Add(List);
                title.Text = "Choose quiz:";
                button.Content = "Start";
            }
            else if (curMode == UIMode.Login)
            {
                contentPanel.Children.Clear();
                LoginPage page = new LoginPage();
                page.Submit += Button_Click_1;
                contentPanel.Children.Add(page);
                title.Text = "Login";
                button.Content = "Ok";
            }
            fadein.Begin();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            DB.CloseConnection();
            base.OnClosing(e);
        }

        private void List_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Button_Click_1(sender, new RoutedEventArgs());
        }
    }

    enum UIMode
    {
        Login,
        QuizSelection,
        Questionary,
        ResultsView,
    }
}
