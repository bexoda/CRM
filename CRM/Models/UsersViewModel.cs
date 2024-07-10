using CRM.Entities;
using System.ComponentModel.DataAnnotations;

namespace crm.Models.ViewModels
{
    public class UsersViewModel
    {
        public string? Id { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Role")]
        public string? RoleId { get; set; }
        public int EmployeeId { get; set; }
    }
}
