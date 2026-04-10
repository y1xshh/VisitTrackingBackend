using System;
using System.Collections.Generic;

namespace VisitTracking.Domain.Entities;

public partial class Visit
{
    public int Id { get; set; }

    public string? VisitCode { get; set; }

    public DateTime? VisitDate { get; set; }

    public int? EmployeeId { get; set; }

    public int? CompanyId { get; set; }

    public int? OrganisationId { get; set; }

    public int? DepartmentId { get; set; }

    public int? ContactPersonId { get; set; }

    public int? VisitPurposeId { get; set; }

    public string? DiscussionSummary { get; set; }

    public string? NextAction { get; set; }

    public DateTime? NextFollowUpDate { get; set; }

    public int? VehicleTypeId { get; set; }

    public decimal? DistanceKm { get; set; }

    public decimal? RateAppliedPerKm { get; set; }

    public decimal? TravelExpenseAmount { get; set; }

    public int? FunnelStageId { get; set; }

    public int? OutcomeTypeId { get; set; }

    public decimal? ExpectedBusinessValue { get; set; }

    public decimal? ActualBusinessValue { get; set; }

    public int? ProbabilityPercent { get; set; }

    public string? Status { get; set; }

    public DateTime? CheckInTime { get; set; }

    public DateTime? CheckOutTime { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public string? Remarks { get; set; }

    public string? AttachmentPath { get; set; }

    public bool? IsActive { get; set; }

    public string? InsertedBy { get; set; }

    public DateTime? InsertedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual Company? Company { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<Expenseapproval> Expenseapprovals { get; set; } = new List<Expenseapproval>();

    public virtual ICollection<Visitattachment> Visitattachments { get; set; } = new List<Visitattachment>();

    public virtual ICollection<Visitfollowup> Visitfollowups { get; set; } = new List<Visitfollowup>();
}
