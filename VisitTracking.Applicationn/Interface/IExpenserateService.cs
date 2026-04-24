using VisitTracking.Application.DTOs;

namespace VisitTracking.Application.Interface
{
    public interface IExpenserateService
    {
        Task<IEnumerable<ExpenserateDto>> GetAllAsync();
        Task<ExpenserateDto?> GetByIdAsync(int id);
        Task CreateAsync(ExpenserateDto dto);
        Task UpdateAsync(ExpenserateDto dto);
        Task DeleteAsync(int id);
    }
}