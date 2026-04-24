using VisitTracking.Application.DTOs;
using VisitTracking.Domain.Entities;

namespace VisitTracking.Application.Interface
{
    public interface IDepartmentService
    {
        Task<List<DepartmentDto>> GetAllAsync();   
        Task<DepartmentDto?> GetByIdAsync(int id); 
        Task Create(DepartmentDto dto);
        Task UpdateAsync(int id, DepartmentDto dto);
        Task DeleteAsync(int id);
    }
}
