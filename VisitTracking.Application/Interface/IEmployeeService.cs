using VisitTracking.Application.DTOs;

namespace VisitTracking.Application.Interface;

public interface IEmployeeService
{
    Task<List<EmployeeDto>> GetAllAsync();
    Task<EmployeeDto?> GetByIdAsync(int id);
    Task UpdateAsync(int id, EmployeeDto dto);
    Task<List<EmployeeDropdownDto>> GetReportingManagerDropdown();
}