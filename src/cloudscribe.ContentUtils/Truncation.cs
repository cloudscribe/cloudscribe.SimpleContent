using System;
using System.Linq;

namespace cloudscribe.ContentUtils
{
    public static class Truncation
    {
        /// <summary>
        /// Truncate the string
        /// </summary>
        /// <param name="input">The string to be truncated</param>
        /// <param name="length">The length to truncate to</param>
        /// <returns>The truncated string</returns>
        public static string Truncate(this string input, int length)
        {
            return input.Truncate(length, "…", Truncator.FixedLength);
        }

        /// <summary>
        /// Truncate the string
        /// </summary>
        /// <param name="input">The string to be truncated</param>
        /// <param name="length">The length to truncate to</param>
        /// <param name="truncator">The truncate to use</param>
        /// <param name="from">The enum value used to determine from where to truncate the string</param>
        /// <returns>The truncated string</returns>
        public static string Truncate(this string input, int length, ITruncator truncator, TruncateFrom from = TruncateFrom.Right)
        {
            return input.Truncate(length, "…", truncator, from);
        }

        /// <summary>
        /// Truncate the string
        /// </summary>
        /// <param name="input">The string to be truncated</param>
        /// <param name="length">The length to truncate to</param>
        /// <param name="truncationString">The string used to truncate with</param>
        /// <param name="from">The enum value used to determine from where to truncate the string</param>
        /// <returns>The truncated string</returns>
        public static string Truncate(this string input, int length, string truncationString, TruncateFrom from = TruncateFrom.Right)
        {
            return input.Truncate(length, truncationString, Truncator.FixedLength, from);
        }

        /// <summary>
        /// Truncate the string
        /// </summary>
        /// <param name="input">The string to be truncated</param>
        /// <param name="length">The length to truncate to</param>
        /// <param name="truncationString">The string used to truncate with</param>
        /// <param name="truncator">The truncator to use</param>
        /// <param name="from">The enum value used to determine from where to truncate the string</param>
        /// <returns>The truncated string</returns>
        public static string Truncate(this string input, int length, string truncationString, ITruncator truncator, TruncateFrom from = TruncateFrom.Right)
        {
            if (truncator == null)
                throw new ArgumentNullException(nameof(truncator));

            if (input == null)
                return null;

            return truncator.Truncate(input, length, truncationString, from);
        }
    }

    /// <summary>
    /// Truncation location for humanizer
    /// </summary>
    public enum TruncateFrom
    {
        /// <summary>
        /// Truncate letters from the left (start) of the string
        /// </summary>
        Left,
        /// <summary>
        /// Truncate letters from the right (end) of the string
        /// </summary>
        Right
    }

    public enum ExcerptTruncationMode : byte
    {
        /// <summary>
        /// (Default) Truncate the post based on number of words.
        /// </summary>
        Word = 0,
        /// <summary>
        /// Truncate the post to a fixed length.
        /// </summary>
        Length,
        /// <summary>
        /// Truncate the post based on number of characters.
        /// </summary>
        Character
    }

    public interface ITruncator
    {
        /// <summary>
        /// Truncate a string
        /// </summary>
        /// <param name="value">The string to truncate</param>
        /// <param name="length">The length to truncate to</param>
        /// <param name="truncationString">The string used to truncate with</param>
        /// <param name="truncateFrom">The enum value used to determine from where to truncate the string</param>
        /// <returns>The truncated string</returns>
        string Truncate(string value, int length, string truncationString, TruncateFrom truncateFrom = TruncateFrom.Right);
    }

    public static class Truncator
    {
        /// <summary>
        /// Fixed length truncator
        /// </summary>
        public static ITruncator FixedLength
        {
            get
            {
                return new FixedLengthTruncator();
            }
        }

        /// <summary>
        /// Fixed number of characters truncator
        /// </summary>
        public static ITruncator FixedNumberOfCharacters
        {
            get
            {
                return new FixedNumberOfCharactersTruncator();
            }
        }

        /// <summary>
        /// Fixed number of words truncator
        /// </summary>
        public static ITruncator FixedNumberOfWords
        {
            get
            {
                return new FixedNumberOfWordsTruncator();
            }
        }
    }

