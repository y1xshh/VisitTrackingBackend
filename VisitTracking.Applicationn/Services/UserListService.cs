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

            return data.Select(user =>
            {
                var employee = user.Employees?.FirstOrDefault();

                return new UserListDto
                {
                    Id = user.Id,

                    EmployeeCode = employee?.EmployeeCode,
                    FullName = user.FullName,
                    Email = user.Email,
                    Mobile = user.Mobile,

                    RoleId = user.Role?.Id ?? 0,
                    DesignationId = user.DesignationId,
                    DepartmentId = user.DepartmentId,

                    ManagerId = employee?.ReportingManagerId,   
                    LocationId = employee?.LocationId,

                    IsActive = user.IsActive
                };
            }).ToList();
        }

        public async Task<UserListDto?> GetUserByIdAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);

            if (user == null) return null;

             var employee = user.Employees?.FirstOrDefault();

            return new UserListDto
            {
                Id = user.Id,

                    EmployeeCode = employee?.EmployeeCode,
                    FullName = user.FullName,
                    Email = user.Email,
                    Mobile = user.Mobile,

                    RoleId = user.Role?.Id ?? 0,
                    DesignationId = user.DesignationId,
                    DepartmentId = user.DepartmentId,

                    ManagerId = employee?.ReportingManagerId,   
                    LocationId = employee?.LocationId,

                    IsActive = user.IsActive
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