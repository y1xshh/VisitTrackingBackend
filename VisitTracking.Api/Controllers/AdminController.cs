using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; 
using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;

namespace VisitTracking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    [Authorize] 
    public class AdminController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserListService _UserListService;

        public AdminController(IAuthService authService, IUserListService userListService)
        {
            _authService = authService;
            _UserListService = userListService;
        }

        
        [HttpPost("create-user-by-admin")]
        public async Task<IActionResult> CreateUserByAdmin([FromBody] CreateUserByAdminDto dto)
        {
            if (dto == null)
                return BadRequest(new { success = false, message = "Invalid request body" });

            await _authService.CreateUserByAdmin(dto);

            return Ok(new
            {
                success = true,
                message = "User created successfully"
            });
        }

        
        [HttpPost("create-employee")]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeUserDto dto)
        {
            if (dto == null)
                return BadRequest(new { success = false, message = "Invalid request body" });

            try
            {
                await _authService.CreateEmployee(dto);

                return Ok(new
                {
                    success = true,
                    message = "Employee created successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
        }

       
        [HttpGet("users")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _UserListService.GetAllUsersAsync());
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _UserListService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }

        [HttpPut("user/{id}")]
        public async Task<IActionResult> Update(int id, UserListDto dto)
        {
            var result = await _UserListService.UpdateUserAsync(id, dto);

            if (!result)
                return NotFound("User not found");

            return Ok("Updated successfully");
        }

        [HttpDelete("user/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _UserListService.DeleteUserAsync(id);

            if (!result)
                return NotFound("User not found");

            return Ok("Deleted successfully");
        }
    }
}