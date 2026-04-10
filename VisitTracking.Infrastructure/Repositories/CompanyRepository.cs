using Microsoft.EntityFrameworkCore;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;
using VisitTracking.Infrastructure.Data;

namespace VisitTracking.Infrastructure.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly AppDbContext _context;

    public CompanyRepository(AppDbContext context)
    {
        _context = context;
    }

    // ✅ GET ALL
    public async Task<List<Company>> GetAllAsync()
    {
        return await _context.Companies
            .OrderByDescending(x => x.Id)
            .ToListAsync();
    }

    // ✅ GET BY ID
    public async Task<Company?> GetByIdAsync(int id)
    {
        var entity = await _context.Companies.FindAsync(id);
        return entity;
    }

    // ✅ CREATE
    public async Task AddAsync(Company entity)
    {
        entity.InsertedDate = DateTime.UtcNow;
        entity.IsActive = true;

        await _context.Companies.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    // ✅ UPDATE
    public async Task UpdateAsync(Company entity)
    {
        entity.UpdatedDate = DateTime.UtcNow;

        _context.Companies.Update(entity);
        await _context.SaveChangesAsync();
    }

    // ✅ DELETE
    public async Task DeleteAsync(int id)
    {
        var data = await _context.Companies.FindAsync(id);
        if (data == null) return;

        _context.Companies.Remove(data);
        await _context.SaveChangesAsync();
    }

  
}