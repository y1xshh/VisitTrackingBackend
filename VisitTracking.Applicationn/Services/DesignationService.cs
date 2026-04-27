using Newtonsoft.Json;
using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;
using VisitTracking.Application.Interfaces;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;

namespace VisitTracking.Application.Services;

public class DesignationService : IDesignationService
{
    private readonly IDesignationRepository _repo;
    private readonly IAuditLogService _auditService;

    public DesignationService(IDesignationRepository repo, IAuditLogService auditLogService)
    {
        _repo = repo;
        _auditService = auditLogService;
    }

    public async Task<IEnumerable<DesignationResponseDto>> GetAllAsync()
    {
        var data = await _repo.GetAllAsync();

        return data.Select(x => new DesignationResponseDto
        {
            Id = x.Id,
            DesignationName = x.DesignationName,
            DepartmentId = x.DepartmentId,
            IsActive = x.IsActive
        }).ToList();
    }

    public async Task<DesignationResponseDto?> GetByIdAsync(int id)
    {
        var x = await _repo.GetByIdAsync(id);
        if (x == null) return null;

        return new DesignationResponseDto
        {
            Id = x.Id,
            DesignationName = x.DesignationName,
            DepartmentId = x.DepartmentId,
            IsActive = x.IsActive
        };
    }

    public async Task<string> CreateAsync(DesignationDto dto)
    {
        var entity = new MstDesignation
        {
            DesignationName = dto.DesignationName,
            DepartmentId = dto.DepartmentId,
            IsActive = true,
            InsertedDate = DateTime.UtcNow
        };

        await _repo.AddAsync(entity);

        await _auditService.CreateAsync(new AuditLogDto
        {
            TableName = "Designation",
            RecordId = entity.Id,
            ActionType = "INSERT",
            OldValueJson = null,
            NewValueJson = JsonConvert.SerializeObject(entity, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }),
            ActionBy = 1
        });

        return "Created Successfully";
    }

    public async Task<string> UpdateAsync(int id, DesignationDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return "Not Found";

        var oldValueJson = JsonConvert.SerializeObject(entity, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });

        entity.DesignationName = dto.DesignationName;
        entity.DepartmentId = dto.DepartmentId;
        entity.IsActive = dto.IsActive;
        entity.UpdatedDate = DateTime.UtcNow;

        await _repo.UpdateAsync(entity);

        await _auditService.CreateAsync(new AuditLogDto
        {
            TableName = "Designation",
            RecordId = entity.Id,
            ActionType = "UPDATE",
            OldValueJson = oldValueJson,
            NewValueJson = JsonConvert.SerializeObject(entity, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }),
            ActionBy = 1
        });

        return "Updated Successfully";
    }

    public async Task<string> DeleteAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return "Not Found";

        await _repo.DeleteAsync(entity);

        await _auditService.CreateAsync(new AuditLogDto
        {
            TableName = "Designation",
            RecordId = entity.Id,
            ActionType = "DELETE",
            OldValueJson = JsonConvert.SerializeObject(entity, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }),
            NewValueJson = null,
            ActionBy = 1
        });

        return "Deleted Successfully";
    }
}