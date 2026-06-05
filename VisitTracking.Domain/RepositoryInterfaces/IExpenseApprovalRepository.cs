using VisitTracking.Domain.Entities;

public interface IExpenseApprovalRepository
{
    Task<IEnumerable<ExpenseApproval>> GetAllAsync();
    Task<ExpenseApproval?> GetByIdAsync(int id);
    Task AddAsync(ExpenseApproval entity);
    Task UpdateAsync(ExpenseApproval entity);
    Task DeleteAsync(int id);
}
