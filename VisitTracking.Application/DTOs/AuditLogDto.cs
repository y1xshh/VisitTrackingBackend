namespace VisitTracking.Application.DTOs
{
    public class AuditLogDto
    {
        public int Id { get; set; }

        public string? TableName { get; set; }
        public int? RecordId { get; set; }
        public string? ActionType { get; set; }

        public string? OldValueJson { get; set; }
        public string? NewValueJson { get; set; }

        public int? ActionBy { get; set; }

        public bool? IsActive { get; set; }
    }
}
