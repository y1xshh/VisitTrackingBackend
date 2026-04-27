using VisitTracking.Application.DTOs;

namespace VisitTracking.Application.Interface
{
    public interface IFunnelStageService
    {
        Task<IEnumerable<FunnelStageResponseDto>> GetAllAsync();
        Task<FunnelStageResponseDto?> GetByIdAsync(int id);
        Task CreateAsync(FunnelStageDto dto);
        Task UpdateAsync(int id, FunnelStageDto dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<object>> GetDropdownAsync();
    }
}