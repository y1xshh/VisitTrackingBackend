using Microsoft.AspNetCore.Mvc;
using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;


namespace VisitTracking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService service) : ControllerBase
{
    private readonly IAuthService _service = service;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDTo dTo)
    {
        var result = await _service.Register(dTo);
        return Ok("Register succesfully");
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var result = await _service.Login(dto);

        if (string.IsNullOrEmpty(result.Token))
            return Unauthorized(result);

        return Ok(result);
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var result = await _service.ChangePassword(dto);
        if (result == "Password changed successfully")
            return Ok(result);
        return BadRequest("not update");
    }

    

}

    