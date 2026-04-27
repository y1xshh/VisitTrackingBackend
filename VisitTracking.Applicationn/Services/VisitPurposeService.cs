using Newtonsoft.Json;
using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;

public class VisitPurposeService : IVisitPurposeService
{
    private readonly IVisitPurposeRepository _repository;
    private readonly IAuditLogService _auditService;

    public VisitPurposeService(IVisitPurposeRepository repository, IAuditLogService auditLogService)
    {
        _repository = repository;
        _auditService = auditLogService;
    }

    public async Task<IEnumerable<VisitPurposeResponseDto>> GetAllAsync()
    {
        var data = await _repository.GetAllAsync();

        return data.Select(x => new VisitPurposeResponseDto
        {
            Id = x.Id,
            PurposeName = x.PurposeName,
            IsActive = x.IsActive.GetValueOrDefault(),
            IsActive = data.IsActive,
            IsActive = data.IsActive.GetValueOrDefault()
        }).ToList();
    }

    public async Task<VisitPurposeResponseDto?> GetByIdAsync(int id)
    {
        var data = await _repository.GetByIdAsync(id);
        if (data == null) return null;

        return new VisitPurposeResponseDto
        {
            Id = data.Id,
            PurposeName = data.PurposeName,
            IsActive = data.IsActive
        };
    }

    public async Task Create(VisitPurposeDto dto)
    {
        var entity = new Visitpurpose
        {
            PurposeName = dto.PurposeName,
            IsActive = dto.IsActive
        };

        await _repository.AddAsync(entity);

        await _auditService.CreateAsync(new AuditLogDto
        {
            TableName = "Visitpurpose",
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

    public async Task UpdateAsync(int id, VisitPurposeDto dto)
    {
        var data = await _repository.GetByIdAsync(id);
        if (data == null) return;

        var oldValueJson = JsonConvert.SerializeObject(data, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });

        data.PurposeName = dto.PurposeName;
        data.IsActive = dto.IsActive;

        await _repository.UpdateAsync(data);

        await _auditService.CreateAsync(new AuditLogDto
        {
            TableName = "Visitpurpose",
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
        var data = await _repository.GetByIdAsync(id);
        if (data == null) return;

        var oldValueJson = JsonConvert.SerializeObject(data, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });

        await _repository.DeleteAsync(id);

        await _auditService.CreateAsync(new AuditLogDto
        {
            TableName = "Visitpurpose",
            RecordId = id,
            ActionType = "DELETE",
            OldValueJson = oldValueJson,
            NewValueJson = null,
            ActionBy = 1
        });
    }
}
