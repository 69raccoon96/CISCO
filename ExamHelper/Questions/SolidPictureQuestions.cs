using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ExamHelper.Questions
{
    public class SolidPictureQuestions : IQuestion
    {
        public string Question { get; set; }
        private readonly List<string> _correctAnswers;
        private readonly List<string> _inCorrectAnswers;
        private GroupBox _groupBox;
        private Bitmap _image;

        private SolidPictureQuestions(string question, List<string> correctAnswers, List<string> inCorrectAnswers)
        {
            Question = question;
            _correctAnswers = correctAnswers;
            _inCorrectAnswers = inCorrectAnswers;
        }

        public GroupBox CreateQuestion()
        {
            var gb = new GroupBox();
            gb.Location = new Point(0, 0);
            gb.Size = new Size(1000, 400);
            var mixedQuestions = new List<string>();
            mixedQuestions.AddRange(_correctAnswers);
            mixedQuestions.AddRange(_inCorrectAnswers);
            var text = Utilities.SplitToLines(Question, 150);
            var label = new Label { Text = text, Location = new Point(10, 10) };
            label.AutoSize = true;
            var rnd = new Random(DateTime.Now.Millisecond);
            var hs = new HashSet<int>();
            const int x = 10;
            var y = label.Location.Y + label.Size.Height + 50;
            var pb = new PictureBox();
            pb.Image = _image;
            pb.Location = new Point(x, y);
            pb.Size = new Size(500, 200);
            pb.SizeMode = PictureBoxSizeMode.StretchImage;
            y = pb.Location.Y + pb.Size.Height + 50;
            gb.Controls.Add(pb);
            for (var i = 0; i < mixedQuestions.Count; i++)
            {
                var index = rnd.Next(0, mixedQuestions.Count);
                if (hs.Contains(index))
                {
                    i--;
                    continue;
                }

                var size = 0;
                var answer = Utilities.SplitToLines(mixedQuestions[index], 150);
                var radioButton1 = new RadioButton()
                    { Text = answer, Location = new Point(x, y), AutoSize = true };
                radioButton1.AccessibleDescription = mixedQuestions[index];
                size += radioButton1.Size.Height;
                gb.Controls.Add(radioButton1);
                hs.Add(index);
                y += size + 30;
            }

            gb.Controls.Add(label);
            gb.AutoSize = true;
            _groupBox = gb;
            return gb;
        }

        public bool CheckAnswer()
        {
            var checkboxes = _groupBox.Controls
                .OfType<RadioButton>()
                .Where(x => x.Checked)
                .Select(x => x.AccessibleDescription)
                .ToList();
            
            if (!checkboxes.Any())
            {
                ShowCorrect();
                MessageBox.Show("Неправильно");
                return false;
            }

            foreach (var elem in checkboxes)
            {
                if (!_correctAnswers.Contains(elem))
                {
                    ShowCorrect();
                    MessageBox.Show("Неправильно");
                    return false;
                }
            }

            return true;
        }

        private void ShowCorrect()
        {
            var checkboxes = _groupBox.Controls.OfType<RadioButton>();
            foreach (var checkbox in checkboxes)
            {
                if (_inCorrectAnswers.Contains(checkbox.AccessibleDescription))
                {
                    if (checkbox.Checked)
                    {
                        checkbox.BackColor = Color.FromArgb(233, 153, 152);
                    }
                }

                if (_correctAnswers.Contains(checkbox.AccessibleDescription))
                {
                    checkbox.BackColor = Color.FromArgb(182, 215, 168);
                }
            }
        }

        public static List<IQuestion> ParseQuestions(string fileName)
        {
            var result = new List<IQuestion>();
            var sr = new StreamReader(fileName);
            SolidPictureQuestions currentQuestion;
            var good = new List<string>();
            var bad = new List<string>();
            var currentQ = "";
            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();
                if (line == "")
                    break;
                if (line![0] != '#')
                {
                    if (line[0] == '+')
                        good.Add(new string(line.Skip(1).ToArray()));
                    else
                        bad.Add(new string(line.Skip(1).ToArray()));
                }
                else
                {
                    if (good.Count != 0 && bad.Count != 0)
                    {
                        currentQuestion = new SolidPictureQuestions(currentQ, good, bad);
                        result.Add(currentQuestion);
                        good = new List<string>();
                        bad = new List<string>();
                        var imageName = new string(currentQ.Split('.')[0].Skip(1).ToArray());
                        currentQuestion._image = new Bitmap($"QuestionsData\\images\\{imageName}.png");
                    }

                    currentQ = new string(line);
                }
            }

            try
            {
                currentQuestion = new SolidPictureQuestions(currentQ, good, bad);
                result.Add(currentQuestion);
                var imageName1 = new string(currentQ.Split('.')[0].Skip(1).ToArray());
                currentQuestion._image = new Bitmap($"QuestionsData\\images\\{imageName1}.png");
            }
            catch
            {
                return new List<IQuestion>();
            }

            return result;
        }
    }
}