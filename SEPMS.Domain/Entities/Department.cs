using System.ComponentModel.DataAnnotations;

namespace SEPMS.Domain.Entities
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Department name")]
        public string DepartmentName { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        [Display(Name = "Department code")]
        public string DepartmentCode { get; set; } = string.Empty;

        [StringLength(250)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
