using VisitTracking.Domain.Entities;

public interface IOutcomeTypeRepository
{
    Task<IEnumerable<Outcometype>> GetAllAsync();
    Task<Outcometype?> GetByIdAsync(int id);
    Task AddAsync(Outcometype entity);
    Task UpdateAsync(Outcometype entity);
    Task DeleteAsync(int id);
}