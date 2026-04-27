namespace VisitTracking.Application.DTOs
{
    public class VisitFollowUpDto
    {
        public int? VisitId { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public string? FollowUpRemarks { get; set; }
        public int? FunnelStageId { get; set; }
        public int? OutcomeTypeId { get; set; }
        public decimal? ExpectedBusinessValue { get; set; }
        public decimal? ActualBusinessValue { get; set; }
        public DateTime? NextFollowUpDate { get; set; }
        public bool? IsActive { get; set; }
    }
}