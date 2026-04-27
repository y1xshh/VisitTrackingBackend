using Newtonsoft.Json;
using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;
using VisitTracking.Application.Interfaces;
using VisitTracking.Domain.Entities;

namespace VisitTracking.Application.Services;

public class LocationService : ILocationService
{
    private readonly ILocationRepository _repo;
    private readonly IAuditLogService _auditService;

    public LocationService(ILocationRepository repo, IAuditLogService auditLogService)
    {
        _repo = repo;
        _auditService = auditLogService;
    }

    public async Task<IEnumerable<LocationResponseDto>> GetAllAsync()
    {
        var data = await _repo.GetAllAsync();

        return data.Select(x => new LocationResponseDto
        {
            Id = x.Id,
            LocationName = x.LocationName,
            State = x.State,
            Country = x.Country,
            IsActive = x.IsActive
        }).ToList();
    }

    public async Task<LocationResponseDto?> GetByIdAsync(int id)
    {
        var x = await _repo.GetByIdAsync(id);
        if (x == null) return null;

        return new LocationResponseDto
        {
            Id = x.Id,
            LocationName = x.LocationName,
            State = x.State,
            Country = x.Country,
            IsActive = x.IsActive
        };
    }

    public async Task<string> CreateAsync(LocationDto dto)
    {
        var entity = new MstLocation
        {
            LocationName = dto.LocationName,
            State = dto.State,
            Country = dto.Country,
            IsActive = true,
            InsertedDate = DateTime.UtcNow
        };

        await _repo.AddAsync(entity);

        await _auditService.CreateAsync(new AuditLogDto
        {
            TableName = "Location",
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

    public async Task<string> UpdateAsync(int id, LocationDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return "Not Found";

        var oldValueJson = JsonConvert.SerializeObject(entity, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });

        entity.LocationName = dto.LocationName;
        entity.State = dto.State;
        entity.Country = dto.Country;
        entity.IsActive = dto.IsActive;
        entity.UpdatedDate = DateTime.UtcNow;

        await _repo.UpdateAsync(entity);

        await _auditService.CreateAsync(new AuditLogDto
        {
            TableName = "Location",
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
            TableName = "Location",
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