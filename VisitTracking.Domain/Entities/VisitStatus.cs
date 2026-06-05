namespace VisitTracking.Domain.Entities
{
    public enum VisitStatus
    {
        Pending = 1,
        ForwardedToRM = 2,
        ForwardedToManager = 3,
        PendingAdminApproval = 4,
        Approved = 5,
        Rejected = 6,
        Cancelled = 7,
        Completed = 8
    }
}
