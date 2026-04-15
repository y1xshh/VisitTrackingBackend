using VisitTracking.Application.DTOs;
using VisitTracking.Domain.Entities;

namespace VisitTracking.Application.Interface
{
    public interface IAuthService
    {
        Task<string> Register(RegisterDTo dto);
        Task<LoginResponseDto> Login(LoginDto dto);
        Task<string> ChangePassword(ChangePasswordDto dto);
        Task<string> ApproveUser(ApproveUserDto dto);
        Task<string> CreateUserByAdmin(CreateUserByAdminDto dto);
        Task CreateEmployee(EmployeeUserDto dto);

        Task<User> GetByEmailAsync(string email);
        Task<User> GetUserByIdAsync(int userId);
        Task UpdateAsync(User user);
    }
}
