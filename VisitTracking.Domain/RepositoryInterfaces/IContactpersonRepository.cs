using VisitTracking.Domain.Entities;

namespace VisitTracking.Domain.RepositoryInterfaces
{
    public interface IContactpersonRepository
    {
        Task<List<Contactperson>> GetAllAsync();
        Task<Contactperson?> GetByIdAsync(int id);
        Task AddAsync(Contactperson entity);
        Task UpdateAsync(Contactperson entity);
        Task DeleteAsync(int id);
        Task<Contactperson?> GetByEmailAsync(string email);
        Task DeleteAsync(Contactperson entity);
    }
}