namespace VisitTracking.Application.DTOs;

public class LocationDto
{
    public int Id { get; set; }
    public string LocationName { get; set; } = null!;
    public string? State { get; set; }
    public string? Country { get; set; }
    public bool? IsActive { get; set; }
}