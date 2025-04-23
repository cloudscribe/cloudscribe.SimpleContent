using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;

namespace cloudscribe.SimpleContent.Web.TagHelpers
{
    [HtmlTargetElement("safe-script")]
    public class SafeScriptTagHelper : TagHelper
    {
        public string Script { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.SetAttribute("id", "user-script-container");

            if (!string.IsNullOrWhiteSpace(Script))
            {
                string encodedScript = JavaScriptEncoder.Default.Encode(Script);
                output.Attributes.SetAttribute("data-user-script", encodedScript);
            }

            output.TagMode = TagMode.StartTagAndEndTag;
        }
    }
}