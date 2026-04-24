using System.ComponentModel.DataAnnotations;

namespace VisitTracking.Application.DTOs
{
    public class EmployeeUserDto
    {
        [Required]
        public string FullName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [Phone]
        public string Mobile { get; set; } = null!;

        [Required]
        public int RoleId { get; set; }

        [Required]
        public int DepartmentId { get; set; }
        public string? EmployeeCode { get; set; }
        public int? DesignationId { get; set; }
        public int? ReportingManagerId { get; set; }
        public int? LocationId { get; set; }
    }
}