using System.Security.Claims;

namespace VisitTracking.Application.Interface
{
    public interface ICurrentUserService
    {
        int GetCurrentUserId();
        int GetCurrentEmployeeId();
        string? GetCurrentDesignation();
        string? GetCurrentRole();
        ClaimsPrincipal? GetCurrentPrincipal();
    }
}
