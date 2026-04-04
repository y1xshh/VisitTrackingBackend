using Microsoft.EntityFrameworkCore;
using VisitTracking.Domain.Entities;
using VisitTracking.Infrastructure.Data;

public class VisitAttachmentRepository : IVisitAttachmentRepository
{
    private readonly AppDbContext _context;

    public VisitAttachmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Visitattachment>> GetAllAsync()
    {
        return await _context.Visitattachments
            .Include(x => x.Visit)
            .ToListAsync();
    }

    public async Task<Visitattachment?> GetByIdAsync(int id)
    {
        return await _context.Visitattachments
            .Include(x => x.Visit)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(Visitattachment entity)
    {
        await _context.Visitattachments.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Visitattachment entity)
    {
        _context.Visitattachments.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var data = await _context.Visitattachments.FindAsync(id);
        if (data != null)
        {
            _context.Visitattachments.Remove(data);
            await _context.SaveChangesAsync();
        }
    }
}