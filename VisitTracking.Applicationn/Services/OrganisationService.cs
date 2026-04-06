using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;

namespace VisitTracking.Application.Services
{
    public class OrganisationService : IOrganisationService
    {
        private readonly IOrganisationRepository _repository;

        public OrganisationService(IOrganisationRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Organisation>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Organisation?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        // ✅ FIXED DTO METHOD
        public async Task<OrganisationDto?> GetDtoByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
                return null;

            return new OrganisationDto
            {
                Id = entity.Id,
                OrganisationName = entity.OrganisationName,

                CompanyId = (int)entity.CompanyId,   // ✅ no casting

                Address = entity.Address,
                City = entity.City,
                State = entity.State,

                // ✅ navigation safe
                CompanyName = entity.Company != null
                                ? entity.Company.CompanyName
                                : null,

                UpdatedBy = entity.UpdatedBy,

                // ✅ null safe
                UpdatedDate = entity.UpdatedDate ?? DateTime.UtcNow
            };
        }

        // ✅ ADD
        public async Task AddAsync(OrganisationDto dto)
        {
            var organisation = new Organisation
            {
                OrganisationName = dto.OrganisationName,
                CompanyId = dto.CompanyId,
                Address = dto.Address,
                City = dto.City,
                State = dto.State,

                InsertedBy = dto.InsertedBy ?? "System",   // ✅ FIX
                InsertedDate = DateTime.UtcNow,

                UpdatedBy = dto.UpdatedBy,
                UpdatedDate = dto.UpdatedDate
            };

            await _repository.AddAsync(organisation);
        }

        // ✅ UPDATE
        public async Task UpdateAsync(int id, OrganisationDto dto)
        {
            var org = await _repository.GetByIdAsync(id);

            if (org == null)
                return;

            org.OrganisationName = dto.OrganisationName;
            org.CompanyId = dto.CompanyId;

            org.Address = dto.Address;
            org.City = dto.City;
            org.State = dto.State;

            org.UpdatedBy = "System";
            org.UpdatedDate = DateTime.UtcNow;

            await _repository.UpdateAsync(org);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}