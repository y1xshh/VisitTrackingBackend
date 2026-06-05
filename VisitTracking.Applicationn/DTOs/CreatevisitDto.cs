namespace VisitTracking.Application.DTOs
{
    public class CreateVisitDto
    {
        public string? VisitCode { get; set; }
        public DateTime VisitDate { get; set; }

        public int CompanyId { get; set; }
        public int OrganisationId { get; set; }
        public int DepartmentId { get; set; }
        public int ContactPersonId { get; set; }
        public int VisitPurposeId { get; set; }

        public string? DiscussionSummary { get; set; }
        public string? NextAction { get; set; }
        public DateTime? NextFollowUpDate { get; set; }
        public int VehicleTypeId { get; set; }
        public decimal? DistanceKm { get; set; }
        public decimal? RateAppliedPerKm { get; set; }
        public int FunnelStageId { get; set; }
        public int OutcomeTypeId { get; set; }
        public decimal? ExpectedBusinessValue { get; set; }
        public decimal? ActualBusinessValue { get; set; }
        public decimal? ProbabilityPercent { get; set; }
        public string? Status { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? Remarks { get; set; }
        public string? AttachmentPath { get; set; }
    }
}
