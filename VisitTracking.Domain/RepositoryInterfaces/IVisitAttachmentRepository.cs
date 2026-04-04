using VisitTracking.Domain.Entities;

public interface IVisitAttachmentRepository
{
    Task<IEnumerable<Visitattachment>> GetAllAsync();
    Task<Visitattachment?> GetByIdAsync(int id);
    Task AddAsync(Visitattachment entity);
    Task UpdateAsync(Visitattachment entity);
    Task DeleteAsync(int id);
}
