using Newtonsoft.Json;
using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;

namespace VisitTracking.Application.Services;

public class ContactpersonService : IContactpersonService
{
    private readonly IContactpersonRepository _repository;
    private readonly IAuditLogService _auditService;

    public ContactpersonService(IContactpersonRepository repository, IAuditLogService auditLogService)
    {
        _repository = repository;
        _auditService = auditLogService;
    }

    public async Task<List<ContactpersonDto>> GetAllAsync()
    {
        var data = await _repository.GetAllAsync();

        return data.Select(x => new ContactpersonDto
        {
            Id = x.Id,
            Name = x.Name,
            Designation = x.Designation,
            Mobile = x.Mobile,
            Email = x.Email,
            CompanyId = x.CompanyId,
            OrganisationId = x.OrganisationId,
            DepartmentId = x.DepartmentId,
            Remark = x.Remarks,
            IsActive = x.IsActive ?? false,
            CompanyName = x.Company?.CompanyName,
            DepartmentName = x.Department?.DepartmentName,
            OrganisationName = x.Organisation?.OrganisationName
        }).ToList();
    }

    public async Task<ContactpersonDto?> GetByIdAsync(int id)
    {
        var x = await _repository.GetByIdAsync(id);
        if (x == null) return null;

        return new ContactpersonDto
        {
            Id = x.Id,
            Name = x.Name,
            Designation = x.Designation,
            Mobile = x.Mobile,
            Email = x.Email,
            Remark = x.Remarks,
            IsActive = x.IsActive ?? false,
            CompanyId = x.CompanyId,
            OrganisationId = x.OrganisationId,
            DepartmentId = x.DepartmentId,
            CompanyName = x.Company?.CompanyName,
            DepartmentName = x.Department?.DepartmentName,
            OrganisationName = x.Organisation?.OrganisationName
        };
    }

    public async Task Create(ContactpersonDto dto)
    {
        var entity = new Contactperson
        {
            CompanyId = dto.CompanyId,
            OrganisationId = dto.OrganisationId,
            DepartmentId = dto.DepartmentId,
            Name = dto.Name,
            Designation = dto.Designation,
            Mobile = dto.Mobile,
            Email = dto.Email,
            Remarks = dto.Remark,
            IsActive = dto.IsActive,
            InsertedBy = "system",
            InsertedDate = DateTime.UtcNow
        };

        await _repository.AddAsync(entity);

        await _auditService.CreateAsync(new AuditLogDto
        {
            TableName = "Contactperson",
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

    public async Task UpdateAsync(int id, ContactpersonDto dto)
    {
        var data = await _repository.GetByIdAsync(id);
        if (data == null) return;

        var oldValueJson = JsonConvert.SerializeObject(data, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        }) ?? string.Empty;

        data.CompanyId = dto.CompanyId;
        data.OrganisationId = dto.OrganisationId;
        data.DepartmentId = dto.DepartmentId;
        data.Name = dto.Name;
        data.Designation = dto.Designation;
        data.Mobile = dto.Mobile;
        data.Email = dto.Email;
        data.Remarks = dto.Remark;
        data.IsActive = dto.IsActive;
        data.UpdatedBy = "system";
        data.UpdatedDate = DateTime.UtcNow;

        await _repository.UpdateAsync(data);

        await _auditService.CreateAsync(new AuditLogDto
        {
            TableName = "Contactperson",
            RecordId = data.Id,
            ActionType = "UPDATE",
            OldValueJson = oldValueJson,
            NewValueJson = JsonConvert.SerializeObject(data, new JsonSerializerSettings
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
            TableName = "Contactperson",
            RecordId = entity.Id,
            ActionType = "DELETE",
            OldValueJson = oldValueJson,
            NewValueJson = JsonConvert.SerializeObject(entity, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }) ?? string.Empty,
            ActionBy = 1
        });
    }

    public async Task<ContactpersonDto?> GetByEmailAsync(string email)
    {
        var x = await _repository.GetByEmailAsync(email);
        if (x == null) return null;

        return new ContactpersonDto
        {
            Id = x.Id,
            Name = x.Name,
            Designation = x.Designation,
            Mobile = x.Mobile,
            Email = x.Email,
            Remark = x.Remarks,
            IsActive = x.IsActive ?? false,
            CompanyId = x.CompanyId,
            OrganisationId = x.OrganisationId,
            DepartmentId = x.DepartmentId,
            CompanyName = x.Company?.CompanyName,
            DepartmentName = x.Department?.DepartmentName,
            OrganisationName = x.Organisation?.OrganisationName
        };
    }
}

