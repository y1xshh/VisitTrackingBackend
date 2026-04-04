using VisitTracking.Domain.Entities;

namespace VisitTracking.Domain.RepositoryInterfaces
{
    public interface IVehicleTypeRepository
    {
        Task<List<Vehicletype>> GetAllAsync();
        Task<Vehicletype> GetByIdAsync(int id);
        Task AddAsync(Vehicletype entity);
        Task UpdateAsync(Vehicletype entity);
        Task DeleteAsync(int id);
    }
}