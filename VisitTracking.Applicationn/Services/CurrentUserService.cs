using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using VisitTracking.Application.Interface;

namespace VisitTracking.Application.Services
{
    public sealed class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal? GetCurrentPrincipal()
        {
            return _httpContextAccessor.HttpContext?.User;
        }

        public int GetCurrentUserId()
        {
            return GetIntClaim("UserId", ClaimTypes.NameIdentifier, "id", "sub");
        }

        public int GetCurrentEmployeeId()
        {
            return GetIntClaim("EmployeeId");
        }

        public string? GetCurrentDesignation()
        {
            var principal = GetCurrentPrincipal();
            return principal?.FindFirstValue("Designation");
        }

        public string? GetCurrentRole()
        {
            var principal = GetCurrentPrincipal();
            return principal?.FindFirstValue(ClaimTypes.Role) ?? principal?.FindFirstValue("role");
        }

        private int GetIntClaim(params string[] claimTypes)
        {
            var principal = GetCurrentPrincipal();

            foreach (var claimType in claimTypes)
            {
                var value = principal?.FindFirstValue(claimType);
                if (int.TryParse(value, out var parsedValue))
                {
                    return parsedValue;
                }
            }

            return 0;
        }
    }
}
