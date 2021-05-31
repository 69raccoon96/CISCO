using System.Collections.Generic;
using System.Drawing;

namespace Cisco
{
    public class QuestionClass
    {
        public string Question;
        public List<string> GoodAnswers;
        public List<string> BadAnswers;
        public QuestionType QuestionType;
        public Bitmap Image { get; set; }

        public QuestionClass(string question, List<string> goodAnswers, List<string> badAnswers, QuestionType questionType)
        {
            Question = question;
            GoodAnswers = goodAnswers;
            BadAnswers = badAnswers;
            QuestionType = questionType;
        }
    }
}