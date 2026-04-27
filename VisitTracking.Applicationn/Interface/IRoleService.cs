using VisitTracking.Application.DTOs;

public interface IRoleService
{
    Task<IEnumerable<RoleResponseDto>> GetAllAsync();
    Task<RoleResponseDto?> GetByIdAsync(int id);
    Task Create(RoleDto dto);
    Task UpdateAsync(int id, RoleDto dto);
    Task DeleteAsync(int id);
}