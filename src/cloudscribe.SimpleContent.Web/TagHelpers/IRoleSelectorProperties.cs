using System.Collections.Generic;

namespace cloudscribe.SimpleContent.Web.TagHelpers
{
    public interface IRoleSelectorProperties
    {
        string Controller { get; }

        string Action { get; }

        Dictionary<string, string> GetAttributes(string csvTargetElementId, string displayTargetId = "");

        Dictionary<string, string> GetRouteParams(string projectId);

        List<string> RequiredScriptPaths { get; }

        //string CsvTargetElementId { get; }
    }
}
