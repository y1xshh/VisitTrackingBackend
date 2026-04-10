using VisitTracking.Domain.Entities;

namespace VisitTracking.Application.Interfaces;

public interface ILocationRepository
{
    Task<List<MstLocation>> GetAllAsync();
    Task<MstLocation?> GetByIdAsync(int id);
    Task AddAsync(MstLocation entity);
    Task UpdateAsync(MstLocation entity);
    Task DeleteAsync(MstLocation entity);
}