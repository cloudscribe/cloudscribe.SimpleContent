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

        public Dictionary<string, string> GetAttributes(string csvTargetElementId, string displayTargetId = "")
        {
            return null; 
        }
        
        public Dictionary<string, string> GetRouteParams(string projectId)
        {
            return null; 
        }

        public List<string> RequiredScriptPaths
        {
            get { return null; }
        }

       
    }
}
