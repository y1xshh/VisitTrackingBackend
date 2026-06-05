using System;

namespace VisitTracking.Domain.Entities;

public partial class VisitApprovalHistory
{
    public int Id { get; set; }

    public int VisitId { get; set; }

    public string PreviousStatus { get; set; } = default!;

    public string NewStatus { get; set; } = default!;

    public int ActionByUserId { get; set; }

    public DateTime ActionDateUtc { get; set; }

    public string? IpAddress { get; set; }

    public string? Remark { get; set; }

    public bool? IsActive { get; set; }

    public string? InsertedBy { get; set; }

    public DateTime? InsertedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual Visit? Visit { get; set; }
}
