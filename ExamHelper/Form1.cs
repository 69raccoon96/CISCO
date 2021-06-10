using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ExamHelper;

namespace ExamHelper
{
    public partial class Form1 : Form
    {
        private List<IQuestion> All;
        private IQuestion currentQuestion;
        private HashSet<int> PassedQuestions = new HashSet<int>();
        private Random rnd = new Random(DateTime.Now.Millisecond);
        private int correct = 0;
        public Form1()
        {
            string multi, single,pictureSingle,pictureMulti, input, order;
            InitializeComponent();
            single = "singleQuestions_ru.txt";
            multi = "multiQuestions_ru.txt";
            pictureSingle = "singlePictureQuestions_ru.txt";
            pictureMulti = "multiPictureQuestions_ru.txt";
            input = "inputQuestions_ru.txt";
            order = "orderQuestions_ru.txt";
            var Solid = SolidQuestion.ParseQuestions(single);
            var Multi = MultiQuestion.ParseQuestions(multi);
            var Input = InputQuestion.ParseQuestions(input);
            var solidPicture = SolidPictureQuestions.ParseQuestions(pictureSingle);
            var multiPicture = MultiPictureQuestions.ParseQuestions(pictureMulti);
            var Order = OrderQuestions.ParseQuestions(order);
            All = Solid;
            All.AddRange(Multi);
            All.AddRange(Input);
            All.AddRange(solidPicture);
            All.AddRange(multiPicture);
            All.AddRange(Order);
            InputQuestion.answerCallback = ButtonClick;
            OrderQuestions.answerCallback = ButtonClick;
            CreateQuestion();
            
        }
        void ButtonClick()
        {
            if(currentQuestion.CheckAnswer())
                correct++;
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
            currentQuestion = All[index];
            var gb = currentQuestion.CreateQuestion();
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
            var location2 = Point.Add(button.Location, new Size(20, 0));
            location2.X += button.Size.Width;
            var num2 = Math.Round(correct * 1.0 / (PassedQuestions.Count - 1.0), 2) * 100.0;
            var value = new Label
            {
                Location = location2,
                Text = string.Format("Пройдено вопросов {0} из {1}, правильно:{2}%", PassedQuestions.Count - 1, All.Count - 1, num2),
                AutoSize = true
            };
            Controls.Add(value);
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