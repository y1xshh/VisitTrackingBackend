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

    public async Task<IEnumerable<Expenseapproval>> GetAllAsync()
    {
        return await _context.Expenseapprovals
            .Include(x => x.Visit)
            .ToListAsync();
    }

    public async Task<Expenseapproval?> GetByIdAsync(int id)
    {
        return await _context.Expenseapprovals
            .Include(x => x.Visit)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(Expenseapproval entity)
    {
        await _context.Expenseapprovals.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Expenseapproval entity)
    {
        _context.Expenseapprovals.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var data = await _context.Expenseapprovals.FindAsync(id);
        if (data != null)
        {
            _context.Expenseapprovals.Remove(data);
            await _context.SaveChangesAsync();
        }
    }
}