    class FixedLengthTruncator : ITruncator
    {
        public string Truncate(string value, int length, string truncationString, TruncateFrom truncateFrom = TruncateFrom.Right)
        {
            if (value == null)
                return null;

            if (value.Length == 0)
                return value;

            if (truncationString == null || truncationString.Length > length)
                return truncateFrom == TruncateFrom.Right
                    ? value.Substring(0, length)
                    : value.Substring(value.Length - length);


            if (truncateFrom == TruncateFrom.Left)
                return value.Length > length
                    ? truncationString + value.Substring(value.Length - length + truncationString.Length)
                    : value;

            return value.Length > length
                ? value.Substring(0, length - truncationString.Length) + truncationString
                : value;
        }
    }

    class FixedNumberOfCharactersTruncator : ITruncator
    {
        public string Truncate(string value, int length, string truncationString, TruncateFrom truncateFrom = TruncateFrom.Right)
        {
            if (value == null)
                return null;

            if (value.Length == 0)
                return value;

            if (truncationString == null)
                truncationString = string.Empty;

            if (truncationString.Length > length)
                return truncateFrom == TruncateFrom.Right ? value.Substring(0, length) : value.Substring(value.Length - length);

            var alphaNumericalCharactersProcessed = 0;

            if (value.ToCharArray().Count(char.IsLetterOrDigit) <= length)
                return value;

            if (truncateFrom == TruncateFrom.Left)
            {
                for (var i = value.Length - 1; i > 0; i--)
                {
                    if (char.IsLetterOrDigit(value[i]))
                        alphaNumericalCharactersProcessed++;

                    if (alphaNumericalCharactersProcessed + truncationString.Length == length)
                        return truncationString + value.Substring(i);
                }
            }

            for (var i = 0; i < value.Length - truncationString.Length; i++)
            {
                if (char.IsLetterOrDigit(value[i]))
                    alphaNumericalCharactersProcessed++;

                if (alphaNumericalCharactersProcessed + truncationString.Length == length)
                    return value.Substring(0, i + 1) + truncationString;
            }

            return value;
        }
    }

    class FixedNumberOfWordsTruncator : ITruncator
    {
        public string Truncate(string value, int length, string truncationString, TruncateFrom truncateFrom = TruncateFrom.Right)
        {
            if (value == null)
                return null;

            if (value.Length == 0)
                return value;

            var numberOfWords = value.Split((char[])null, StringSplitOptions.RemoveEmptyEntries).Count();
            if (numberOfWords <= length)
                return value;

            return truncateFrom == TruncateFrom.Left
                ? TruncateFromLeft(value, length, truncationString)
                : TruncateFromRight(value, length, truncationString);
        }

        private static string TruncateFromRight(string value, int length, string truncationString)
        {
            var lastCharactersWasWhiteSpace = true;
            var numberOfWordsProcessed = 0;
            for (var i = 0; i < value.Length; i++)
            {
                if (char.IsWhiteSpace(value[i]))
                {
                    if (!lastCharactersWasWhiteSpace)
                        numberOfWordsProcessed++;

                    lastCharactersWasWhiteSpace = true;

                    if (numberOfWordsProcessed == length)
                        return value.Substring(0, i) + truncationString;
                }
                else
                    lastCharactersWasWhiteSpace = false;

            }
            return value + truncationString;
        }

        private static string TruncateFromLeft(string value, int length, string truncationString)
        {
            var lastCharactersWasWhiteSpace = true;
            var numberOfWordsProcessed = 0;
            for (var i = value.Length - 1; i > 0; i--)
            {
                if (char.IsWhiteSpace(value[i]))
                {
                    if (!lastCharactersWasWhiteSpace)
                        numberOfWordsProcessed++;

                    lastCharactersWasWhiteSpace = true;

                    if (numberOfWordsProcessed == length)
                        return truncationString + value.Substring(i + 1).TrimEnd();
                }
                else
                    lastCharactersWasWhiteSpace = false;

            }
            return truncationString + value;
        }
    }


}
