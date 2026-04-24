using Microsoft.EntityFrameworkCore;
using VisitTracking.Domain.Entities;
using VisitTracking.Infrastructure.Data;

public class VisitFollowUpRepository : IVisitFollowUpRepository
{
    private readonly AppDbContext _context;

    public VisitFollowUpRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Visitfollowup>> GetAllAsync()
    {
        return await _context.Visitfollowups
            .Include(x => x.Visit) 
            .OrderByDescending(x => x.FollowUpDate)
            .ToListAsync();
    }

    public async Task<Visitfollowup?> GetByIdAsync(int id)
    {
        return await _context.Visitfollowups
            .Include(x => x.Visit)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(Visitfollowup entity)
    {
        await _context.Visitfollowups.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Visitfollowup entity)
    {
        _context.Visitfollowups.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var data = await _context.Visitfollowups.FindAsync(id);
        if (data != null)
        {
            _context.Visitfollowups.Remove(data);
            await _context.SaveChangesAsync();
        }
    }
}