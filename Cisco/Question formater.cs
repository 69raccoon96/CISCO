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
                if (questionClass.QuestionType == QuestionType.Single)
                {
                    var checkbox = new RadioButton()
                        {Text = answer, Location = new Point(x, y), AutoSize = true};
                    size += checkbox.Size.Height;
                    gb.Controls.Add(checkbox);
                    hs.Add(index);
                }
                else
                {
                    var checkbox = new CheckBox
                        {Text = answer, Location = new Point(x, y), AutoSize = true};
                    size += checkbox.Size.Height;
                    gb.Controls.Add(checkbox);
                    hs.Add(index);
                    
                }

                y += size + 10;
            }

            gb.Controls.Add(label);
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
    }
}