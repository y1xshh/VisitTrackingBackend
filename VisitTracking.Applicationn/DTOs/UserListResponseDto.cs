
namespace VisitTracking.Application.DTOs
    {
    public class UserListResponseDto
    {
    public int Id { get; set; }
    public string? EmployeeCode { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Mobile { get; set; }
    public int RoleId { get; set; }
    public int? DesignationId { get; set; }
    public int? DepartmentId { get; set; }
    public int? ManagerId { get; set; }
    public int? LocationId { get; set; }
    public bool? IsActive { get; set; }
}
}
