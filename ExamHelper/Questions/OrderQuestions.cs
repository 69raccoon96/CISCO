using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ExamHelper
{
    public class OrderQuestions : IQuestion
    {
        public string Question { get; set; }
        private string Answer { get; set; }
        private GroupBox GroupBox;
        public static Action answerCallback;
        
        public GroupBox CreateQuestion()
        {
            var gb = new GroupBox {Location = new Point(0, 0), Size = new Size(1000, 400)};
            //var text = Utilities.SplitToLines(Question, 150);
            var label = new Label {Text = Question, Location = new Point(10, 10), AutoSize = true, Size = new Size(800, 100)};
            const int x = 10;
            var y = label.Location.Y + label.Size.Height + 150;
            var input = new TextBox
            {
                Location = new Point(x, y), AutoSize = true
            };
            input.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                    answerCallback();
            };
            gb.Controls.Add(input);
            y += 30;
            var hint = new Button
            {
                Location = new Point(x,y),
                Text = "Подсказка",
                AccessibleDescription = Answer,
                AutoSize = true
            };
            hint.Click += (s, e) =>
            {
                ButtonClick(s);
            };
            gb.Controls.Add(hint);

            gb.Controls.Add(label);
            gb.AutoSize = true;
            GroupBox = gb;
            return gb;
        }

        public bool CheckAnswer()
        {
            var input = GroupBox.Controls.OfType<TextBox>().First().Text;
            if (string.Equals(input, Answer, StringComparison.CurrentCultureIgnoreCase)) return true;
            MessageBox.Show($"Правильные варианты: {Answer}");
            CreateQuestion();
            return false;

        }

        public static List<IQuestion> ParseQuestions(string fileName)
        {
            var sr = new StreamReader(fileName);
            var result = new List<IQuestion>();
            var current = new OrderQuestions();
            var line = sr.ReadLine();
            current.Question = line;
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (line[0] == '#')
                {
                    result.Add(current);
                    current = new OrderQuestions();
                    current.Question = line;
                    continue;
                }

                if (line[0] == '+')
                {
                    current.Answer = new string(line.Skip(1).ToArray());
                }
                else
                {
                    current.Question += '\n' + line;
                }
            }
            result.Add(current);
            return result;
        }
        static void ButtonClick(object sender)
        {
            var button = sender as Button;
            button.Text = button.AccessibleDescription;
            //button.Enabled = false;
        }
    }
}