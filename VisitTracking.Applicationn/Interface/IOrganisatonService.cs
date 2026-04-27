using VisitTracking.Application.DTOs;

namespace VisitTracking.Application.Interface;

public interface IOrganisationService
{
    Task<List<OrganisationResponseDto>> GetAllAsync();
    Task<OrganisationResponseDto?> GetByIdAsync(int id);
    Task AddAsync(OrganisationDto dto);
    Task UpdateAsync(int id, OrganisationDto dto);
    Task DeleteAsync(int id);
}
