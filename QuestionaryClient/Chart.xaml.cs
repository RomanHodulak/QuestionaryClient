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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Markup;
using System.Xml;
using System.Windows.Media.Effects;
using System.Windows.Controls.Primitives;

namespace QuestionaryClient
{
    /// <summary>
    /// Interaction logic for Chart.xaml
    /// </summary>
    public partial class Chart : UserControl
    {
        Grid lineBase;
        Color[] colors = new Color[]
        {
            Colors.Red,
            Colors.Green,
            Colors.CornflowerBlue,
            Colors.Brown,
        };
        int colorIndex = 0;

        public Chart()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            lineBase = FindResource("rectangle") as Grid;
            base.OnInitialized(e);
        }

        public void LoadQuiz(int quiz_id, int user_id)
        {
            desc.Children.Clear();
            Results results = new Results();
            QuestionResult[] res;
            Question[] questions;
            DB.LoadResult(quiz_id, user_id, out results);
            DB.LoadQuestionResults(results.ID, out res);
            DB.LoadQuestions(quiz_id, out questions);
            Answer[] answers = new Answer[res.Length];
            for (int i = 0; i < questions.Length; i++)
            {
                Answer[] a;
                AddLine(questions[i].Description);
                DB.LoadAnswers(questions[i].ID, out a);
                int total = 100;
                int totalCount = 0;
                int[] percentages = new int[a.Length];
                bool[] boldText = new bool[a.Length];
                for (int j = 0; j < a.Length; j++)
                {
                    DB.LoadQuestionResultsByAnswer(a[j].ID, out res);
                    totalCount += res.Length;
                    percentages[j] = res.Length;
                    boldText[j] = false;
                    for (int k = 0; k < res.Length; k++)
                    {
                        if (results.ID == res[k].Results_ID)
                        {
                            boldText[j] = true;
                            break;
                        }
                    }
                }
                for (int j = 0; j < a.Length; j++)
                {
                    int perc = (int)Math.Round(100.0f * ((float)percentages[j] / (float)totalCount));
                    total -= perc;
                    if (j == a.Length - 1 && total != 0 && total != 100)
                        perc += total;
                    AddLine(a[j].Description, boldText[j], perc, a[j].Correctness > 0 ? Colors.Green : Colors.Red);
                }
            }
        }

        void AddLine(string text, bool bold, int percentage, Color color)
        {
            if (percentage < 0 || percentage > 100)
                return;
            Grid stack = new Grid();
            stack.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(100, GridUnitType.Pixel),
                });
            stack.ColumnDefinitions.Add(new ColumnDefinition());
            Grid rect2 = (Grid)Clone(lineBase);
            Rectangle line = rect2.Children[0] as Rectangle;
            (line.Effect as DropShadowEffect).Color = color;
            (line.Fill as LinearGradientBrush).GradientStops[0].Color = darkerColor(color, 0.66f);
            (line.Fill as LinearGradientBrush).GradientStops[1].Color = darkerColor(color, 0.33f);
            rect2.ColumnDefinitions[0].Width = new GridLength((percentage) * 0.01, GridUnitType.Star);
            rect2.ColumnDefinitions[1].Width = new GridLength((100 - percentage) * 0.01, GridUnitType.Star);
            TextBlock block = new TextBlock()
            {
                Text = text + ":",
                Height = 20,
                Margin = new Thickness(5),
                FontWeight = bold ? FontWeights.Bold : FontWeights.Normal,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
            };
            TextBlock percentageText = rect2.Children[1] as TextBlock;
            percentageText.FontWeight = bold ? FontWeights.Bold : FontWeights.Normal;
            percentageText.Text = percentage + "%";
            stack.Children.Add(block);
            stack.Children.Add(rect2);
            Grid.SetColumn(rect2, 1);
            desc.Children.Add(stack);
        }

        void AddLine(string text)
        {
            TextBlock textBlock = new TextBlock()
            {
                Text = text,
                Height = 20,
                FontSize = 14,
                Margin = new Thickness(110,10,5,5),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
            };
            desc.Children.Add(textBlock);
        }

        Color darkerColor(Color col, float m)
        {
            byte A = (byte)(col.A * m);
            byte R = (byte)(col.R * m);
            byte G = (byte)(col.G * m);
            byte B = (byte)(col.B * m);
            Color ret = Color.FromArgb(A, R, G, B);
            return ret;
        }

        private FrameworkElement Clone(FrameworkElement e)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(XamlWriter.Save(e));
            return (FrameworkElement)XamlReader.Load(new XmlNodeReader(document));
        } 
    }
}
