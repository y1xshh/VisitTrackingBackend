using Newtonsoft.Json;
using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;
using VisitTracking.Domain.Entities;

public class OutcomeTypeService : IOutcomeTypeService
{
    private readonly IOutcomeTypeRepository _repo;
    private readonly IAuditLogService _auditService;

    public OutcomeTypeService(IOutcomeTypeRepository repo, IAuditLogService auditLogService)
    {
        _repo = repo;
        _auditService = auditLogService;
    }

    public async Task<IEnumerable<OutcomeTypeDto>> GetAllAsync()
    {
        var data = await _repo.GetAllAsync();

        return data.Select(x => new OutcomeTypeDto
        {
            Id = x.Id,
            OutComeName = x.OutcomeName,
            IsRevenueLinked = x.IsRevenueLinked,
            IsActive = x.IsActive
        });
    }

    public async Task<OutcomeTypeDto?> GetByIdAsync(int id)
    {
        var x = await _repo.GetByIdAsync(id);
        if (x == null) return null;

        return new OutcomeTypeDto
        {
            Id = x.Id,
            OutComeName = x.OutcomeName,
            IsRevenueLinked = x.IsRevenueLinked,
            IsActive = x.IsActive
        };
    }

    public async Task CreateAsync(OutcomeTypeDto dto)
    {
        var entity = new Outcometype
        {
            OutcomeName = dto.OutComeName,
            IsRevenueLinked = dto.IsRevenueLinked,
            IsActive = dto.IsActive,
            InsertedDate = DateTime.Now
        };

        await _repo.AddAsync(entity);

        await _auditService.CreateAsync(new AuditLogDto
        {
            TableName = "Outcometype",
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

    public async Task UpdateAsync(OutcomeTypeDto dto)
    {
        var existingEntity = await _repo.GetByIdAsync((int)dto.Id);
        if (existingEntity == null) return;

        var oldValueJson = JsonConvert.SerializeObject(existingEntity, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });

        existingEntity.OutcomeName = dto.OutComeName;
        existingEntity.IsRevenueLinked = dto.IsRevenueLinked;
        existingEntity.IsActive = dto.IsActive;
        existingEntity.UpdatedDate = DateTime.Now;

        await _repo.UpdateAsync(existingEntity);

        await _auditService.CreateAsync(new AuditLogDto
        {
            TableName = "Outcometype",
            RecordId = existingEntity.Id,
            ActionType = "UPDATE",
            OldValueJson = oldValueJson,
            NewValueJson = JsonConvert.SerializeObject(existingEntity, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }),
            ActionBy = 1
        });
    }

    public async Task DeleteAsync(int id)
    {
        var existingEntity = await _repo.GetByIdAsync(id);
        if (existingEntity == null) return;

        var oldValueJson = JsonConvert.SerializeObject(existingEntity, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });

        await _repo.DeleteAsync(id);

        await _auditService.CreateAsync(new AuditLogDto
        {
            TableName = "Outcometype",
            RecordId = id,
            ActionType = "DELETE",
            OldValueJson = oldValueJson,
            NewValueJson = null,
            ActionBy = 1
        });
    }
}
