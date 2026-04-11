using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;

namespace VisitTracking.Application.Services
{
    public class ExpenserateService : IExpenserateService
    {
        private readonly IExpenserateRepository _repo;

        public ExpenserateService(IExpenserateRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ExpenserateDto>> GetAllAsync()
        {
            var data = await _repo.GetAllAsync();
            return data.Select(x => new ExpenserateDto
            {
                Id = x.Id,
                VehicleTypeId = x.VehicleTypeId ?? 0,
                RatePerKm = x.RatePerKm ?? 0,
                EffectiveFrom = x.EffectiveFrom.HasValue ? x.EffectiveFrom.Value.ToDateTime(TimeOnly.MinValue) : DateTime.MinValue,
                EffectiveTo = x.EffectiveTo.HasValue ? x.EffectiveTo.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                IsActive = x.IsActive ?? false
            });
        }

        public async Task<ExpenserateDto> GetByIdAsync(int id)
        {
            var x = await _repo.GetByIdAsync(id);
            if (x == null) return null;

            return new ExpenserateDto
            {
                Id = x.Id,
                VehicleTypeId = x.VehicleTypeId ?? 0,
                RatePerKm = x.RatePerKm ?? 0,
                EffectiveFrom = x.EffectiveFrom.HasValue ? x.EffectiveFrom.Value.ToDateTime(TimeOnly.MinValue) : DateTime.MinValue,
                EffectiveTo = x.EffectiveTo.HasValue ? x.EffectiveTo.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                IsActive = x.IsActive ?? false
            };
        }

        public async Task CreateAsync(ExpenserateDto dto)
        {
            var entity = new Expenserate
            {
                VehicleTypeId = dto.VehicleTypeId,
                RatePerKm = dto.RatePerKm,
                EffectiveFrom = DateOnly.FromDateTime(dto.EffectiveFrom),
                EffectiveTo = dto.EffectiveTo.HasValue ? DateOnly.FromDateTime(dto.EffectiveTo.Value) : null,
                IsActive = dto.IsActive
            };

            await _repo.AddAsync(entity);
        }

        public async Task UpdateAsync(ExpenserateDto dto)
        {
            var entity = new Expenserate
            {
                Id = dto.Id,
                VehicleTypeId = dto.VehicleTypeId,
                RatePerKm = dto.RatePerKm,
                EffectiveFrom = DateOnly.FromDateTime(dto.EffectiveFrom),
                EffectiveTo = dto.EffectiveTo.HasValue ? DateOnly.FromDateTime(dto.EffectiveTo.Value) : null,
                IsActive = dto.IsActive
            };

            await _repo.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
