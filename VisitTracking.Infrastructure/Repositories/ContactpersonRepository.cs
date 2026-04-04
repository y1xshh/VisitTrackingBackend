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
            .Include(x => x.Designation) // 🔥 important
            .ToListAsync();
    }

    // ✅ FIXED GET BY ID
    public async Task<Contactperson> GetByIdAsync(int id)
    {
        return await _context.Contactpersons
            .Include(x => x.Company)
            .Include(x => x.Department)
            .Include(x => x.Organisation)
            .Include(x => x.Designation) // 🔥 important
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
}