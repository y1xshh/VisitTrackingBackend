using System;
using System.Collections.Generic;

namespace VisitTracking.Domain.Entities;

public partial class Visitfollowup
{
    public int Id { get; set; }

    public int? VisitId { get; set; }

    public DateTime? FollowUpDate { get; set; }

    public string? FollowUpRemarks { get; set; }

    public int? FunnelStageId { get; set; }

    public int? OutcomeTypeId { get; set; }

    public decimal? ExpectedBusinessValue { get; set; }

    public decimal? ActualBusinessValue { get; set; }

    public DateTime? NextFollowUpDate { get; set; }

    public bool? IsActive { get; set; }

    public string? InsertedBy { get; set; }

    public DateTime? InsertedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual Visit? Visit { get; set; }
}
