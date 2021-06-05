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
        private int correct = 0;
        public Form1()
        {
            string multi, single,pictureSingle,pictureMulti, input;
            InitializeComponent();
            single = "singleQuestions_ru.txt";
            multi = "multiQuestions_ru.txt";
            pictureSingle = "singlePictureQuestions_ru.txt";
            pictureMulti = "multiPictureQuestions_ru.txt";
            input = "inputQuestions_ru.txt";
            var Solid = FileParser.ParseQuestions(QuestionType.Single,single);
            var Multi = FileParser.ParseQuestions(QuestionType.Multi,multi);
            var Input = FileParser.ParseInputQuestions(QuestionType.Input, input);
            var solidPicture =
                FileParser.ParseQuestionsWithImage(QuestionType.PictureSingle, pictureSingle);
            var multiPicture =
                FileParser.ParseQuestionsWithImage(QuestionType.PictureMulti, pictureMulti);
            All = Solid;
            All.AddRange(Multi);
            All.AddRange(Input);
            All.AddRange(solidPicture);
            All.AddRange(multiPicture);
            CreateQuestion();
            
        }
        //TODO в нижний регистр
        void ButtonClick()
        {
            if (currentQuestion.QuestionType == QuestionType.Multi || currentQuestion.QuestionType == QuestionType.PictureMulti)
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
            if (currentQuestion.QuestionType == QuestionType.Single || currentQuestion.QuestionType == QuestionType.PictureSingle)
            {
                var checkboxes = currentGb.Controls.OfType<RadioButton>().Where(x => x.Checked).Select(x => x.AccessibleDescription);
                if (checkboxes.Count() == 0)
                {
                    MessageBox.Show($"Правильные варианты:\n{string.Join('\n', currentQuestion.GoodAnswers)}");
                    CreateQuestion();
                    return;
                }
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

            if (currentQuestion.QuestionType == QuestionType.Input)
            {
                var input = currentGb.Controls.OfType<TextBox>().First().Text;
                var answer = currentQuestion.GoodAnswers.First();
                if (input.ToLower() != answer.ToLower())
                {
                    MessageBox.Show($"Правильные варианты:\n{string.Join('\n', currentQuestion.GoodAnswers)}");
                    CreateQuestion();
                    return;
                }
            }

            correct++;
            MessageBox.Show($"Правильный ответ!");
            CreateQuestion();
        }

        void CreateQuestion()
        {
            Controls.Clear();
            var index = GetNextIndex();
            if (index == -1)
            {
                var percent = Math.Round(correct * 1.0 / All.Count, 2) * 100;
                MessageBox.Show($"Количество правильных ответов: {correct}\n" +
                                $"Процент правильных ответов: {percent}%","Вы прошли все вопросы!");
                PassedQuestions.Clear();
                index = GetNextIndex();
                correct = 0;
            }
            var gb = QuestionFormatter.GetGroupBox(All[index]);
            currentGb = gb;
            currentQuestion = All[index];
            Controls.Add(gb);
            var p = new Point(gb.Location.X, gb.Location.Y + gb.Size.Height + 10);
            var button = new Button
            {
                AutoSize = true,
                Text = "Ok",
                Location = p
            };
            button.Click += (s, e) =>
            {
                ButtonClick();
            };
            Controls.Add(button);
            var buttonLock = button.Location;
            var newLoc = Point.Add(buttonLock, new Size(20, 0));
            newLoc.X += button.Size.Width;
            var label = new Label
            {
                Location = newLoc,
                Text = $@"Пройдено вопросов {PassedQuestions.Count} из {All.Count}",
                AutoSize = true
                
            };
            Controls.Add(label);
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