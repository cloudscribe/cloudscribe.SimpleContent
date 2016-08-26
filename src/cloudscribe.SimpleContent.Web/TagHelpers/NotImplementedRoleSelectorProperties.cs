using System.Collections.Generic;

namespace cloudscribe.SimpleContent.Web.TagHelpers
{
    public class NotImplementedRoleSelectorProperties : IRoleSelectorProperties
    {
        public string Action
        {
            get { return string.Empty; }
        }

        public string Controller
        {
            get { return string.Empty; }
        }

        public Dictionary<string, string> Attributes
        {
            get {  return null; }
        }
        
        public Dictionary<string, string> RouteParams
        {
            get { return null; }
        }

        public List<string> RequiredScriptPaths
        {
            get { return null; }
        }

        public string CsvTargetElementId
        {
            get { return string.Empty; }
        }
    }
}
