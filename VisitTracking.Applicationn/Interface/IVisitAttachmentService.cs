using VisitTracking.Application.DTOs;

namespace VisitTracking.Application.Interface
{
    public interface IVisitAttachmentService
    {
        Task<IEnumerable<VisitAttachmentResponseDto>> GetAllAsync();
        Task<VisitAttachmentResponseDto?> GetByIdAsync(int id);
        Task CreateAsync(VisitAttachmentDto dto);
        Task UpdateAsync(int id, VisitAttachmentDto dto);
        Task DeleteAsync(int id);
    }
}