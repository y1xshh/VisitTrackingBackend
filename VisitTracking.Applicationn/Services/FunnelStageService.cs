using Newtonsoft.Json;
using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;

public class FunnelStageService : IFunnelStageService
{
    private readonly IFunnelStageRepository _repo;
    private readonly IAuditLogService _auditService;

    private static bool? NormalizeStageFlag(bool? value) => value == true ? true : null;

    public FunnelStageService(IFunnelStageRepository repo, IAuditLogService auditLogService)
    {
        _repo = repo;
        _auditService = auditLogService;
    }

    public async Task<IEnumerable<FunnelStageDto>> GetAllAsync()
    {
        var data = await _repo.GetAllAsync();

        return data
            .OrderBy(x => x.StageOrder)
            .Select(static x => new FunnelStageDto
            {
                Id = x.Id,
                StageName = x.StageName,
                Stagecode = $"F-{(x.StageOrder ?? 0).ToString("D2")}",
                StageOrder = x.StageOrder,
                IsClosedStage = x.IsClosedStage,
                IsWonStage = x.IsWonStage,
                IsLostStage = x.IsLostStage,
                IsActive = x.IsActive
            });
    }

    public async Task<FunnelStageDto?> GetByIdAsync(int id)
    {
        var x = await _repo.GetByIdAsync(id);
        if (x == null) return null;

        return new FunnelStageDto
        {
            Id = x.Id,
            Stagecode = $"F-{(x.StageOrder ?? 0).ToString("D2")}",
            StageName = x.StageName,
            StageOrder = x.StageOrder,
            IsClosedStage = x.IsClosedStage,
            IsWonStage = x.IsWonStage,
            IsLostStage = x.IsLostStage,
            IsActive = x.IsActive
        };
    }

    public async Task CreateAsync(FunnelStageDto dto)
    {
        var all = await _repo.GetAllAsync();

        int nextOrder = (all.Max(x => x.StageOrder) ?? 0) + 1;

        var entity = new Funnelstage
        {
            StageName = dto.StageName,
            StageOrder = nextOrder,
            IsClosedStage = NormalizeStageFlag(dto.IsClosedStage),
            IsWonStage = NormalizeStageFlag(dto.IsWonStage),
            IsLostStage = NormalizeStageFlag(dto.IsLostStage),
            IsActive = dto.IsActive,
            InsertedDate = DateTime.Now
        };

        await _repo.AddAsync(entity);

        await _auditService.CreateAsync(new AuditLogDto
        {
            TableName = "Funnelstage",
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

    public async Task UpdateAsync(FunnelStageDto dto)
    {
        var existingEntity = await _repo.GetByIdAsync(dto.Id);
        if (existingEntity == null) return;

        var oldValueJson = JsonConvert.SerializeObject(existingEntity, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });

        existingEntity.StageName = dto.StageName;
        existingEntity.StageOrder = dto.StageOrder;
        existingEntity.IsClosedStage = NormalizeStageFlag(dto.IsClosedStage);
        existingEntity.IsWonStage = NormalizeStageFlag(dto.IsWonStage);
        existingEntity.IsLostStage = NormalizeStageFlag(dto.IsLostStage);
        existingEntity.IsActive = dto.IsActive;
        existingEntity.UpdatedDate = DateTime.Now;

        await _repo.UpdateAsync(existingEntity);

        await _auditService.CreateAsync(new AuditLogDto
        {
            TableName = "Funnelstage",
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

    public async Task<IEnumerable<object>> GetDropdownAsync()
    {
        var data = await _repo.GetAllAsync();

        return data
            .OrderBy(x => x.StageOrder)
            .Select(x => new
            {
                value = x.Id,
                code = $"F-{(x.StageOrder ?? 0).ToString("D2")}",
                label = $"F-{(x.StageOrder ?? 0).ToString("D2")} - {x.StageName}"
            });
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return;

        var oldValueJson = JsonConvert.SerializeObject(entity, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });

        await _repo.DeleteAsync(id);

        await _auditService.CreateAsync(new AuditLogDto
        {
            TableName = "Funnelstage",
            RecordId = entity.Id,
            ActionType = "DELETE",
            OldValueJson = oldValueJson,
            NewValueJson = null,
            ActionBy = 1
        });
    }
}

