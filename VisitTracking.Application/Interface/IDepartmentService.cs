using VisitTracking.Application.DTOs;
using VisitTracking.Domain.Entities;

namespace VisitTracking.Application.Interface
{
    public interface IDepartmentService
    {
        Task<List<Department>> GetAllAsync();
        Task<Department> GetByIdAsync(int id);
        Task Create(DepartmentDto dto);   // 👈 yeh hai
        Task UpdateAsync(int id, DepartmentDto dto);
        Task DeleteAsync(int id);
    }
}
