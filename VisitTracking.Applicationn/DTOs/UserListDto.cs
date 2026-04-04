namespace VisitTracking.Application.DTOs
{
    public class UserListDto
    {
        public string? EmployeeCode { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }

        public int? RoleId { get; set; }
        public int? DesignationId { get; set; }
        public int? DepartmentId { get; set; }
        public int? ManagerId { get; set; }
        public int? LocationId { get; set; }

        public bool? IsActive { get; set; }
        public int Rolename { get; internal set; }
        public int MangerId { get; internal set; }
        public int Id { get; internal set; }
    }
}