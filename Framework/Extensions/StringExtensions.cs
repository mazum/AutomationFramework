using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Framework.Extensions
{
    public static class StringExtensions
    {
        public static string GetRegExMatch(this string text, string pattern)
        {
            var result = "";
            var regex = new Regex(pattern, RegexOptions.Singleline);
            var match = regex.Match(text);
            if (match.Success)
                result = match.Value;
            return result;
        }

        public static string GetRegExCaptureGroup(this string text, string
            pattern, int groupIndex = 1)
        {
            var result = "";
            var regex = new Regex(pattern, RegexOptions.Singleline);
            var match = regex.Match(text);
            if (match.Success)
                result = match.Groups[groupIndex].Value;
            return result;
        }

        public static List<string> GetRegExCaptureGroups(this string text, string pattern)
        {
            var result = new List<string>();
            var regex = new Regex(pattern, RegexOptions.Singleline);
            var match = regex.Match(text);
            if (match.Success)
                foreach (Match group in match.Groups)
                {
                    result.Add(group.Value);
                }
            return result;
        }

        public static string ReplaceRegExMatch(this string text, string pattern, string replacementText)
        {
            return Regex.Replace(text, pattern, replacementText, RegexOptions.Singleline);
        }

        public static bool IsRegExMatch(this string text, string pattern)
        {
            var regex = new Regex(pattern, RegexOptions.Singleline);
            return regex.IsMatch(text);
        }

        public static int GetRegExMatchCount(this string text, string pattern)
        {
            var regex = new Regex(pattern, RegexOptions.Singleline);
            return regex.Matches(text).Count;
        }

        public static string ToTitleCase(this string text)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text.ToLower());
        }

        public static string ToUppercaseFirst(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            return char.ToUpper(text[0]) + text.Substring(1).ToLower();
        }

        public static bool IsNullOrEmpty(this string text)
        {
            return string.IsNullOrEmpty(text);
        }

        public static bool IsNotNullOrEmpty(this string text)
        {
            return !string.IsNullOrEmpty(text);
        }

        public static bool IsNullOrWhiteSpace(this string text)
        {
            return text == null || text.IsRegExMatch(@"^\s+$");
        }

        public static bool IsNegative(this string text)
        {
            return text.StartsWith("-");
        }
    }
}