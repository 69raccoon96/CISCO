using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ExamHelper.Questions
{
    public class InputQuestion : IQuestion
    {
        public string Question { get; set; }
        private readonly string _answer;
        private GroupBox _groupBox;
        public static Action AnswerCallback;

        private InputQuestion(string question, string answer)
        {
            Question = question;
            _answer = answer;
        }

        public GroupBox CreateQuestion()
        {
            var gb = new GroupBox { Location = new Point(0, 0), Size = new Size(1000, 400) };
            var text = Utilities.SplitToLines(Question, 150);
            var label = new Label { Text = text, Location = new Point(10, 10), AutoSize = true };
            const int x = 10;
            var y = label.Location.Y + label.Size.Height + 50;
            var input = new TextBox
            {
                Location = new Point(x, y), AutoSize = true
            };
            input.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                    AnswerCallback();
            };
            gb.Controls.Add(input);
            y += 30;
            var hint = new Button
            {
                Location = new Point(x, y),
                Text = "Подсказка",
                AccessibleDescription = _answer,
                AutoSize = true
            };
            hint.Click += (s, e) => { ButtonClick(s); };
            gb.Controls.Add(hint);

            gb.Controls.Add(label);
            gb.AutoSize = true;
            _groupBox = gb;
            return gb;
        }

        public bool CheckAnswer()
        {
            var input = _groupBox.Controls.OfType<TextBox>().First().Text;
            if (string.Equals(input, _answer, StringComparison.CurrentCultureIgnoreCase)) return true;
            MessageBox.Show($"Правильные варианты: {_answer}");
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
                if (question == "")
                    break;
                var answer = new string(sr.ReadLine()!.Skip(1).ToArray());
                var current = new InputQuestion(question, answer);
                result.Add(current);
            }

            return result;
        }

        private static void ButtonClick(object sender)
        {
            var button = sender as Button;
            button!.Text = button.AccessibleDescription;
            //button.Enabled = false;
        }
    }
}