using VisitTracking.Application.DTOs;

namespace VisitTracking.Application.Interface
{
    public interface IVehicleTypeService
    {
        Task<List<VehicleTypeDto>> GetAllAsync();
        Task<VehicleTypeDto?> GetByIdAsync(int id);
        Task Create(VehicleTypeDto dto);
        Task UpdateAsync(int id, VehicleTypeDto dto);
        Task DeleteAsync(int id);
    }
}