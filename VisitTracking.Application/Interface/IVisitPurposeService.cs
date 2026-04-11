using VisitTracking.Application.DTOs;
using VisitTracking.Domain.Entities;

public interface IVisitPurposeService
{
    Task<List<Visitpurpose>> GetAllAsync();
    Task<Visitpurpose> GetByIdAsync(int id);
    Task Create(VisitPurposeDto dto);
    Task UpdateAsync(int id, VisitPurposeDto dto);
    Task DeleteAsync(int id);
}