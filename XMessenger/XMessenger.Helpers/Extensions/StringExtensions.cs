using System.Globalization;

namespace XMessenger.Helpers.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrWhiteSpace(this string s) 
        {
            return string.IsNullOrWhiteSpace(s);
        }

        public static string ToUpperFirstLatter(this string word)
        {
            string firstLetter = word.Substring(0, 1).ToUpper(CultureInfo.InvariantCulture);
            string restOfWord = word.Substring(1).ToLower(CultureInfo.InvariantCulture);
            return firstLetter + restOfWord;
        }
    }
}
