using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;

public class VehicleTypeService : IVehicleTypeService
{
    private readonly IVehicleTypeRepository _repository;

    public VehicleTypeService(IVehicleTypeRepository repository)
    {
        _repository = repository;
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

    public async Task<VehicleTypeDto> GetByIdAsync(int id)
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
    }

    public async Task UpdateAsync(int id, VehicleTypeDto dto)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null) return;

        entity.VehicleName = dto.VehicleName;
        entity.DefaultRatePerKm = dto.DefaultRatePerKm;
        entity.IsActive = dto.IsActive;

        await _repository.UpdateAsync(entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}