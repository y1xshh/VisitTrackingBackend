using VisitTracking.Application.DTOs;

namespace VisitTracking.Application.Interface
{
    public interface IExpenserateService
    {
        Task<IEnumerable<ExpenserateResponseDto>> GetAllAsync();
        Task<ExpenserateResponseDto?> GetByIdAsync(int id);
        Task CreateAsync(ExpenserateDto dto);
        Task UpdateAsync(int id, ExpenserateDto dto);
        Task DeleteAsync(int id);
    }
}