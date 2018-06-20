using System.Collections.Generic;

namespace cloudscribe.SimpleContent.Web.ViewModels
{
    public class StaffMemberListViewModel
    {
        public StaffMemberListViewModel()
        {
            StaffMembers = new List<StaffMemberViewModel>();
        }

        public List<StaffMemberViewModel> StaffMembers { get; set; }
    }
}
