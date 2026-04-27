using VisitTracking.Application.DTOs;

namespace VisitTracking.Application.Interface
{
    public interface IExpenseApprovalService
    {
        Task<IEnumerable<ExpenseApprovalResponseDto>> GetAllAsync();
        Task<ExpenseApprovalResponseDto?> GetByIdAsync(int id);
        Task CreateAsync(ExpenseApprovalDto dto);
        Task UpdateAsync(int id, ExpenseApprovalDto dto);
        Task DeleteAsync(int id);

        Task ApproveAsync(int id, int approvedBy, string? remarks);
        Task RejectAsync(int id, int approvedBy, string? remarks);
    }
}