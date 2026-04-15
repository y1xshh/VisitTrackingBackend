using Microsoft.EntityFrameworkCore;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;
using VisitTracking.Infrastructure.Data;

public class ContactpersonRepository : IContactpersonRepository
{
    private readonly AppDbContext _context;

    public ContactpersonRepository(AppDbContext context)
    {
        _context = context;
    }

    // ✅ FIXED GET ALL
    public async Task<List<Contactperson>> GetAllAsync()
    {
        return await _context.Contactpersons
            .Include(x => x.Company)
            .Include(x => x.Department)
            .Include(x => x.Organisation)
            .ToListAsync();
    }

    // ✅ FIXED GET BY ID
    public async Task<Contactperson> GetByIdAsync(int id)
    {
        return await _context.Contactpersons
            .Include(x => x.Company)
            .Include(x => x.Department)
            .Include(x => x.Organisation)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(Contactperson entity)
    {
        await _context.Contactpersons.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Contactperson entity)
    {
        _context.Contactpersons.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var data = await _context.Contactpersons.FindAsync(id);
        if (data != null)
        {
            _context.Contactpersons.Remove(data);
            await _context.SaveChangesAsync();
        }
    }

    public Task GetByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Contactperson entity)
    {
        throw new NotImplementedException();
    }
}