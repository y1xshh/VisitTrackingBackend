using System.ComponentModel.DataAnnotations.Schema;
using VisitTracking.Domain.Entities;

namespace VisitTracking.Application.DTOs
{
    public class CreateUserByAdminDto
    {
        public string FullName { get; set; }
            public string Email { get; set; }
            public string? Mobile { get; set; }
            public int RoleId { get; set; }
            public int DesignationId { get; set; }
            public int DepartmentId { get; set; }
            public int? ReportingManagerId { get; set; }
        }

    }

