using System.Collections.Generic;
using System.Windows.Forms;

namespace ExamHelper
{
    public interface IQuestion
    {
        public GroupBox CreateQuestion();
        public bool CheckAnswer();
        
    }
}