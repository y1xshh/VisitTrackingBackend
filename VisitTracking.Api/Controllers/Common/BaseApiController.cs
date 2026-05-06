using Microsoft.AspNetCore.Mvc;

namespace VisitTracking.Api.Controllers.Common
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected IActionResult SuccessResponse(string message = "Success")
        {
            return Ok(new ApiResponse
            {
                Success = true,
                Message = message
            });
        }

        protected IActionResult ErrorResponse(string message = "An unexpected error occurred.")
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
            {
                Success = false,
                Message = message
            });
        }

        protected IActionResult NotFoundResponse(string message = "Not found")
        {
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = message
            });
        }

        protected IActionResult BadRequestResponse(string message = "Bad request")
        {
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = message
            });
        }
    }
}