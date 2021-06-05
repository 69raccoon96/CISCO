using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cisco
{
    public static class QuestionFormatter
    {
        public static GroupBox GetGroupBox(QuestionClass questionClass)
        {
            var gb = new GroupBox();
            gb.Location = new Point(0, 0);
            gb.Size = new Size(1000, 400);
            var mixedQuestions = new List<string>();
            mixedQuestions.AddRange(questionClass.GoodAnswers);
            mixedQuestions.AddRange(questionClass.BadAnswers);
            var text = SplitToLines(questionClass.Question, 150);
            var label = new Label {Text = text, Location = new Point(10, 10)};
            label.AutoSize = true;
            var rnd = new Random(DateTime.Now.Millisecond);
            var hs = new HashSet<int>();
            const int x = 10;
            var y = label.Location.Y + label.Size.Height + 50;
            if (questionClass.QuestionType == QuestionType.PictureMulti ||
                questionClass.QuestionType == QuestionType.PictureSingle)
            {
                var pb = new PictureBox();
                pb.Image = questionClass.Image;
                pb.Location = new Point(x,y);
                pb.Size = new Size(500,200);
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                y = pb.Location.Y + pb.Size.Height + 50;
                gb.Controls.Add(pb);
            }
            for (var i = 0; i < mixedQuestions.Count; i++)
            {
                var index = rnd.Next(0, mixedQuestions.Count);
                if (hs.Contains(index))
                {
                    i--;
                    continue;
                }

                var size = 0;
                var answer = SplitToLines(mixedQuestions[index],150);
                if (questionClass.QuestionType == QuestionType.Single || questionClass.QuestionType == QuestionType.PictureSingle)
                {
                    var radioButton1 = new RadioButton()
                        {Text = answer, Location = new Point(x, y), AutoSize = true};
                    radioButton1.AccessibleDescription = mixedQuestions[index];
                    size += radioButton1.Size.Height;
                    gb.Controls.Add(radioButton1);
                    hs.Add(index);
                }
                if(questionClass.QuestionType == QuestionType.PictureMulti || questionClass.QuestionType == QuestionType.Multi)
                {
                    var checkBox1 = new CheckBox
                        {Text = answer, Location = new Point(x, y), AutoSize = true};
                    checkBox1.AccessibleDescription = mixedQuestions[index];
                    size += checkBox1.Size.Height;
                    gb.Controls.Add(checkBox1);
                    hs.Add(index);
                }

                if (questionClass.QuestionType == QuestionType.Input)
                {
                    var input = new TextBox
                    {
                        Location = new Point(x, y), AutoSize = true
                    };
                    size += input.Size.Height;
                    gb.Controls.Add(input);
                    y += 30;
                    var hint = new Button
                    {
                        Location = new Point(x,y),
                        Text = "Подсказка",
                        AccessibleDescription = questionClass.GoodAnswers[0],
                        AutoSize = true
                    };
                    hint.Click += (s, e) =>
                    {
                        ButtonClick(s);
                    };
                    gb.Controls.Add(hint);
                }

                y += size + 10;
            }

            gb.Controls.Add(label);
            gb.AutoSize = true;
            return gb;
        }
        public static string SplitToLines(string str, int n)
        {
            var sb = new StringBuilder(str.Length + (str.Length + 9) / 10);

            for (int q=0; q<str.Length; )
            {
                sb.Append(str[q]);

                if (++q % n == 0)
                    sb.AppendLine();
            }

            if (str.Length % n == 0)
                --sb.Length;

            return sb.ToString();
        }

        static void ButtonClick(object sender)
        {
            var button = sender as Button;
            button.Text = button.AccessibleDescription;
            //button.Enabled = false;
        }
    }
}