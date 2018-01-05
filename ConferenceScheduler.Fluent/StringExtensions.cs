using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ConferenceScheduler
{
    public static class StringExtensions
    {
        public static string RemoveWhitespace(this string source)
        {
            return Regex.Replace(source, @"\s", "");
        }
    }
}
