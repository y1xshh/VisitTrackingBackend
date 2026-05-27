using System;

namespace VisitTracking.Domain.Entities;

public partial class ExpenseApproval
{
    public int Id { get; set; }

    public int VisitId { get; set; }

    public int? SubmittedBy { get; set; }

    public int? ApprovedBy { get; set; }

    public string? ApprovalStatus { get; set; }

    public string? ApprovalRemarks { get; set; }

    public DateTime? SubmittedAt { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public bool? IsActive { get; set; }

    public string? InsertedBy { get; set; }

    public DateTime? InsertedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual Visit Visit { get; set; } = null!;
}
