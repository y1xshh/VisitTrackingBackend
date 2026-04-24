using VisitTracking.Domain.Entities;

namespace VisitTracking.Domain.RepositoryInterfaces
{
    public interface IExpenserateRepository
    {
        Task<IEnumerable<Expenserate>> GetAllAsync();
        Task<Expenserate?> GetByIdAsync(int id);
        Task AddAsync(Expenserate entity);
        Task UpdateAsync(Expenserate entity);
        Task DeleteAsync(int id);
    }
}
