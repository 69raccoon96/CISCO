using System;
using System.Collections.Generic;
using System.Drawing;
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
            var label = new Label {Text = questionClass.Question, Location = new Point(10, 10)};
            label.AutoSize = true;
            var rnd = new Random(DateTime.Now.Millisecond);
            var hs = new HashSet<int>();
            const int x = 10;
            var y = 50;
            for (var i = 0; i < mixedQuestions.Count; i++)
            {
                var index = rnd.Next(0, mixedQuestions.Count);
                if (hs.Contains(index))
                {
                    i--;
                    continue;
                }

                if (questionClass.QuestionType == QuestionType.Single)
                {
                    var checkbox = new RadioButton()
                        {Text = mixedQuestions[index], Location = new Point(x, y), AutoSize = true};

                    y += 50;
                    gb.Controls.Add(checkbox);
                    hs.Add(index);
                }
                else
                {
                    var checkbox = new CheckBox
                        {Text = mixedQuestions[index], Location = new Point(x, y), AutoSize = true};

                    y += 50;
                    gb.Controls.Add(checkbox);
                    hs.Add(index);
                    
                }
            }

            gb.Controls.Add(label);
            return gb;
        }
    }
}