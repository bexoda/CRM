using System.ComponentModel.DataAnnotations;

namespace CRM.Entities
{
    public class Client
    {
        [Key]
        [Display(Name = "Client ID")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Client Name")]
        [StringLength(100, ErrorMessage = "Client name must not exceed 100 characters.")]
        public string Name { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Company")]
        [StringLength(100, ErrorMessage = "Company name must not exceed 100 characters.")]
        public string Company { get; set; }

        [Display(Name = "Address")]
        [StringLength(200, ErrorMessage = "Address must not exceed 200 characters.")]
        public string Address { get; set; }

        [Display(Name = "Date Registered")]
        public DateTime DateRegistered { get; set; } = DateTime.UtcNow;

        [Display(Name = "Last Contact Date")]
        public DateTime? LastContactDate { get; set; }

        [Display(Name = "Notes")]
        [StringLength(1000, ErrorMessage = "Notes must not exceed 1000 characters.")]
        public string Notes { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Updated Date")]
        public DateTime? UpdatedDate { get; set; }


    }
}