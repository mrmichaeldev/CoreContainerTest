using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService1
{
    public static class Utils
    {
        public static bool CheckContains(string check, string valList)
        {
            if (String.IsNullOrEmpty(check) || String.IsNullOrEmpty(valList))
                return false;

            var list = valList.Split(',').ToList();
            return list.Contains(check);
        }

        public static bool RegexMatch(string text, string expression)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(text, expression);
        }
    }
}
