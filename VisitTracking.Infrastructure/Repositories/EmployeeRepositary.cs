using Microsoft.EntityFrameworkCore;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;
using VisitTracking.Infrastructure.Data;

public class EmployeeRepository(AppDbContext context) : IEmployeeRepository
{
    private readonly AppDbContext _context = context;

    public async Task<List<Employee>> GetAllAsync() => await _context.Employees
            .Include(x => x.Designation)
            .Include(x => x.Location)
            .Include(x => x.User)
            .OrderByDescending(x => x.Id)
            .ToListAsync();

    public async Task<Employee?> GetByIdAsync(int id) => await _context.Employees
            .Include(x => x.Designation)
            .Include(x => x.Location)
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task UpdateAsync(Employee employee)
    {
        _context.Employees.Update(employee);
        await _context.SaveChangesAsync();
    }

   
}