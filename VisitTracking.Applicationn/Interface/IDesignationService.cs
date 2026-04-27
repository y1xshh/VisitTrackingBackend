using VisitTracking.Application.DTOs;

namespace VisitTracking.Application.Interfaces;

public interface IDesignationService
{
    Task<IEnumerable<DesignationResponseDto>> GetAllAsync();
    Task<DesignationResponseDto?> GetByIdAsync(int id);
    Task<string> CreateAsync(DesignationDto dto);
    Task<string> UpdateAsync(int id, DesignationDto dto);
    Task<string> DeleteAsync(int id);
}