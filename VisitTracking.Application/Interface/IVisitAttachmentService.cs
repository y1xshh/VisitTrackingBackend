using VisitTracking.Application.DTOs;

namespace VisitTracking.Application.Interface
{
    public interface IVisitAttachmentService
    {
        Task<IEnumerable<VisitAttachmentDto>> GetAllAsync();
        Task<VisitAttachmentDto?> GetByIdAsync(int id);
        Task CreateAsync(VisitAttachmentDto dto);
        Task UpdateAsync(VisitAttachmentDto dto);
        Task DeleteAsync(int id);
    }
}
