using VisitTracking.Application.DTOs;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _repository;

    public RoleService(IRoleRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Role>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Role> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task Create(RoleDto dto)
    {
        var entity = new Role
        {
            RoleName = dto.RoleName
        };

        await _repository.AddAsync(entity);
    }

    public async Task UpdateAsync(int id, RoleDto dto)
    {
        var data = await _repository.GetByIdAsync(id);
        if (data == null) return;

        data.RoleName = dto.RoleName;

        await _repository.UpdateAsync(data);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}