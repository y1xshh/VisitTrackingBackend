using VisitTracking.Domain.Entities;

public interface IAuditLogRepository
{
    Task<IEnumerable<Auditlog>> GetAllAsync();
    Task<Auditlog?> GetByIdAsync(int id);
    Task AddAsync(Auditlog entity);
}