using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cisco
{
    public partial class Form1 : Form
    {
        private List<QuestionClass> All;
        private GroupBox currentGb;
        private QuestionClass currentQuestion;
        private HashSet<int> PassedQuestions = new HashSet<int>();
        private Random rnd = new Random(DateTime.Now.Millisecond);
        public Form1()
        {
            InitializeComponent();
            
            List<QuestionClass> Solid = FileParser.ParseQuestions(QuestionType.Single,"singleQuestions.txt");
            List<QuestionClass> Multi = FileParser.ParseQuestions(QuestionType.Multi,"multiQuestions.txt");
            All = Solid;
            All.AddRange(Multi);
            CreateQuestion();
        }

        void ButtonClick()
        {
            if (currentQuestion.QuestionType == QuestionType.Multi)
            {
                var checkboxes = currentGb.Controls.OfType<CheckBox>().Where(x => x.Checked).Select(x => x.Text);
                var count = 0;
                foreach (var elem in checkboxes)
                {
                    if (!currentQuestion.GoodAnswers.Contains(elem))
                    {
                        MessageBox.Show($"Правильные варианты:\n{string.Join('\n', currentQuestion.GoodAnswers)}");
                        CreateQuestion();
                        return;
                    }

                    count++;
                }

                if (currentQuestion.GoodAnswers.Count != count)
                {
                    MessageBox.Show($"Правильные варианты:\n{string.Join('\n', currentQuestion.GoodAnswers)}");
                    CreateQuestion();
                    return;
                }
            }
            else
            {
                var checkboxes = currentGb.Controls.OfType<RadioButton>().Where(x => x.Checked).Select(x => x.Text);
                foreach (var elem in checkboxes)
                {
                    if (!currentQuestion.GoodAnswers.Contains(elem))
                    {
                        MessageBox.Show($"Правильные варианты:\n{string.Join('\n', currentQuestion.GoodAnswers)}");
                        CreateQuestion();
                        return;
                    }
                }
            }

            MessageBox.Show($"Правильный ответ!");
            CreateQuestion();
        }

        void CreateQuestion()
        {
            Controls.Clear();
            var index = GetNextIndex();
            if (index == -1)
            {
                MessageBox.Show("Вы прошли все вопросы!");
                PassedQuestions.Clear();
                index = GetNextIndex();
            }
            var gb = QuestionFormatter.GetGroupBox(All[index]);
            currentGb = gb;
            currentQuestion = All[index];
            Controls.Add(gb);
            var button = new Button
            {
                AutoSize = true,
                Text = "Ok",
                Location = new Point(450,450)
            };
            button.Click += (s, e) =>
            {
                ButtonClick();
            };
            Controls.Add(button);
        }

        int GetNextIndex()
        {
            if (PassedQuestions.Count == All.Count)
                return -1;
            var index = rnd.Next(0, All.Count);
            while (PassedQuestions.Contains(index))
            {
                index = rnd.Next(0, All.Count);
            }

            PassedQuestions.Add(index);
            return index;
        }
    }
}