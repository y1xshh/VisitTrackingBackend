using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;
using VisitTracking.Domain.RepositoryInterfaces;


namespace VisitTracking.Application.Services
{
    public class UserListService : IUserListService
    {
        private readonly IUserListRepository _repository;

        public UserListService(IUserListRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<UserListDto>> GetAllUsersAsync()
        {
            var data = await _repository.GetAllAsync();

            return [.. data.Select(x => new UserListDto
        {
            EmployeeCode = x.EmployeeCode,
            FullName = x.FullName,
            Email = x.Email,
            Mobile = x.Mobile,
            Rolename = x.RoleId,
            DesignationId = x.DesignationId,
            DepartmentId = x.DepartmentId,
            MangerId = (int)x.ReportingManagerId,
            LocationId = x.LocationId,
            IsActive = x.IsActive
        })];
        }

        public async Task<UserListDto?> GetUserByIdAsync(int id)
        {
            var x = await _repository.GetByIdAsync(id);

            if (x == null) return null;

            return new UserListDto
            {
                EmployeeCode = x.EmployeeCode ?? "",
                FullName = x.FullName ?? "",
                Email = x.Email ?? "",
                Mobile = x.Mobile ?? "",
                Rolename = x.RoleId, // agar int hai to ok
                DesignationId = x.DesignationId,
                DepartmentId = x.DepartmentId,
                MangerId = (int)x.ReportingManagerId,
                LocationId = x.LocationId,
                IsActive = x.IsActive
            };
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);

            if (user == null)
                return false;

            await _repository.DeleteAsync(id);
            return true;
        }


        public async Task<bool> UpdateUserAsync(int id, UserListDto dto)
        {
            var user = await _repository.GetByIdAsync(id);

            if (user == null)
                return false;

            user.FullName = dto.FullName ?? "";
            user.Email = dto.Email ?? "";
            user.Mobile = dto.Mobile ?? "";

            await _repository.UpdateAsync(user);

            return true;
        }
    }
}