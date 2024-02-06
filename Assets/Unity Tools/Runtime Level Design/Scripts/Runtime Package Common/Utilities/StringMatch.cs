using System.Collections.Generic;

namespace RLD
{
    public static class StringMatch
    {
        public enum Case
        {
            Sensitive = 0,
            Insensitive,
        }

        public static void Match(List<string> strings, string stringToMatch, Case matchCase, List<string> matches)
        {
            matches.Clear();
            if (strings.Count == 0) return;

            if (matchCase == Case.Insensitive)
            {
                string lowerToMatch = stringToMatch.ToLower();
                foreach(var str in strings)
                {
                    string lowerCase = str.ToLower();
                    if (lowerCase.Contains(lowerToMatch)) matches.Add(str);
                }
            }
            else
            {
                foreach (var str in strings)
                {
                    if (str.Contains(stringToMatch)) matches.Add(str);
                }
            }
        }
    }
}