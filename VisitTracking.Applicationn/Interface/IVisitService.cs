using VisitTracking.Application.DTOs;

namespace VisitTracking.Application.Interface
{
    public interface IVisitService
    {
        Task<IEnumerable<dynamic>> GetAllAsync(int flag);
        Task<VisitResponseDto?> GetByIdAsync(int id);
        Task Create(CreateVisitDto dto);
        Task UpdateAsync(int id, CreateVisitDto dto);
        Task DeleteAsync(int id);

        Task<VisitApprovalResponseDto> ApproveVisitAsync(
            int visitId,
            VisitApprovalRequestDto request);
    }
}
