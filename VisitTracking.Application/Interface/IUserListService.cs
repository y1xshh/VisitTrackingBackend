using VisitTracking.Application.DTOs;

namespace VisitTracking.Application.Interface
{
    public interface IUserListService
    {
        Task<List<UserListDto>> GetAllUsersAsync();
        Task<UserListDto?> GetUserByIdAsync(int id);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> UpdateUserAsync(int id, UserListDto dto); // ✔ FIXED
    }
}