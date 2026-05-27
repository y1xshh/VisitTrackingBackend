using VisitTracking.Application.DTOs;

namespace VisitTracking.Application.Interface
{
    public interface IVisitService
    {
        Task<IEnumerable<VisitResponseDto>> GetAllAsync();
        Task<VisitResponseDto?> GetByIdAsync(int id);
        Task Create(CreateVisitDto dto);
        Task UpdateAsync(int id, CreateVisitDto dto);
        Task DeleteAsync(int id);

        Task<ApiResponse<VisitApprovalResponseDto>> ApproveVisitAsync(
            int visitId,
            VisitApprovalRequestDto request,
            string? userId,
            string? role);
    }
}
