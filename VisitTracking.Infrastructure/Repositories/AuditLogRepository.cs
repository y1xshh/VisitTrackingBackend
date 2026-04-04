using Microsoft.EntityFrameworkCore;
using VisitTracking.Domain.Entities;
using VisitTracking.Infrastructure.Data;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly AppDbContext _context;

    public AuditLogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Auditlog>> GetAllAsync()
    {
        return await _context.Auditlogs
            .OrderByDescending(x => x.InsertedDate)
            .ToListAsync();
    }

    public async Task<Auditlog?> GetByIdAsync(int id)
    {
        return await _context.Auditlogs.FindAsync(id);
    }

    public async Task AddAsync(Auditlog entity)
    {
        await _context.Auditlogs.AddAsync(entity);
        await _context.SaveChangesAsync();
    }
}