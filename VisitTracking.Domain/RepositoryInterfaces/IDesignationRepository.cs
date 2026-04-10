using VisitTracking.Domain.Entities;

namespace VisitTracking.Domain.RepositoryInterfaces;

public interface IDesignationRepository
{
    Task<List<MstDesignation>> GetAllAsync();
    Task<MstDesignation?> GetByIdAsync(int id);
    Task AddAsync(MstDesignation entity);
    Task UpdateAsync(MstDesignation entity);
    Task DeleteAsync(MstDesignation entity);
}