using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Entities
{
    public class Employee
    {

        [Key]
        [Display(Name = "Staff ID")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Staff Number")]
        [StringLength(50, ErrorMessage = "Staff number must not exceed 50 characters.")]
        public string StaffNumber { get; set; }

        [Required]
        [Display(Name = "Surname")]
        [StringLength(100, ErrorMessage = "Surname must not exceed 100 characters.")]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(100, ErrorMessage = "First name must not exceed 100 characters.")]
        public string Firstname { get; set; }

        [Display(Name = "Other Name")]
        [StringLength(100, ErrorMessage = "Other name must not exceed 100 characters.")]
        public string Othername { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Full Name")]
        public string Fullname => $"{Surname} {Firstname} {Othername}";

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [ForeignKey("DepartmentId")]
        [Display(Name = "Department")]
        public Department Department { get; set; }

        [Display(Name = "Department ID")]
        public int DepartmentId { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Updated Date")]
        public DateTime? UpdatedDate { get; set; }

    }

}

