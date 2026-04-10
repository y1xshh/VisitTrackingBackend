using VisitTracking.Domain.Entities;

namespace VisitTracking.Domain.RepositoryInterfaces;

public interface IOrganisationRepository
{
    Task<List<Organisation>> GetAllAsync();
    Task<Organisation?> GetByIdAsync(int id);
    Task AddAsync(Organisation organisation);
    Task UpdateAsync(Organisation organisation);
    Task DeleteAsync(int id);
}