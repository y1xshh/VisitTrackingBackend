namespace VisitTracking.Application.DTOs
{
    public class DepartmentResponseDto
    {
        public int Id { get; set; }
        public string? DepartmentName { get; set; }
        public int OrganisationId { get; set; }
        public List<string> Designations { get; set; } = new List<string>();
    }
}