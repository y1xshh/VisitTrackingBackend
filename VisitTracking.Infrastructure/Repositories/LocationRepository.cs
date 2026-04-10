using Microsoft.EntityFrameworkCore;
using VisitTracking.Application.Interfaces;
using VisitTracking.Domain.Entities;
using VisitTracking.Infrastructure.Data;

namespace VisitTracking.Infrastructure.Repositories;

public class LocationRepository : ILocationRepository
{
    private readonly AppDbContext _context;

    public LocationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<MstLocation>> GetAllAsync()
    {
        return await _context.MstLocations.ToListAsync();
    }

    public async Task<MstLocation?> GetByIdAsync(int id)
    {
        return await _context.MstLocations.FindAsync(id);
    }

    public async Task AddAsync(MstLocation entity)
    {
        await _context.MstLocations.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(MstLocation entity)
    {
        _context.MstLocations.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(MstLocation entity)
    {
        _context.MstLocations.Remove(entity);
        await _context.SaveChangesAsync();
    }
}