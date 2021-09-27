using System.Linq;
using System.Text.RegularExpressions;
using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using cloudscribe.SimpleContent.Models;
using System;

namespace cloudscribe.SimpleContent.Web.Services
{
    public class MarkdownProcessor : IMarkdownProcessor
    {
        private MarkdownPipeline _mdPipeline = null;
        public string ExtractFirstImageUrl(string markdown)
        {
            if (_mdPipeline == null)
            {
                _mdPipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            }

            if (!String.IsNullOrWhiteSpace(markdown))
            {
                var doc = Markdown.Parse(markdown, _mdPipeline);
                var img = doc.Descendants<ParagraphBlock>()
                    .SelectMany(x => x.Inline.Descendants<LinkInline>())
                    .FirstOrDefault(l => l.IsImage);
                if (img != null)
                {
                    return img.Url;
                }
            }

            return string.Empty;
        }


        //private const string imgRegex = @"!\[.*?\]\((.*?)\)";
        
        //public string ExtractFirstImageUrl(string markdown)
        //{
        //    var matches = new Regex(imgRegex)
        //        .Matches(markdown)
        //        .Cast<Match>()
        //        .Select(m => ExtractUrl(m.Value)) 
        //        .ToList();

        //    return matches.FirstOrDefault();
        //}

        
        //private string ExtractUrl(string input)
        //{
        //    if (string.IsNullOrEmpty(input)) return input;
        //    // example input coming from the regex above
        //    // I just want the url:
        //    //![my pond](/media/images/img_1349-ws.jpg)
        //    var indexOpen = input.IndexOf("(");
        //    var indexClose = input.IndexOf(")");
        //    if(indexOpen > -1 && indexClose > -1 && indexClose > indexOpen)
        //    {
        //        return input.Substring(indexOpen + 1, indexClose - indexOpen -1);
        //    }
            
        //    return input;
        //}
    }
}
