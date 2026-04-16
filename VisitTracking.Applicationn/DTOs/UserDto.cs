namespace VisitTracking.Application.DTOs
{
    public class UserDto
    {
        public string EmployeeCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public int DesignationId { get; set; }
        public int DepartmentId { get; set; }
    }
}