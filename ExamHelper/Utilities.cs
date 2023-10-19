using System.Text;

namespace ExamHelper
{
    public static class Utilities
    {
        public static string SplitToLines(string str, int n)
        {
            var sb = new StringBuilder(str.Length + (str.Length + 9) / 10);

            for (var q = 0; q < str.Length;)
            {
                sb.Append(str[q]);

                if (++q % n == 0)
                    sb.AppendLine();
            }

            if (str.Length % n == 0)
                --sb.Length;

            return sb.ToString();
        }
    }
}