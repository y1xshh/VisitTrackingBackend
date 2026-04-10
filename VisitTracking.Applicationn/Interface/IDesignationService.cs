using VisitTracking.Application.DTOs;

namespace VisitTracking.Application.Interfaces;

public interface IDesignationService
{
    Task<List<DesignationDto>> GetAllAsync();
    Task<DesignationDto?> GetByIdAsync(int id);
    Task<string> CreateAsync(DesignationDto dto);
    Task<string> UpdateAsync(int id, DesignationDto dto);
    Task<string> DeleteAsync(int id);
}