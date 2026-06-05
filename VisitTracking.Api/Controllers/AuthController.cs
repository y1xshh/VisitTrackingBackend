using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;

namespace VisitTracking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService service) : ControllerBase
{
    private readonly IAuthService _service = service;

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterDTo dTo)
    {
        var result = await _service.Register(dTo);
        return Ok("Register successfully");
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var result = await _service.Login(dto);

        if (result.Message == "Invalid credentials")
            return Unauthorized(result);
     
        return Ok(result);

    }
  
[HttpPost("change-password")]
    [Authorize] 
    public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
    {
        var email = dto.Email?.Trim();
        if (string.IsNullOrWhiteSpace(email))
            return BadRequest("Email is required");

        if (string.IsNullOrWhiteSpace(dto.OldPassword))
            return BadRequest("Old password is required");

        if (string.IsNullOrWhiteSpace(dto.NewPassword))
            return BadRequest("New password is required");

        var user = await _service.GetByEmailAsync(email);

        if (!BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.PasswordHash))
            return BadRequest("Old password is incorrect");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        user.IsFirstLogin = false;

        await _service.UpdateAsync(user);

        return Ok("Password changed successfully");
    }

    [HttpPost("first-login-change-password")]
    [AllowAnonymous]
    public async Task<IActionResult> FirstLoginChangePassword(FirstLoginChangePasswordDto dto)
    {
        var result = await _service.FirstLoginChangePasswordAsync(dto);
        if (result.Token != string.Empty)
            return Ok(result);
        return BadRequest(result);
    }
}

