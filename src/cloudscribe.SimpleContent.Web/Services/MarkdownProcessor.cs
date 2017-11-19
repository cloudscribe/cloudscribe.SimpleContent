using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace cloudscribe.SimpleContent.Web.Services
{
    public class MarkdownProcessor
    {
        
        private const string imgRegex = @"!\[.*?\]\((.*?)\)";
        public string ExtractFirstImageUrl(string markdown)
        {
            var matches = new Regex(imgRegex)
                .Matches(markdown)
                .Cast<Match>()
                .Select(m => m.Value.Replace("![](", "").Replace(")","")) // this is funky, need a better regex to eliminate the need for replace
                .ToList();

            return matches.FirstOrDefault();
        }
    }
}
