using VisitTracking.Application.DTOs;

public interface IVisitPurposeService
{
    Task<IEnumerable<VisitPurposeResponseDto>> GetAllAsync();
    Task<VisitPurposeResponseDto?> GetByIdAsync(int id);
    Task Create(VisitPurposeDto dto);
    Task UpdateAsync(int id, VisitPurposeDto dto);
    Task DeleteAsync(int id);
}