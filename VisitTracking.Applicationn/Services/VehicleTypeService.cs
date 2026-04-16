using Newtonsoft.Json;
using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;

public class VehicleTypeService : IVehicleTypeService
{
    private readonly IVehicleTypeRepository _repository;
    private readonly IAuditLogService _auditService;

    public VehicleTypeService(IVehicleTypeRepository repository, IAuditLogService auditLogService)
    {
        _repository = repository;
        _auditService = auditLogService;
    }

    public async Task<List<VehicleTypeDto>> GetAllAsync()
    {
        var data = await _repository.GetAllAsync();

        return data.Select(x => new VehicleTypeDto
        {
            VehicleName = x.VehicleName,
            DefaultRatePerKm = x.DefaultRatePerKm,
            IsActive = x.IsActive
        }).ToList();
    }

    public async Task<VehicleTypeDto?> GetByIdAsync(int id)
    {
        var x = await _repository.GetByIdAsync(id);

        if (x == null) return null;

        return new VehicleTypeDto
        {
            VehicleName = x.VehicleName,
            DefaultRatePerKm = x.DefaultRatePerKm,
            IsActive = x.IsActive
        };
    }

    public async Task Create(VehicleTypeDto dto)
    {
        var entity = new Vehicletype
        {
            VehicleName = dto.VehicleName,
            DefaultRatePerKm = dto.DefaultRatePerKm,
            IsActive = dto.IsActive ?? true
        };

        await _repository.AddAsync(entity);

        await _auditService.CreateAsync(new AuditLogDto
        {
            TableName = "VehicleType",
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

    public async Task UpdateAsync(int id, VehicleTypeDto dto)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null) return;

        var oldValueJson = JsonConvert.SerializeObject(entity, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        }) ?? string.Empty;

        entity.VehicleName = dto.VehicleName;
        entity.DefaultRatePerKm = dto.DefaultRatePerKm;
        entity.IsActive = dto.IsActive;

        await _repository.UpdateAsync(entity);

        await _auditService.CreateAsync(new AuditLogDto
        {
            TableName = "VehicleType",
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
        var oldEntity = await _repository.GetByIdAsync(id);

        await _repository.DeleteAsync(id);

        await _auditService.CreateAsync(new AuditLogDto
        {
            TableName = "VehicleType",
            RecordId = id,
            ActionType = "DELETE",
            OldValueJson = oldEntity == null ? JsonConvert.SerializeObject(new { Id = id }, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }) ?? string.Empty : JsonConvert.SerializeObject(oldEntity, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }) ?? string.Empty,
            NewValueJson = string.Empty,
            ActionBy = 1
        });
    }
}