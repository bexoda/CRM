using Microsoft.AspNetCore.Identity;

namespace CRM.Entities
{
    public class AppUser : IdentityUser
    {
        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
