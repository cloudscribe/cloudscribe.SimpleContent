using System;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.SimpleContent.Web.ViewModels
{
    public class StaffMemberViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        public string Phone { get; set; }
        public string MobilePhone { get; set; }

        [EmailAddress(ErrorMessage = "Email address must be valid format")]
        public string Email { get; set; }

        public string ImageUrl { get; set; }
        public string ImageFullSizeUrl { get; set; }
        public string ImageThumbSizeUrl { get; set; }

        public string ProfileUrl { get; set; }

        public string Department { get; set; }

        public string Position { get; set; }

        public string Biography { get; set; }

        public DateTime? JoinDate { get; set; }

        public int YearsExperience { get; set; }

      
        public decimal Salary { get; set; }

        public decimal? AnnualBonus { get; set; }
    }
}
