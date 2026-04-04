using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;

namespace VisitTracking.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repo;

        public EmployeeService(IEmployeeRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task AddAsync(EmployeeDto dto)
        {
            var employee = new Employee
            {
                // ✅ FIX: correct mapping
                EmployeeCode = dto.EmployeeCode,

                UserId = dto.UserId,
                DesignationId = dto.DesignationId,

                // ✅ FIX: avoid FK error
                ReportingManagerId = dto.ReportingManagerId > 0
                    ? dto.ReportingManagerId
                    : null,

                LocationId = dto.LocationId > 0
                    ? dto.LocationId
                    : null,

                IsActive = dto.IsActive
            };

            await _repo.AddAsync(employee);
        }

        public async Task UpdateAsync(int id, EmployeeDto dto)
        {
            var employee = await _repo.GetByIdAsync(id);

            if (employee == null)
                throw new Exception("Employee not found");

            employee.EmployeeCode = dto.EmployeeCode;
            employee.UserId = dto.UserId;
            employee.DesignationId = dto.DesignationId;

            employee.ReportingManagerId = dto.ReportingManagerId > 0
                ? dto.ReportingManagerId
                : null;

            employee.LocationId = dto.LocationId > 0
                ? dto.LocationId
                : null;

            employee.IsActive = dto.IsActive;

            await _repo.UpdateAsync(employee);
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}