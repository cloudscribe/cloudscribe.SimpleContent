using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace cloudscribe.SimpleContent.Storage.NoDb
{
    public class YamlHelper
    {

        static readonly Regex YamlFrontMatterRegex = new Regex("^---[\n,\r\n].*?^(---[\n,\r\n]|...[\n,\r\n])", RegexOptions.Singleline | RegexOptions.Multiline);


        public Match MatchFrontMatter (string markdownWithYaml)
        {
            return YamlFrontMatterRegex.Match(markdownWithYaml);
        }

        public string RemoveFrontMatterDelimiters(string source,
            string beginDelim = "---",
            string endDelim = "\n---")
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;

            int at1, at2;
            at1 = source.IndexOf(beginDelim, 0, source.Length, StringComparison.OrdinalIgnoreCase);
            if (at1 == -1)
                return string.Empty;

            at2 = source.IndexOf(endDelim, at1 + beginDelim.Length, StringComparison.OrdinalIgnoreCase);

            if (at1 > -1 && at2 > 1)
            {
                return source.Substring(at1 + beginDelim.Length, at2 - at1 - beginDelim.Length);
            }

            return string.Empty;
        }

    }
}
