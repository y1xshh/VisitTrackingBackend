using VisitTracking.Application.DTOs;

namespace VisitTracking.Application.Interface
{
    public interface IVisitFollowUpService
    {
        Task<IEnumerable<VisitFollowUpDto>> GetAllAsync();
        Task<VisitFollowUpDto?> GetByIdAsync(int id);
        Task CreateAsync(VisitFollowUpDto dto);
        Task UpdateAsync(VisitFollowUpDto dto);
        Task DeleteAsync(int id);
    }
}
