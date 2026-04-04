using Microsoft.EntityFrameworkCore;
using VisitTracking.Domain.Entities;
using VisitTracking.Infrastructure.Data;

public class OutcomeTypeRepository : IOutcomeTypeRepository
{
    private readonly AppDbContext _context;

    public OutcomeTypeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Outcometype>> GetAllAsync()
    {
        return await _context.Outcometypes.ToListAsync();
    }

    public async Task<Outcometype?> GetByIdAsync(int id)
    {
        return await _context.Outcometypes.FindAsync(id);
    }

    public async Task AddAsync(Outcometype entity)
    {
        await _context.Outcometypes.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Outcometype entity)
    {
        _context.Outcometypes.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var data = await _context.Outcometypes.FindAsync(id);
        if (data != null)
        {
            _context.Outcometypes.Remove(data);
            await _context.SaveChangesAsync();
        }
    }
}