using VisitTracking.Application.DTOs;

namespace VisitTracking.Application.Interface
{
    public interface IUserService
    {
        Task<List<UserListDto>> GetAllUsersAsync();
        Task<UserListDto?> GetUserByIdAsync(int id);

        Task<bool> UpdateUserAsync(int id, UserListDto dto); // ✅ FIX
        Task<bool> DeleteUserAsync(int id);                  // ✅ FIX

    }
}