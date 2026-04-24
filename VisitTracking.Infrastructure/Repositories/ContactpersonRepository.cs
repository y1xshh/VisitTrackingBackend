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


    public async Task<List<Contactperson>> GetAllAsync()
    {
        return await _context.Contactpersons
            .Include(x => x.Company)
            .Include(x => x.Department)
            .Include(x => x.Organisation)
            .ToListAsync();
    }


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
       var entry = await _context.Contactpersons.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Contactperson entity)
    {
        var existingEntity = await _context.Contactpersons.FindAsync(entity.Id);
        if (existingEntity != null)
        {
            existingEntity.CompanyId = entity.CompanyId;
            existingEntity.OrganisationId = entity.OrganisationId;
            existingEntity.DepartmentId = entity.DepartmentId;
            existingEntity.Name = entity.Name;
            existingEntity.Designation = entity.Designation;
            existingEntity.Mobile = entity.Mobile;
            existingEntity.Email = entity.Email;
            existingEntity.Remarks = entity.Remarks;
            existingEntity.IsActive = entity.IsActive;
            _context.Contactpersons.Update(existingEntity);
            await _context.SaveChangesAsync();
        }

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
        var data = _context.Contactpersons.FirstOrDefault(x => x.Email == email);
        return Task.FromResult(data);

    }

    public Task DeleteAsync(Contactperson entity)
    {
        var data = _context.Contactpersons.Find(entity.Id);
        if (data != null)
        {
            _context.Contactpersons.Remove(data);
            return _context.SaveChangesAsync();
        }
        return Task.CompletedTask;

    }
}