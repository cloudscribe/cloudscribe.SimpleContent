using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace cloudscribe.ContentUtils
{
    public static class SlugHelper
    {
        public static string CreateSlug(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) { return title; }

            title = title.ToLowerInvariant().Replace("  ", " ")
                .Replace(" ", "-")
                .Replace("--", "-")
                .Replace("--", "-")
                .Replace("\n", string.Empty)
                .Replace("\r", string.Empty)
                .Replace("\t", string.Empty)
                ;
            title = RemoveDiacritics(title);
            title = RemoveReservedUrlCharacters(title);

            return title.ToLowerInvariant().Trim();
        }

        private static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) { return text; }

            var normalizedString = text.Normalize(NormalizationForm.FormD);

            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);

        }

        private static string RemoveReservedUrlCharacters(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) { return text; }

            var reservedCharacters = new List<string>() { "!", "#", "$", "&", "'", "(", ")", "*", ",", "/", ":", ";", "=", "?", "@", "[", "]", "\"", "%", ".", "<", ">", "\\", "^", "_", "'", "{", "}", "|", "~", "`", "+" };

            foreach (var chr in reservedCharacters)
            {
                text = text.Replace(chr, "");
            }

            return text;
        }

    }
}
