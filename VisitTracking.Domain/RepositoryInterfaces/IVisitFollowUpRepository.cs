using VisitTracking.Domain.Entities;

public interface IVisitFollowUpRepository
{
    Task<IEnumerable<Visitfollowup>> GetAllAsync();
    Task<Visitfollowup?> GetByIdAsync(int id);
    Task AddAsync(Visitfollowup entity);
    Task UpdateAsync(Visitfollowup entity);
    Task DeleteAsync(int id);
}
