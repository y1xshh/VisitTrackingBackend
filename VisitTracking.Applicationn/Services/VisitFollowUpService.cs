using Newtonsoft.Json;
using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;
using VisitTracking.Domain.Entities;

public class VisitFollowUpService : IVisitFollowUpService
{
    private readonly IVisitFollowUpRepository _repo;
    private readonly IAuditLogService _auditService;

    public VisitFollowUpService(IVisitFollowUpRepository repo, IAuditLogService auditLogService)
    {
        _repo = repo;
        _auditService = auditLogService;
    }

    public async Task<IEnumerable<VisitFollowUpDto>> GetAllAsync()
    {
        var data = await _repo.GetAllAsync();

        return data.Select(x => new VisitFollowUpDto
        {
            Id = x.Id,
            VisitId = x.VisitId,
            FollowUpDate = x.FollowUpDate,
            FollowUpRemarks = x.FollowUpRemarks,
            FunnelStageId = x.FunnelStageId,
            OutcomeTypeId = x.OutcomeTypeId,
            ExpectedBusinessValue = x.ExpectedBusinessValue,
            ActualBusinessValue = x.ActualBusinessValue,
            NextFollowUpDate = x.NextFollowUpDate,
            IsActive = x.IsActive
        });
    }

    public async Task<VisitFollowUpDto?> GetByIdAsync(int id)
    {
        var x = await _repo.GetByIdAsync(id);
        if (x == null) return null;

        return new VisitFollowUpDto
        {
            Id = x.Id,
            VisitId = x.VisitId,
            FollowUpDate = x.FollowUpDate,
            FollowUpRemarks = x.FollowUpRemarks,
            FunnelStageId = x.FunnelStageId,
            OutcomeTypeId = x.OutcomeTypeId,
            ExpectedBusinessValue = x.ExpectedBusinessValue,
            ActualBusinessValue = x.ActualBusinessValue,
            NextFollowUpDate = x.NextFollowUpDate,
            IsActive = x.IsActive
        };
    }

    public async Task CreateAsync(VisitFollowUpDto dto)
    {
        var entity = new Visitfollowup
        {
            VisitId = dto.VisitId,
            FollowUpDate = dto.FollowUpDate,
            FollowUpRemarks = dto.FollowUpRemarks,
            FunnelStageId = dto.FunnelStageId,
            OutcomeTypeId = dto.OutcomeTypeId,
            ExpectedBusinessValue = dto.ExpectedBusinessValue,
            ActualBusinessValue = dto.ActualBusinessValue,
            NextFollowUpDate = dto.NextFollowUpDate,
            IsActive = dto.IsActive,
            InsertedDate = DateTime.Now
        };

        if (dto.NextFollowUpDate == null && dto.FollowUpDate != null)
        {
            entity.NextFollowUpDate = dto.FollowUpDate.Value.AddDays(2);
        }
        else
        {
            entity.NextFollowUpDate = dto.NextFollowUpDate;
        }

        if (dto.OutcomeTypeId == 1)
        {
            entity.ActualBusinessValue = dto.ExpectedBusinessValue;
        }
        else
        {
            entity.ActualBusinessValue = dto.ActualBusinessValue;
        }

        await _repo.AddAsync(entity);

        await _auditService.CreateAsync(new AuditLogDto
        {
            TableName = "Visitfollowup",
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

    public async Task UpdateAsync(VisitFollowUpDto dto)
    {
        var existingEntity = await _repo.GetByIdAsync(dto.Id);
        if (existingEntity == null) return;

        var oldValueJson = JsonConvert.SerializeObject(existingEntity, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });

        existingEntity.VisitId = dto.VisitId;
        existingEntity.FollowUpDate = dto.FollowUpDate;
        existingEntity.FollowUpRemarks = dto.FollowUpRemarks;
        existingEntity.FunnelStageId = dto.FunnelStageId;
        existingEntity.OutcomeTypeId = dto.OutcomeTypeId;
        existingEntity.ExpectedBusinessValue = dto.ExpectedBusinessValue;
        existingEntity.ActualBusinessValue = dto.ActualBusinessValue;
        existingEntity.NextFollowUpDate = dto.NextFollowUpDate;
        existingEntity.IsActive = dto.IsActive;
        existingEntity.UpdatedDate = DateTime.Now;

        if (dto.NextFollowUpDate == null && dto.FollowUpDate != null)
        {
            existingEntity.NextFollowUpDate = dto.FollowUpDate.Value.AddDays(2);
        }
        else
        {
            existingEntity.NextFollowUpDate = dto.NextFollowUpDate;
        }

        if (dto.OutcomeTypeId == 1)
        {
            existingEntity.ActualBusinessValue = dto.ExpectedBusinessValue;
        }
        else
        {
            existingEntity.ActualBusinessValue = dto.ActualBusinessValue;
        }

        await _repo.UpdateAsync(existingEntity);

        await _auditService.CreateAsync(new AuditLogDto
        {
            TableName = "Visitfollowup",
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
            TableName = "Visitfollowup",
            RecordId = id,
            ActionType = "DELETE",
            OldValueJson = oldValueJson,
            NewValueJson = null,
            ActionBy = 1
        });
    }
}