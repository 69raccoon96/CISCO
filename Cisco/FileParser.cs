using System.Collections.Generic;
using System.Drawing;
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
            var currentQ = "";
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
                    
                    if (good.Count != 0 && bad.Count != 0)
                    {
                        currentQuestion = new QuestionClass(currentQ, good, bad, type);
                        result.Add(currentQuestion);
                        good = new List<string>();
                        bad = new List<string>();
                    }
                    currentQ = new string(line.Skip(1).ToArray());
                }
            }
            currentQuestion = new QuestionClass(currentQ, good, bad, type);
            result.Add(currentQuestion);

            return result;
        }
        public static List<QuestionClass> ParseQuestionsWithImage(QuestionType type,string fileName)
        {
            var result = new List<QuestionClass>();
            var sr = new StreamReader(fileName);
            QuestionClass currentQuestion = null;
            List<string> good = new List<string>();
            List<string> bad = new List<string>();
            var currentQ = "";
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
                    
                    if (good.Count != 0 && bad.Count != 0)
                    {
                        currentQuestion = new QuestionClass(currentQ, good, bad, type);
                        result.Add(currentQuestion);
                        good = new List<string>();
                        bad = new List<string>();
                        var imageName = new string(currentQ.Split('.')[0].Skip(1).ToArray());
                        currentQuestion.Image = new Bitmap($"images\\{imageName}.png");
                    }
                    currentQ = new string(line);
                }
            }
            currentQuestion = new QuestionClass(currentQ, good, bad, type);
            result.Add(currentQuestion);
            var imageName1 = new string(currentQ.Split('.')[0].Skip(1).ToArray());
            currentQuestion.Image = new Bitmap($"images\\{imageName1}.png");

            return result;
        }
    }
}