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
            .Include(x => x.DesignationName) 
            .Where(x => x.IsActive == true)
            .ToListAsync();
    }

    public async Task<Department> GetByIdAsync(int id)
    {
        return await _context.Departments
            .Include(x => x.Organisation)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(Department department)
    {
        var org = await _context.Organisations.FindAsync(department.OrganisationId)
            ?? throw new Exception("Organisation not found");

        await _context.Departments.AddAsync(department);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Department department)
    {
        var existingDep = await _context.Departments.FindAsync(department.Id)
            ?? throw new Exception("Department not found");

        existingDep.DepartmentName = department.DepartmentName;
        existingDep.OrganisationId = department.OrganisationId;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var dep = await _context.Departments.FindAsync(id);
        if (dep != null)
        {
            dep.IsActive = false; 
            await _context.SaveChangesAsync();
        }
    }
}