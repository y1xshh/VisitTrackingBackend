using Newtonsoft.Json;
using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _repository;
    private readonly IAuditLogService _auditService;

    public RoleService(IRoleRepository repository, IAuditLogService auditLogService)
    {
        _repository = repository;
        _auditService = auditLogService;
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

        await _auditService.CreateAsync(new AuditLogDto
        {
            TableName = "Role",
            RecordId = entity.Id,
            ActionType = "INSERT",
            OldValueJson = null,
            NewValueJson = JsonConvert.SerializeObject(entity, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }),
            ActionBy = 1
        });
    }

    public async Task UpdateAsync(int id, RoleDto dto)
    {
        var data = await _repository.GetByIdAsync(id);
        if (data == null) return;

        var oldValueJson = JsonConvert.SerializeObject(data, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });

        data.RoleName = dto.RoleName;

        await _repository.UpdateAsync(data);

        await _auditService.CreateAsync(new AuditLogDto
        {
            TableName = "Role",
            RecordId = data.Id,
            ActionType = "UPDATE",
            OldValueJson = oldValueJson,
            NewValueJson = JsonConvert.SerializeObject(data, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }),
            ActionBy = 1
        });
    }

    public async Task DeleteAsync(int id)
    {
        var oldEntity = await _repository.GetByIdAsync(id);

        await _repository.DeleteAsync(id);

        await _auditService.CreateAsync(new AuditLogDto
        {
            TableName = "Role",
            RecordId = id,
            ActionType = "DELETE",
            OldValueJson = oldEntity == null ? JsonConvert.SerializeObject(new { Id = id }, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }) : JsonConvert.SerializeObject(oldEntity, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }),
            NewValueJson = null,
            ActionBy = 1
        });
    }
}