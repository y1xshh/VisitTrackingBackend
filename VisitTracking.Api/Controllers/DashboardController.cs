using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
// [Authorize]
public class DashboardController : ControllerBase
{
    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public IActionResult AdminDashboard()
    {
        return Ok("Welcome Admin Dashboard");
    }

    [HttpGet("manager")]
    [Authorize(Roles = "Manager")]
    public IActionResult ManagerDashboard()
    {
        return Ok("Welcome Manager Dashboard");
    }

    [HttpGet("employee")]
    [Authorize(Roles = "Employee")]
    public IActionResult EmployeeDashboard()
    {
        return Ok("Welcome Employee Dashboard");
    }
}