using VisitTracking.Domain.Entities;

namespace VisitTracking.Domain.RepositoryInterfaces
{
    public interface IVisitPurposeRepository
    {
        Task<List<Visitpurpose>> GetAllAsync();
        Task<Visitpurpose> GetByIdAsync(int id);
        Task AddAsync(Visitpurpose entity);
        Task UpdateAsync(Visitpurpose entity);
        Task DeleteAsync(int id);
    }
}