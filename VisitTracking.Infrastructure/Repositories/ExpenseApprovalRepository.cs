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
        return await _context.ExpenseApprovals.ToListAsync();
    }

    public async Task<ExpenseApproval?> GetByIdAsync(int id)
    {
        return await _context.ExpenseApprovals.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(ExpenseApproval entity)
    {
        await ValidateVisitExistsAsync(entity.VisitId);

        try
        {
            await _context.ExpenseApprovals.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new Exception("Failed to save Expense Approval. Invalid foreign key.", ex);
        }
    }

    public async Task UpdateAsync(ExpenseApproval entity)
    {
        await ValidateVisitExistsAsync(entity.VisitId);

        try
        {
            _context.ExpenseApprovals.Update(entity);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new Exception("Failed to update Expense Approval. Invalid foreign key.", ex);
        }
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

    private async Task ValidateVisitExistsAsync(int? visitId)
    {
        if (!visitId.HasValue)
        {
            throw new Exception("VisitId is required.");
        }

        var visitExists = await _context.Visits.AnyAsync(x => x.Id == visitId.Value);
        if (!visitExists)
        {
            throw new Exception($"Visit with Id {visitId.Value} does not exist.");
        }
    }
}
