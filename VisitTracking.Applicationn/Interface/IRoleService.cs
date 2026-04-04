using VisitTracking.Application.DTOs;
using VisitTracking.Domain.Entities;

public interface IRoleService
{
    Task<List<Role>> GetAllAsync();
    Task<Role> GetByIdAsync(int id);
    Task Create(RoleDto dto);
    Task UpdateAsync(int id, RoleDto dto);
    Task DeleteAsync(int id);
}