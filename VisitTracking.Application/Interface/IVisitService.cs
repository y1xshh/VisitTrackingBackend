using VisitTracking.Application.DTOs;
using VisitTracking.Domain.Entities;

namespace VisitTracking.Application.Interface
{
   public interface IVisitService
    {
        Task<List<Visit>> GetAllAsync();
        Task<Visit> GetByIdAsync(int id);
        Task Create(VisitDto dto);
        Task UpdateAsync(int id, VisitDto dto);
        Task DeleteAsync(int id);
    }
}
