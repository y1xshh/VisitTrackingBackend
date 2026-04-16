using VisitTracking.Domain.Entities;

namespace VisitTracking.Domain.RepositoryInterfaces
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetAllAsync();
        Task<Role?> GetByIdAsync(int id);
        Task AddAsync(Role entity);
        Task UpdateAsync(Role entity);
        Task DeleteAsync(int id);
    }
}