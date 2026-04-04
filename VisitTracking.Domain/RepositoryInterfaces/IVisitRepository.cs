using VisitTracking.Domain.Entities;

namespace VisitTracking.Domain.RepositoryInterfaces
{
    public interface IVisitRepository
    {
        Task<List<Visit>> GetAllAsync();
        Task<Visit> GetByIdAsync(int id);
        Task AddAsync(Visit entity);
        Task UpdateAsync(Visit entity);
        Task DeleteAsync(int id);
    }
}