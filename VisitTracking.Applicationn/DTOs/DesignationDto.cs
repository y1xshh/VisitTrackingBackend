namespace VisitTracking.Application.DTOs;

public class DesignationDto
{
    public string DesignationName { get; set; } = null!;
    public int? DepartmentId { get; set; }
    public bool? IsActive { get; set; }
}