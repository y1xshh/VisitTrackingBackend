namespace VisitTracking.Application.DTOs
{
    public sealed class VisitApprovalRequestDto
    {
        public bool IsApproved { get; set; }

        public string? Remark { get; set; }
    }
}
