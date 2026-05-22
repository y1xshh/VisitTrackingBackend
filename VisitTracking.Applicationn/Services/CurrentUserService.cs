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

        public ClaimsPrincipal? Principal => _httpContextAccessor.HttpContext?.User;

        public int UserId => GetIntClaim("UserId", ClaimTypes.NameIdentifier, "id", "sub");

        public int EmployeeId => GetIntClaim("EmployeeId");

        public string? Designation => Principal?.FindFirstValue("Designation");

        private int GetIntClaim(params string[] claimTypes)
        {
            foreach (var claimType in claimTypes)
            {
                var value = Principal?.FindFirstValue(claimType);
                if (int.TryParse(value, out var parsedValue))
                {
                    return parsedValue;
                }
            }

            return 0;
        }
    }
}
