using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cisco
{
    public static class FileParser
    {
        public static List<QuestionClass> ParseQuestions(QuestionType type,string fileName)
        {
            var result = new List<QuestionClass>();
            var sr = new StreamReader(fileName);
            QuestionClass currentQuestion = null;
            List<string> good = new List<string>();
            List<string> bad = new List<string>();
            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();
                if (line[0] != '#')
                {
                    if(line[0] == '+')
                        good.Add(new string( line.Skip(1).ToArray()));
                    else
                        bad.Add(new string( line.Skip(1).ToArray()));
                }
                else
                {
                    currentQuestion = new QuestionClass(new string( line.Skip(1).ToArray()), good, bad, type);
                    result.Add(currentQuestion);
                    good.Clear();
                    bad.Clear();
                }
            }

            return result;
        }
    }
}