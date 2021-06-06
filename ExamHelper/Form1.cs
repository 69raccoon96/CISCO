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
            string multi, single,pictureSingle,pictureMulti, input;
            InitializeComponent();
            single = "singleQuestions_ru.txt";
            multi = "multiQuestions_ru.txt";
            pictureSingle = "singlePictureQuestions_ru.txt";
            pictureMulti = "multiPictureQuestions_ru.txt";
            input = "inputQuestions_ru.txt";
            var Solid = SolidQuestion.ParseQuestions(single);
            var Multi = MultiQuestion.ParseQuestions(multi);
            var Input = InputQuestion.ParseQuestions(input);
            var solidPicture = SolidPictureQuestions.ParseQuestions(pictureSingle);
            var multiPicture = MultiPictureQuestions.ParseQuestions(pictureMulti);
            All = Solid;
            All.AddRange(Multi);
            All.AddRange(Input);
            All.AddRange(solidPicture);
            All.AddRange(multiPicture);
            InputQuestion.answerCallback = ButtonClick;
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