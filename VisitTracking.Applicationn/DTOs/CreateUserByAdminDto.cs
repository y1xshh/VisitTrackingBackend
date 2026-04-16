using System.ComponentModel.DataAnnotations.Schema;
using VisitTracking.Domain.Entities;

namespace VisitTracking.Application.DTOs
{
    public class CreateUserByAdminDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Mobile { get; set; }
        public int RoleId { get; set; }
        public int DesignationId { get; set; }
        public int DepartmentId { get; set; }
        public int? ReportingManagerId { get; set; }
    }
}