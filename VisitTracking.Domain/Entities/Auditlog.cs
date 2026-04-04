namespace VisitTracking.Domain.Entities;

public partial class Auditlog
{
    public int Id { get; set; }

    public string? TableName { get; set; }

    public int? RecordId { get; set; }

    public string? ActionType { get; set; }

    public string? OldValueJson { get; set; }

    public string? NewValueJson { get; set; }

    public int? ActionBy { get; set; }

    public bool? IsActive { get; set; }

    public string? InsertedBy { get; set; }

    public DateTime? InsertedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
