using Microsoft.EntityFrameworkCore;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;
using VisitTracking.Infrastructure.Data;

public class VisitpurposeRepository : IVisitPurposeRepository
{
    private readonly AppDbContext _context;

    public VisitpurposeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Visitpurpose>> GetAllAsync()
    {
        return await _context.Visitpurposes.ToListAsync();
    }

    public async Task<Visitpurpose> GetByIdAsync(int id)
    {
        return await _context.Visitpurposes.FindAsync(id);
    }

    public async Task AddAsync(Visitpurpose entity)
    {
        await _context.Visitpurposes.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Visitpurpose entity)
    {
        _context.Visitpurposes.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var data = await _context.Visitpurposes.FindAsync(id);
        if (data != null)
        {
            _context.Visitpurposes.Remove(data);
            await _context.SaveChangesAsync();
        }
    }
}