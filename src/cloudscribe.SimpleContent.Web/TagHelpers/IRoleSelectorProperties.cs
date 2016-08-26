using System.Collections.Generic;

namespace cloudscribe.SimpleContent.Web.TagHelpers
{
    public interface IRoleSelectorProperties
    {
        string Controller { get; }

        string Action { get; }

        Dictionary<string, string> Attributes { get; }

        Dictionary<string, string> RouteParams { get; }

        List<string> RequiredScriptPaths { get; }

        string CsvTargetElementId { get; }
    }
}
