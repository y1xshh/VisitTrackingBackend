namespace VisitTracking.Application.DTOs
{
    public class ExpenseApprovalDto
    {
        public int VisitId { get; set; }
        public int? SubmittedBy { get; set; }
        public int? ApprovedBy { get; set; }
        public string? ApprovalStatus { get; set; }
        public string? ApprovalRemarks { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public bool? IsActive { get; set; }
    }
}
