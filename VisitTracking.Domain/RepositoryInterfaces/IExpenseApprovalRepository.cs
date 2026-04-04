using VisitTracking.Domain.Entities;

public interface IExpenseApprovalRepository
{
    Task<IEnumerable<Expenseapproval>> GetAllAsync();
    Task<Expenseapproval?> GetByIdAsync(int id);
    Task AddAsync(Expenseapproval entity);
    Task UpdateAsync(Expenseapproval entity);
    Task DeleteAsync(int id);
}
