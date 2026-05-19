namespace VisitTracking.Application.DTOs
{
    public sealed class VisitApprovalResponseDto
    {
        public int VisitId { get; set; }

        public string Status { get; set; } = default!;

        public DateTime ActionDateUtc { get; set; }

        public string Message { get; set; } = default!;
    }
}
