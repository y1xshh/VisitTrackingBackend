using VisitTracking.Application.DTOs;
using VisitTracking.Domain.Entities;

    namespace VisitTracking.Application.Interface
    {
        public interface IEmployeeService
        {
            Task<List<Employee>> GetAllAsync();
            Task<Employee> GetByIdAsync(int id);
            Task AddAsync(EmployeeDto dto);
            Task UpdateAsync(int id, EmployeeDto dto);
            Task DeleteAsync(int id);
        }
    };
        
    

