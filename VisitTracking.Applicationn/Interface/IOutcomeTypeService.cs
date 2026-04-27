using VisitTracking.Application.DTOs;

namespace VisitTracking.Application.Interface;

public interface IOutcomeTypeService
{
    Task<IEnumerable<OutcomeTypeResponseDto>> GetAllAsync();
    Task<OutcomeTypeResponseDto?> GetByIdAsync(int id);
    Task CreateAsync(OutcomeTypeDto dto);
    Task UpdateAsync(int id, OutcomeTypeDto dto);
    Task DeleteAsync(int id);
    Task<IEnumerable<object>> GetDropdownAsync();
}