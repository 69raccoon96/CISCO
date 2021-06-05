using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ExamHelper
{
    public class InputQuestion : IQuestion
    {
        private string Question;
        private string Answer;
        private GroupBox GroupBox;
        public static Action answerCallback;

        public InputQuestion(string question, string answer)
        {
            Question = question;
            Answer = answer;
        }
        public GroupBox CreateQuestion()
        {
            var gb = new GroupBox {Location = new Point(0, 0), Size = new Size(1000, 400)};
            var text = Utilities.SplitToLines(Question, 150);
            var label = new Label {Text = text, Location = new Point(10, 10), AutoSize = true};
            const int x = 10;
            var y = label.Location.Y + label.Size.Height + 50;
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
            var result = new List<IQuestion>();
            var sr = new StreamReader(fileName);
            while (!sr.EndOfStream)
            {
                var question = sr.ReadLine();
                var answer = new string(sr.ReadLine().Skip(1).ToArray());
                var current = new InputQuestion(question, answer);
                result.Add(current);
            }

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