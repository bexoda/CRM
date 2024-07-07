using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Entities
{
    public class Appointment
    {
        [Key]
        [Display(Name = "Appointment ID")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Subject")]
        [StringLength(200, ErrorMessage = "Subject must not exceed 200 characters.")]
        public string Subject { get; set; }

        [Required]
        [Display(Name = "Description")]
        [StringLength(1000, ErrorMessage = "Description must not exceed 1000 characters.")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Appointment Date")]
        public DateTime AppointmentDate { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        public TimeSpan StartTime { get; set; }

        [Required]
        [Display(Name = "End Time")]
        public TimeSpan EndTime { get; set; }

        [ForeignKey("ClientId")]
        [Display(Name = "Client")]
        public Client Client { get; set; }
        [Display(Name = "Client ID")]
        public int ClientId { get; set; }

        [ForeignKey("StaffId")]
        [Display(Name = "Staff")]
        public Employee Staff { get; set; }
        [Display(Name = "Staff ID")]
        public int StaffId { get; set; }

        [Display(Name = "Location")]
        [StringLength(200, ErrorMessage = "Location must not exceed 200 characters.")]
        public string Location { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Updated Date")]
        public DateTime? UpdatedDate { get; set; }


    }
}
