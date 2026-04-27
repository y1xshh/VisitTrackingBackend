using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;
using VisitTracking.Domain.Entities;

public class AuditLogService : IAuditLogService
{
    private readonly IAuditLogRepository _repo;

    public AuditLogService(IAuditLogRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<AuditLogDto>> GetAllAsync()
    {
        var data = await _repo.GetAllAsync();

        return data.Select(x => new AuditLogDto
        {
            Id = x.Id,
            TableName = x.TableName,
            RecordId = x.RecordId,
            ActionType = x.ActionType,
            OldValueJson = x.OldValueJson,
            NewValueJson = x.NewValueJson,
            ActionBy = x.ActionBy,
   
        });
    }

    public async Task CreateAsync(AuditLogDto dto)
    {
        var entity = new Auditlog
        {
            TableName = dto.TableName,
            RecordId = dto.RecordId,
            ActionType = dto.ActionType,
            OldValueJson = dto.OldValueJson,
            NewValueJson = dto.NewValueJson,
            ActionBy = dto.ActionBy,
            InsertedDate = DateTime.Now
        };

        await _repo.AddAsync(entity);
    }
}