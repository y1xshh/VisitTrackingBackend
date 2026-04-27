using Newtonsoft.Json;
using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;

namespace VisitTracking.Application.Services;

public class OrganisationService : IOrganisationService
{
    private readonly IOrganisationRepository _repository;
    private readonly IAuditLogService _auditService;

    public OrganisationService(IOrganisationRepository repository, IAuditLogService auditLogService)
    {
        _repository = repository;
        _auditService = auditLogService;
    }

    public async Task<List<OrganisationResponseDto>> GetAllAsync()
    {
        var orgs = await _repository.GetAllAsync();

        return orgs.Select(x => new OrganisationResponseDto
        {
            Id = x.Id,
            OrganisationName = x.OrganisationName ?? string.Empty,
            CompanyId = x.CompanyId.GetValueOrDefault(),
            CompanyName = x.Company?.CompanyName ?? string.Empty,
            Address = x.Address,
            City = x.City,
            State = x.State
        }).ToList();
    }

    public async Task<OrganisationResponseDto?> GetByIdAsync(int id)
    {
        var x = await _repository.GetByIdAsync(id);
        if (x == null) return null;

        return new OrganisationResponseDto
        {
            Id = x.Id,
            OrganisationName = x.OrganisationName ?? string.Empty,
            CompanyId = x.CompanyId.GetValueOrDefault(),
            CompanyName = x.Company?.CompanyName ?? string.Empty,
            Address = x.Address,
            City = x.City,
            State = x.State
        };
    }

    public async Task AddAsync(OrganisationDto dto)
    {
        var entity = new Organisation
        {
            OrganisationName = dto.OrganisationName,
            CompanyId = dto.CompanyId,
            Address = dto.Address,
            City = dto.City,
            State = dto.State,
            IsActive = true,
            InsertedBy = "System",
            InsertedDate = DateTime.UtcNow
        };

        await _repository.AddAsync(entity);

        await _auditService.CreateAsync(new AuditLogDto
        {
            TableName = "Organisation",
            RecordId = entity.Id,
            ActionType = "INSERT",
            OldValueJson = string.Empty,
            NewValueJson = JsonConvert.SerializeObject(entity, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }) ?? string.Empty,
            ActionBy = 1
        });
    }

    public async Task UpdateAsync(int id, OrganisationDto dto)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null) return;

        var oldValueJson = JsonConvert.SerializeObject(entity, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        }) ?? string.Empty;

        entity.OrganisationName = dto.OrganisationName;
        entity.CompanyId = dto.CompanyId;
        entity.Address = dto.Address;
        entity.City = dto.City;
        entity.State = dto.State;
        entity.UpdatedBy = "System";
        entity.UpdatedDate = DateTime.UtcNow;

        await _repository.UpdateAsync(entity);

        await _auditService.CreateAsync(new AuditLogDto
        {
            TableName = "Organisation",
            RecordId = entity.Id,
            ActionType = "UPDATE",
            OldValueJson = oldValueJson,
            NewValueJson = JsonConvert.SerializeObject(entity, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }) ?? string.Empty,
            ActionBy = 1
        });
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null) return;

        var oldValueJson = JsonConvert.SerializeObject(entity, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        }) ?? string.Empty;

        entity.IsActive = false;
        entity.UpdatedDate = DateTime.UtcNow;

        await _repository.UpdateAsync(entity);

        await _auditService.CreateAsync(new AuditLogDto
        {
            TableName = "Organisation",
            RecordId = entity.Id,
            ActionType = "DELETE",
            OldValueJson = oldValueJson,
            NewValueJson = string.Empty,
            ActionBy = 1
        });
    }
}
