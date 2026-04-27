using VisitTracking.Application.DTOs;
using VisitTracking.Domain.RepositoryInterfaces;

namespace VisitTracking.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        // public async Task<List<UserListDto>> GetAllUsersAsync()
        // {
        //     var users = await _repository.GetAllAsync();

        //     return [.. users.Select(x => new UserListDto
        //     {
        //         EmployeeCode = x.EmployeeCode ?? "",
        //         FullName = x.FullName ?? "",
        //         Email = x.Email ?? "",
        //         Mobile = x.Mobile ?? "",
        //         RoleId = x.RoleId,
        //         DesignationId = x.DesignationId,
        //         DepartmentId = x.DepartmentId,
        //         ManagerId = x.ReportingManagerId,
        //         LocationId = x.LocationId,
        //         IsActive = x.IsActive
        //     })];
        // }

        // public async Task<UserListDto?> GetUserByIdAsync(int id)
        // {
        //     var x = await _repository.GetByIdAsync(id);

        //     if (x == null) return null;

        //     return new UserListDto
        //     {
        //         EmployeeCode = x.EmployeeCode ?? "",
        //         FullName = x.FullName ?? "",
        //         Email = x.Email ?? "",
        //         Mobile = x.Mobile ?? "",
        //         RoleId = x.RoleId,
        //         DesignationId = x.DesignationId,
        //         DepartmentId = x.DepartmentId,
        //         ManagerId = x.ReportingManagerId,
        //         LocationId = x.LocationId,
        //         IsActive = x.IsActive
        //     };
        // }

        // // ✅ FIXED
        // public async Task<bool> UpdateUserAsync(int id, UserListDto dto)
        // {
        //     var user = await _repository.GetByIdAsync(id);

        //     if (user == null)
        //         return false;

        //     user.FullName = dto.FullName ?? "";
        //     user.Email = dto.Email ?? "";
        //     user.Mobile = dto.Mobile ?? "";

        //     user.RoleId = dto.RoleId ?? 0;
        //     user.DesignationId = dto.DesignationId ?? 0;
        //     user.DepartmentId = dto.DepartmentId ?? 0;
        //     user.ReportingManagerId = dto.ManagerId;
        //     user.LocationId = dto.LocationId ?? 0;

        //     user.IsActive = dto.IsActive ?? true;

        //     await _repository.UpdateAsync(user);

        //     return true;
        // }

        // // ✅ FIXED
        // public async Task<bool> DeleteUserAsync(int id)
        // {
        //     var user = await _repository.GetByIdAsync(id);

        //     if (user == null)
        //         return false;

        //     await _repository.DeleteAsync(id);
        //     return true;
        // }

     
    }
}
