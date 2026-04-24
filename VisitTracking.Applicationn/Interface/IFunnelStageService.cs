using VisitTracking.Application.DTOs;

namespace VisitTracking.Application.Interface
{
    public interface IFunnelStageService
    {
        Task<IEnumerable<FunnelStageDto>> GetAllAsync();
        Task<FunnelStageDto?> GetByIdAsync(int id);
        Task CreateAsync(FunnelStageDto dto);
        Task UpdateAsync(FunnelStageDto dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<object>> GetDropdownAsync();
    }
}