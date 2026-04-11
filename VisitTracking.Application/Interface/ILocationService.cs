using VisitTracking.Application.DTOs;

namespace VisitTracking.Application.Interface;

public interface ILocationService
{
    Task<List<LocationDto>> GetAllAsync();
    Task<LocationDto?> GetByIdAsync(int id);
    Task<string> CreateAsync(LocationDto dto);
    Task<string> UpdateAsync(int id, LocationDto dto);
    Task<string> DeleteAsync(int id);
}