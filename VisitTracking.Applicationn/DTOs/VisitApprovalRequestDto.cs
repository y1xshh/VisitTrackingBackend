namespace VisitTracking.Application.DTOs
{
    public sealed class VisitApprovalRequestDto
    {
        public string? Action { get; set; }

        public string? ForwardTo { get; set; }

        public string? Remark { get; set; }
    }
}
