namespace VisitTracking.Application.DTOs
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string? EmployeeCode { get; set; }

        public int? UserId { get; set; }
        public int? DesignationId { get; set; }
        public int? ReportingManagerId { get; set; }
        public int? LocationId { get; set; }

        public bool? IsActive { get; set; }

        // ✅ Display fields
        public string? DesignationName { get; set; }
        public string? LocationName { get; set; }

        // 🔥 Updated
        public string? ReportingManagerDisplay { get; set; }
    }
}