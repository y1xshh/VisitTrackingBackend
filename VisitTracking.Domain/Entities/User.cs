namespace VisitTracking.Domain.Entities
{
    public partial class User
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Mobile { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;

        public int RoleId { get; set; }
        public int? DesignationId { get; set; }
        public int DepartmentId { get; set; }

        public bool IsActive { get; set; } = true;

        public string InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }

        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public bool IsFirstLogin { get; set; } = true;

        public virtual Role? Role { get; set; }
        public virtual Department? Department { get; set; }

        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public string EmployeeCode { get; set; }
        public int? ReportingManagerId { get; set; }
        public int? LocationId { get; set; }

    }
}