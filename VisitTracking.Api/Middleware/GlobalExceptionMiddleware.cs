using System.Net;
using FluentValidation;
using Newtonsoft.Json;

namespace VisitTracking.Api.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception while processing request {Path}", context.Request.Path);

                context.Response.ContentType = "application/json";

                context.Response.StatusCode = ex switch
                {
                    ValidationException => (int)HttpStatusCode.BadRequest,
                    ArgumentException => (int)HttpStatusCode.BadRequest,
                    UnauthorizedAccessException => (int)HttpStatusCode.Forbidden,
                    KeyNotFoundException => (int)HttpStatusCode.NotFound,
                    InvalidOperationException => (int)HttpStatusCode.Conflict,
                    _ => (int)HttpStatusCode.InternalServerError
                };

                var response = new
                {
                    success = false,
                    message = ex is ValidationException or ArgumentException or UnauthorizedAccessException or KeyNotFoundException or InvalidOperationException
                        ? ex.Message
                        : "An unexpected error occurred."
                };

                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
        }
    }
}
