using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ExamHelper
{
    public class MultiPictureQuestions : IQuestion
    {
        public string Question { get; set; }
        private List<string> CorrectAnswers;
        private List<string> InCorrectAnswers;
        private GroupBox GroupBox;
        private Bitmap Image;
        public MultiPictureQuestions(string question, List<string> correctAnswers, List<string> inCorrectAnswers)
        {
            Question = question;
            CorrectAnswers = correctAnswers;
            InCorrectAnswers = inCorrectAnswers;
        }
        public GroupBox CreateQuestion()
        {
            var gb = new GroupBox();
            gb.Location = new Point(0, 0);
            gb.Size = new Size(1000, 400);
            var mixedQuestions = new List<string>();
            mixedQuestions.AddRange(CorrectAnswers);
            mixedQuestions.AddRange(InCorrectAnswers);
            var text = Utilities.SplitToLines(Question, 150);
            var label = new Label {Text = text, Location = new Point(10, 10)};
            label.AutoSize = true;
            var rnd = new Random(DateTime.Now.Millisecond);
            var hs = new HashSet<int>();
            const int x = 10;
            var y = label.Location.Y + label.Size.Height + 50;
            var pb = new PictureBox();
            pb.Image = Image;
            pb.Location = new Point(x,y);
            pb.Size = new Size(500,200);
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
                var answer = Utilities.SplitToLines(mixedQuestions[index],150);
                var checkBox1 = new CheckBox
                    {Text = answer, Location = new Point(x, y), AutoSize = true};
                checkBox1.AccessibleDescription = mixedQuestions[index];
                size += checkBox1.Size.Height;
                gb.Controls.Add(checkBox1);
                hs.Add(index);
                y += size + 30;
            }

            gb.Controls.Add(label);
            gb.AutoSize = true;
            GroupBox = gb;
            return gb;
        }

        public bool CheckAnswer()
        {
            var checkboxes = GroupBox.Controls.OfType<CheckBox>().Where(x => x.Checked).Select(x => x.AccessibleDescription);
            var count = 0;
            foreach (var elem in checkboxes)
            {
                if (!CorrectAnswers.Contains(elem))
                {
                    ShowCorrect();
                    MessageBox.Show("Неправильно");
                    return false;
                }

                count++;
            }

            if (CorrectAnswers.Count != count)
            {
                ShowCorrect();
                MessageBox.Show("Неправильно");
                return false;
            }
            return true;
        }
        public void ShowCorrect()
        {
            var checkboxes =  GroupBox.Controls.OfType<CheckBox>();
            foreach (var checkbox in checkboxes)
            {
                if (InCorrectAnswers.Contains(checkbox.AccessibleDescription))
                {
                    if (checkbox.Checked)
                    {
                        checkbox.BackColor = Color.FromArgb(233,153,152);
                    }
                }

                if (CorrectAnswers.Contains(checkbox.AccessibleDescription))
                {
                    checkbox.BackColor = Color.FromArgb(182, 215, 168);
                }
            }
        }

        public static List<IQuestion> ParseQuestions(string fileName)
        {
            var result = new List<IQuestion>();
            var sr = new StreamReader(fileName);
            MultiPictureQuestions currentQuestion = null;
            List<string> good = new List<string>();
            List<string> bad = new List<string>();
            var currentQ = "";
            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();
                if (line[0] != '#')
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
                        currentQuestion = new MultiPictureQuestions(currentQ, good, bad);
                        result.Add(currentQuestion);
                        good = new List<string>();
                        bad = new List<string>();
                        var imageName = new string(currentQ.Split('.')[0].Skip(1).ToArray());
                        currentQuestion.Image = new Bitmap($"images\\{imageName}.png");
                    }

                    currentQ = new string(line);
                }
            }

            try
            {
                currentQuestion = new MultiPictureQuestions(currentQ, good, bad);
                result.Add(currentQuestion);
                var imageName1 = new string(currentQ.Split('.')[0].Skip(1).ToArray());
                currentQuestion.Image = new Bitmap($"images\\{imageName1}.png");
            }
            catch
            {
                return new List<IQuestion>();
            }

            return result;
        }
    }
}