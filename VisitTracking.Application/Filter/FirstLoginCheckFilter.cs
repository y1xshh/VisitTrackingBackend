using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VisitTracking.Infrastructure.Data;

namespace VisitTracking.Application.Filter
{
    public class FirstLoginCheckFilter : IAsyncActionFilter
    {
        private readonly AppDbContext _context;

        public FirstLoginCheckFilter(AppDbContext context)
        {
            _context = context;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userId = context.HttpContext.User.FindFirst("id")?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                var user = await _context.Users.FindAsync(int.Parse(userId));

                if (user != null && user.IsFirstLogin == true)
                {
                    context.Result = new UnauthorizedObjectResult("Please change password first");
                    return;
                }
            }

            await next();
        }
    }
}