using VisitTracking.Application.DTOs;

public interface IOutcomeTypeService
{
    Task<IEnumerable<OutcomeTypeDto>> GetAllAsync();
    Task<OutcomeTypeDto?> GetByIdAsync(int id);
    Task CreateAsync(OutcomeTypeDto dto);
    Task UpdateAsync(OutcomeTypeDto dto);
    Task DeleteAsync(int id);
}