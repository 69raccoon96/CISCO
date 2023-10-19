using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ExamHelper.Questions;

namespace ExamHelper
{
    public partial class Form1 : Form
    {
        private readonly List<IQuestion> _all;
        private IQuestion _currentQuestion;
        private readonly HashSet<int> _passedQuestions = new();
        private readonly Random _rnd = new(DateTime.Now.Millisecond);
        private int _correct;
        private const string Single = "QuestionsData/singleQuestions_ru.txt";
        private const string Multi = "QuestionsData/multiQuestions_ru.txt";
        private const string PictureSingle = "QuestionsData/singlePictureQuestions_ru.txt";
        private const string PictureMulti = "QuestionsData/multiPictureQuestions_ru.txt";
        private const string Input = "QuestionsData/inputQuestions_ru.txt";

        public Form1()
        {
            InitializeComponent();
            var files = new List<string>
            {
                Single, 
                Multi, 
                Input, 
                PictureSingle, 
                PictureMulti
            };
            var parsers = new List<Func<string, List<IQuestion>>>
            {
                SolidQuestion.ParseQuestions,
                MultiQuestion.ParseQuestions,
                InputQuestion.ParseQuestions,
                SolidPictureQuestions.ParseQuestions,
                MultiPictureQuestions.ParseQuestions,
            };

            _all = files.SelectMany((x, i) => parsers[i](x)).ToList();


            InputQuestion.AnswerCallback = ButtonClick;
            CreateQuestion();
        }

        private void ButtonClick()
        {
            if (_currentQuestion.CheckAnswer())
                _correct++;
            CreateQuestion();
        }

        private void CreateQuestion()
        {
            Controls.Clear();
            var index = GetNextIndex();
            if (index == -1)
            {
                var percent = Math.Round(_correct * 1.0 / _all.Count, 2) * 100;
                MessageBox.Show($"Количество правильных ответов: {_correct}\n" +
                                $"Процент правильных ответов: {percent}%", "Вы прошли все вопросы!");
                _passedQuestions.Clear();
                index = GetNextIndex();
                _correct = 0;
            }

            _currentQuestion = _all[index];
            var gb = _currentQuestion.CreateQuestion();
            Controls.Add(gb);
            var p = gb.Location with { Y = gb.Location.Y + gb.Size.Height + 10 };
            var button = new Button
            {
                AutoSize = true,
                Text = "Ok",
                Location = p
            };
            button.Click += (_, _) => { ButtonClick(); };
            Controls.Add(button);
            var location2 = Point.Add(button.Location, new Size(20, 0));
            location2.X += button.Size.Width;
            var num2 = Math.Round(_correct * 1.0 / (_passedQuestions.Count - 1.0), 2) * 100.0;
            var value = new Label
            {
                Location = location2,
                Text = $"Пройдено вопросов {_passedQuestions.Count - 1} из {_all.Count - 1}, правильно:{num2}%",
                AutoSize = true
            };
            Controls.Add(value);
        }

        private int GetNextIndex()
        {
            if (_passedQuestions.Count == _all.Count)
                return -1;
            var index = _rnd.Next(0, _all.Count);
            while (_passedQuestions.Contains(index))
            {
                index = _rnd.Next(0, _all.Count);
            }

            _passedQuestions.Add(index);
            return index;
        }
    }
}