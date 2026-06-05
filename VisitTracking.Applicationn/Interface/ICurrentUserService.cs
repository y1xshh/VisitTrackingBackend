using System.Security.Claims;

namespace VisitTracking.Application.Interface
{
    public interface ICurrentUserService
    {
        int UserId { get; }
        int EmployeeId { get; }
        string? Designation { get; }
        ClaimsPrincipal? Principal { get; }
    }
}
