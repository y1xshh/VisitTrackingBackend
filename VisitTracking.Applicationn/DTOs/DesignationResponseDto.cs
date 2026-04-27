namespace VisitTracking.Application.DTOs;

public class DesignationResponseDto
{
    public int Id { get; set; }
    public string DesignationName { get; set; } = null!;
    public int? DepartmentId { get; set; }
    public bool? IsActive { get; set; }
}