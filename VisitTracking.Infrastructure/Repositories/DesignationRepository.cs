using Microsoft.EntityFrameworkCore;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;
using VisitTracking.Infrastructure.Data;

namespace VisitTracking.Infrastructure.Repositories;

public class DesignationRepository(AppDbContext context) : IDesignationRepository
{
    private readonly AppDbContext _context = context;

    public async Task<List<MstDesignation>> GetAllAsync()
    {
        return await _context.MstDesignations.ToListAsync();
    }

    public async Task<MstDesignation?> GetByIdAsync(int id)
    {
        return await _context.MstDesignations.FindAsync(id);
    }

    public async Task AddAsync(MstDesignation entity)
    {
        await _context.MstDesignations.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(MstDesignation entity)
    {
        _context.MstDesignations.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(MstDesignation entity)
    {
        _context.MstDesignations.Remove(entity);
        await _context.SaveChangesAsync();
    }
}