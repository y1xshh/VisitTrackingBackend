using Newtonsoft.Json;
using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _repo;
    private readonly IAuditLogService _auditService;

    public CompanyService(ICompanyRepository repo, IAuditLogService auditLogService)
    {
        _repo = repo;
        _auditService = auditLogService;
    }

    public async Task<IEnumerable<CompanyDto>> GetAllAsync()
    {
        var data = await _repo.GetAllAsync();

        return data.Select(x => new CompanyDto
        {
            Id = x.Id,
            CompanyName = x.CompanyName,
            CompanyType = x.CompanyType,
            IndustryType = x.IndustryType,
            Address = x.Address,
            City = x.City,
            State = x.State,
            Pincode = x.Pincode,
            IsActive = x.IsActive
        });
    }

    public async Task<CompanyDto?> GetByIdAsync(int id)
    {
        var x = await _repo.GetByIdAsync(id);
        if (x == null) return null;

        return new CompanyDto
        {
            Id = x.Id,
            CompanyName = x.CompanyName,
            CompanyType = x.CompanyType,
            IndustryType = x.IndustryType,
            Address = x.Address,
            City = x.City,
            State = x.State,
            Pincode = x.Pincode,
            IsActive = x.IsActive
        };
    }

    public async Task CreateAsync(CompanyDto dto)
    {
        
        if (string.IsNullOrEmpty(dto.CompanyName))
            throw new Exception("Company Name is required");

        var entity = new Company
        {
            CompanyName = dto.CompanyName,
            CompanyType = dto.CompanyType,
            IndustryType = dto.IndustryType,
            Address = dto.Address,
            City = dto.City,
            State = dto.State,
            Pincode = dto.Pincode,
            IsActive = true,
            InsertedDate = DateTime.Now
        };

        await _repo.AddAsync(entity);

        await _auditService.CreateAsync(new AuditLogDto
        {
            TableName = "Company",
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

    public async Task UpdateAsync(CompanyDto dto)
    {
        var oldEntity = await _repo.GetByIdAsync(dto.Id);

        var entity = new Company
        {
            Id = dto.Id,
            CompanyName = dto.CompanyName,
            CompanyType = dto.CompanyType,
            IndustryType = dto.IndustryType,
            Address = dto.Address,
            City = dto.City,
            State = dto.State,
            Pincode = dto.Pincode,
            IsActive = dto.IsActive,
            UpdatedDate = DateTime.Now
        };

        await _repo.UpdateAsync(entity);

        await _auditService.CreateAsync(new AuditLogDto
        {
            TableName = "Company",
            RecordId = entity.Id,
            ActionType = "UPDATE",
            OldValueJson = oldEntity == null ? null : JsonConvert.SerializeObject(oldEntity, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }),
            NewValueJson = JsonConvert.SerializeObject(entity, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }),
            ActionBy = 1
        });
    }

    public async Task DeleteAsync(int id)
    {
        var oldEntity = await _repo.GetByIdAsync(id);

        await _repo.DeleteAsync(id);

        await _auditService.CreateAsync(new AuditLogDto
        {
            TableName = "Company",
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