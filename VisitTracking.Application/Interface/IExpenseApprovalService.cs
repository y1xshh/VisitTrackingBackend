using VisitTracking.Application.DTOs;

namespace VisitTracking.Application.Interface
{
    public interface IExpenseApprovalService
    {
        Task<IEnumerable<ExpenseApprovalDto>> GetAllAsync();
        Task<ExpenseApprovalDto?> GetByIdAsync(int id);
        Task CreateAsync(ExpenseApprovalDto dto);
        Task UpdateAsync(ExpenseApprovalDto dto);
        Task DeleteAsync(int id);

        // 🔥 Special actions
        Task ApproveAsync(int id, int approvedBy, string? remarks);
        Task RejectAsync(int id, int approvedBy, string? remarks);
    }
}
