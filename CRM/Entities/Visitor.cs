
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Entities
{
    public class Visitor
    {
        [Key]
        [Display(Name = "Visitor ID")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Visitor Name")]
        [StringLength(100, ErrorMessage = "Visitor name must not exceed 100 characters.")]
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
        [Display(Name = "Purpose of Visit")]
        [StringLength(200, ErrorMessage = "Purpose of visit must not exceed 200 characters.")]
        public string PurposeOfVisit { get; set; }

        [Display(Name = "Check-in Time")]
        public DateTime CheckInTime { get; set; } = DateTime.UtcNow;

        [Display(Name = "Check-out Time")]
        public DateTime? CheckOutTime { get; set; }

        [Display(Name = "Host")]
        [ForeignKey("HostId")]
        public Employee Host { get; set; }
        [Display(Name = "Host ID")]
        public int HostId { get; set; }

        [Display(Name = "Badge ID")]
        [StringLength(50, ErrorMessage = "Badge ID must not exceed 50 characters.")]
        public string? BadgeId { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Updated Date")]
        public DateTime? UpdatedDate { get; set; }


    }
}