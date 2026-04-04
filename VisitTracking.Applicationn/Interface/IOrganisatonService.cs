using VisitTracking.Application.DTOs;
using VisitTracking.Domain.Entities;

namespace VisitTracking.Application.Interface
{
        public interface IOrganisationService
        {
            Task<List<Organisation>> GetAllAsync();
            Task<Organisation> GetByIdAsync(int id);
            Task AddAsync(OrganisationDto dto);
            Task UpdateAsync(int id, OrganisationDto dto);
            Task DeleteAsync(int id);
        }
    }

