using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System;
using System.Linq;

namespace cloudscribe.ContentUtils
{
    public class MarkdownHelper
    {

        private MarkdownPipeline _mdPipeline = null;

        public string ConvertMarkdownToHtml(string markdown)
        {
            if (_mdPipeline == null)
            {
                _mdPipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            }

            return Markdown.ToHtml(markdown, _mdPipeline);
        }

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

    }
}
