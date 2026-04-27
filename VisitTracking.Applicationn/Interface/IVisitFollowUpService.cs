using VisitTracking.Application.DTOs;

namespace VisitTracking.Application.Interface
{
    public interface IVisitFollowUpService
    {
        Task<IEnumerable<VisitFollowUpResponseDto>> GetAllAsync();
        Task<VisitFollowUpResponseDto?> GetByIdAsync(int id);
        Task CreateAsync(VisitFollowUpDto dto);
        Task UpdateAsync(int id, VisitFollowUpDto dto);
        Task DeleteAsync(int id);
    }
}