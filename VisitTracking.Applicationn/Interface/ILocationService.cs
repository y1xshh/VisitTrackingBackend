using VisitTracking.Application.DTOs;

namespace VisitTracking.Application.Interface;

public interface ILocationService
{
    Task<IEnumerable<LocationResponseDto>> GetAllAsync();
    Task<LocationResponseDto?> GetByIdAsync(int id);
    Task<string> CreateAsync(LocationDto dto);
    Task<string> UpdateAsync(int id, LocationDto dto);
    Task<string> DeleteAsync(int id);
}