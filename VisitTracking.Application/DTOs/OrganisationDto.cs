namespace VisitTracking.Application.DTOs;

public class OrganisationDto
{
    public int Id { get; set; }
    public string OrganisationName { get; set; }
    public int CompanyId { get; set; }
    public string? CompanyName { get; set; }

    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
}