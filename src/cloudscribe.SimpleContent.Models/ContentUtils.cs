using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace cloudscribe.SimpleContent.Models
{
    public class ContentUtils
    {
        public static string CreateSlug(string title)
        {
            title = title.ToLowerInvariant().Replace(" ", "-");
            title = RemoveDiacritics(title);
            title = RemoveReservedUrlCharacters(title);

            return title.ToLowerInvariant();
        }

        public static string RemoveDiacritics(string text)
        {
            //var normalizedString = text.Normalize(NormalizationForm.FormD); // not available in dnxcore50
            var normalizedString = text;
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            //return stringBuilder.ToString().Normalize(NormalizationForm.FormC); // not available in dnxcore50
            return stringBuilder.ToString();
        }

        public static string RemoveReservedUrlCharacters(string text)
        {
            var reservedCharacters = new List<string>() { "!", "#", "$", "&", "'", "(", ")", "*", ",", "/", ":", ";", "=", "?", "@", "[", "]", "\"", "%", ".", "<", ">", "\\", "^", "_", "'", "{", "}", "|", "~", "`", "+" };

            foreach (var chr in reservedCharacters)
            {
                text = text.Replace(chr, "");
            }

            return text;
        }

    }
}
