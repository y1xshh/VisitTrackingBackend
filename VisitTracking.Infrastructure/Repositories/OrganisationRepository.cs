using Microsoft.EntityFrameworkCore;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;
using VisitTracking.Infrastructure.Data;

public class OrganisationRepository : IOrganisationRepository
{
    private readonly AppDbContext _context;

    public OrganisationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Organisation>> GetAllAsync()
    {
        return await _context.Organisations
            .Include(x => x.Company)          // ✅ MUST
            //.Include(x => x.ContactPeople)    // ✅
            .Include(x => x.Departments)      // ✅
            .ToListAsync();
    }

    public async Task<Organisation?> GetByIdAsync(int id)
    {
        return await _context.Organisations
            .Include(x => x.Company)          // ✅ MUST
            //.Include(x => x.ContactPeople)
            .Include(x => x.Departments)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(Organisation organisation)
    {
        await _context.Organisations.AddAsync(organisation);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Organisation organisation)
    {
        _context.Organisations.Update(organisation);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var org = await _context.Organisations.FindAsync(id);
        if (org != null)
        {
            _context.Organisations.Remove(org);
            await _context.SaveChangesAsync();
        }
    }
}