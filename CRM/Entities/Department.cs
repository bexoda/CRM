using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CRM.Entities
{
    public class Department
    {
        [Key]
        [DisplayName("Department ID")]
        public int Id { get; set; }

        [Required]
        [DisplayName("Department Name")]
        [StringLength(100, ErrorMessage = "Department name must not exceed 100 characters.")]
        public string DepartmentName { get; set; }

        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; }

        [DisplayName("Updated Date")]
        public DateTime? UpdatedDate { get; set; }


    }

}
