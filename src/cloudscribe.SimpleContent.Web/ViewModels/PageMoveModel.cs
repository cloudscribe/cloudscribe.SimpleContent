using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web.ViewModels
{
    public class PageMoveModel
    {
        public string MovedNode { get; set; }
        public string TargetNode { get; set; }
        public string PreviousParent { get; set; }
        public string Position { get; set; }
    }
}
