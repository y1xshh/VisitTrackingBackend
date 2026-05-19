using Newtonsoft.Json;
using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;
using VisitTracking.Domain.Entities;

public class ExpenseApprovalService : IExpenseApprovalService
{
    private readonly IExpenseApprovalRepository _repo;
    private readonly IAuditLogService _auditService;

    public ExpenseApprovalService(IExpenseApprovalRepository repo, IAuditLogService auditLogService)
    {
        _repo = repo;
        _auditService = auditLogService;
    }

    public async Task<IEnumerable<ExpenseApprovalResponseDto>> GetAllAsync()
    {
        var data = await _repo.GetAllAsync();

        return data.Select(x => new ExpenseApprovalResponseDto
        {
            Id = x.Id,
            VisitId = x.VisitId,
            SubmittedBy = x.SubmittedBy,
            ApprovedBy = x.ApprovedBy,
            ApprovalStatus = x.ApprovalStatus,
            ApprovalRemarks = x.ApprovalRemarks,
            SubmittedAt = x.SubmittedAt,
            ApprovedAt = x.ApprovedAt,
            IsActive = x.IsActive
        });
    }

    public async Task<ExpenseApprovalResponseDto?> GetByIdAsync(int id)
    {
        var x = await _repo.GetByIdAsync(id);
        if (x == null) return null;

        return new ExpenseApprovalResponseDto
        {
            Id = x.Id,
            VisitId = x.VisitId,
            SubmittedBy = x.SubmittedBy,
            ApprovedBy = x.ApprovedBy,
            ApprovalStatus = x.ApprovalStatus,
            ApprovalRemarks = x.ApprovalRemarks,
            SubmittedAt = x.SubmittedAt,
            ApprovedAt = x.ApprovedAt,
            IsActive = x.IsActive
        };
    }

    public async Task CreateAsync(ExpenseApprovalDto dto)
    {
        var entity = new ExpenseApproval
        {
            VisitId = dto.VisitId,
            SubmittedBy = dto.SubmittedBy,
            ApprovalStatus = "Pending",
            SubmittedAt = DateTime.UtcNow,
            IsActive = true,
            InsertedDate = DateTime.UtcNow
        };

        await _repo.AddAsync(entity);

        await _auditService.CreateAsync(new AuditLogDto
        {
            TableName = "ExpenseApproval",
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

    public async Task UpdateAsync(int id, ExpenseApprovalDto dto)
    {
        var existingEntity = await _repo.GetByIdAsync(id);
        var oldValueJson = existingEntity != null
            ? JsonConvert.SerializeObject(existingEntity, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            })
            : null;

        var entity = new ExpenseApproval
        {
            Id = id,
            VisitId = dto.VisitId,
            SubmittedBy = dto.SubmittedBy,
            ApprovedBy = dto.ApprovedBy,
            ApprovalStatus = dto.ApprovalStatus,
            ApprovalRemarks = dto.ApprovalRemarks,
            SubmittedAt = dto.SubmittedAt,
            ApprovedAt = dto.ApprovedAt,
            IsActive = dto.IsActive,
            UpdatedDate = DateTime.UtcNow
        };

        await _repo.UpdateAsync(entity);

        await _auditService.CreateAsync(new AuditLogDto
        {
            TableName = "ExpenseApproval",
            RecordId = entity.Id,
            ActionType = "UPDATE",
            OldValueJson = oldValueJson,
            NewValueJson = JsonConvert.SerializeObject(entity, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }),
            ActionBy = 1
        });
    }

    public async Task DeleteAsync(int id)
    {
        var existingEntity = await _repo.GetByIdAsync(id);
        var oldValueJson = existingEntity != null
            ? JsonConvert.SerializeObject(existingEntity, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            })
            : null;

        await _repo.DeleteAsync(id);

        await _auditService.CreateAsync(new AuditLogDto
        {
            TableName = "ExpenseApproval",
            RecordId = id,
            ActionType = "DELETE",
            OldValueJson = oldValueJson,
            NewValueJson = null,
            ActionBy = 1
        });
    }

    public async Task ApproveAsync(int id, int approvedBy, string? remarks)
    {
        var data = await _repo.GetByIdAsync(id);
        if (data == null) throw new Exception("Request not found");

        var oldValueJson = JsonConvert.SerializeObject(data, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });

        data.ApprovalStatus = "Approved";
        data.ApprovedBy = approvedBy;
        data.ApprovalRemarks = remarks;
        data.ApprovedAt = DateTime.UtcNow;

        await _repo.UpdateAsync(data);

        await _auditService.CreateAsync(new AuditLogDto
        {
            TableName = "ExpenseApproval",
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

    public async Task RejectAsync(int id, int approvedBy, string? remarks)
    {
        var data = await _repo.GetByIdAsync(id);
        if (data == null) throw new Exception("Request not found");

        var oldValueJson = JsonConvert.SerializeObject(data, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });

        data.ApprovalStatus = "Rejected";
        data.ApprovedBy = approvedBy;
        data.ApprovalRemarks = remarks;
        data.ApprovedAt = DateTime.UtcNow;

        await _repo.UpdateAsync(data);

        await _auditService.CreateAsync(new AuditLogDto
        {
            TableName = "ExpenseApproval",
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
}
