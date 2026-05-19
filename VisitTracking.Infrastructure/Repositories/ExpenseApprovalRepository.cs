using Microsoft.EntityFrameworkCore;
using VisitTracking.Domain.Entities;
using VisitTracking.Infrastructure.Data;

public class ExpenseApprovalRepository : IExpenseApprovalRepository
{
    private readonly AppDbContext _context;

    public ExpenseApprovalRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ExpenseApproval>> GetAllAsync()
    {
        return await _context.ExpenseApprovals
            .Include(x => x.Visit)
            .ToListAsync();
    }

    public async Task<ExpenseApproval?> GetByIdAsync(int id)
    {
        return await _context.ExpenseApprovals
            .Include(x => x.Visit)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(ExpenseApproval entity)
    {
        await _context.ExpenseApprovals.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ExpenseApproval entity)
    {
        _context.ExpenseApprovals.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var data = await _context.ExpenseApprovals.FindAsync(id);
        if (data != null)
        {
            _context.ExpenseApprovals.Remove(data);
            await _context.SaveChangesAsync();
        }
    }
}
