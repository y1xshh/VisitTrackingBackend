namespace VisitTracking.Application.DTOs
{
    public class EmployeeDto
    {
        public string? EmployeeCode { get; set; } // ✅ ADD THIS
        public int UserId { get; set; }
        public int DesignationId { get; set; }
        public int? ReportingManagerId { get; set; }
        public int? LocationId { get; set; }
        public bool IsActive { get; set; }
    }
}