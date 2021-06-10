using System.Collections.Generic;
using System.Windows.Forms;

namespace ExamHelper
{
    public interface IQuestion
    {
        public string Question { get; set; }
        public GroupBox CreateQuestion();
        public bool CheckAnswer();
    }
}