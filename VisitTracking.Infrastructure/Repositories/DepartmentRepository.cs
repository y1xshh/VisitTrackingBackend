using Microsoft.EntityFrameworkCore;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;
using VisitTracking.Infrastructure.Data;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly AppDbContext _context;

    public DepartmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Department>> GetAllAsync()
    {
        return await _context.Departments
            .Where(x => x.IsActive == true)
            .ToListAsync();
    }

    public async Task<Department> GetByIdAsync(int id)
    {
        var dep = await _context.Departments
            .Include(x => x.Organisation)
            .FirstOrDefaultAsync(x => x.Id == id);
        return dep;
    }
    public async Task AddAsync(Department department)
    {
        var org = await _context.Organisations.FindAsync(department.OrganisationId) ?? throw new Exception("Organisation not found");
    }

    public async Task UpdateAsync(Department department)
    {
      var existingDep = await _context.Departments.FindAsync(department.Id) ?? throw new Exception("Department not found");
        existingDep.DepartmentName = department.DepartmentName;
        existingDep.OrganisationId = department.OrganisationId;
        await _context.SaveChangesAsync();

    }

    public async Task DeleteAsync(int id)
    {
        var dep = await _context.Departments.FindAsync(id);
        if (dep != null)
        {
            _context.Departments.Remove(dep);
            await _context.SaveChangesAsync();
        }
    }

    public Task DeleteAsync(Department dep)
    {
       var existingDep = _context.Departments.Find(dep.Id);
        if (existingDep != null)
        {
            _context.Departments.Remove(existingDep);
            return _context.SaveChangesAsync();
        }
        return Task.CompletedTask;

    }
}