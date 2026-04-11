using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;
using VisitTracking.Application.Interfaces;
using VisitTracking.Domain.Entities;

namespace VisitTracking.Application.Services;

public class LocationService : ILocationService
{
    private readonly ILocationRepository _repo;

    public LocationService(ILocationRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<LocationDto>> GetAllAsync()
    {
        var data = await _repo.GetAllAsync();

        return data.Select(x => new LocationDto
        {
            Id = x.Id,
            LocationName = x.LocationName,
            State = x.State,
            Country = x.Country,
            IsActive = x.IsActive
        }).ToList();
    }

    public async Task<LocationDto?> GetByIdAsync(int id)
    {
        var x = await _repo.GetByIdAsync(id);
        if (x == null) return null;

        return new LocationDto
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
        return "Created Successfully";
    }

    public async Task<string> UpdateAsync(int id, LocationDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return "Not Found";

        entity.LocationName = dto.LocationName;
        entity.State = dto.State;
        entity.Country = dto.Country;
        entity.IsActive = dto.IsActive;
        entity.UpdatedDate = DateTime.UtcNow;

        await _repo.UpdateAsync(entity);
        return "Updated Successfully";
    }

    public async Task<string> DeleteAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return "Not Found";

        await _repo.DeleteAsync(entity);
        return "Deleted Successfully";
    }
}