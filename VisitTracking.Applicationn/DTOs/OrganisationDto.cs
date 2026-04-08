namespace VisitTracking.Application.DTOs;
public class OrganisationDto
{
    public int Id { get; set; }
    public string OrganisationName { get; set; }
    public int CompanyId { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }        // ✅ FIX
    public string UpdatedBy { get; set; }    // ✅ FIX
    public DateTime UpdatedDate { get; set; } // ✅ FIX
    public string? CompanyName { get; internal set; }
    public string? InsertedBy { get; internal set; }
}