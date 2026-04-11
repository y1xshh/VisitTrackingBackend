using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;

namespace VisitTracking.Application.Services
{
    public class VisitService : IVisitService
    {
        private readonly IVisitRepository _repository;
        private readonly IVehicleTypeRepository _vehicleRepo; // <-- Add this line

        public VisitService(IVisitRepository repository, IVehicleTypeRepository vehicleRepo) // <-- Update constructor
        {
            _repository = repository;
            _vehicleRepo = vehicleRepo; // <-- Assign injected dependency
        }

        public async Task<List<Visit>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Visit> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task Create(VisitDto dto)
        {
            decimal? rate = dto.RateAppliedPerKm;

            // 👉 Agar rate nahi diya to VehicleType se uthao
            if (rate == null || rate == 0)
            {
                var vehicle = await _vehicleRepo.GetByIdAsync(dto.VehicleTypeId);
                rate = vehicle?.DefaultRatePerKm;
            }

            // 👉 Expense calculate
            decimal? expense = null;

            if (dto.DistanceKm != null && rate != null)
            {
                expense = dto.DistanceKm * rate;
            }

            var entity = new Visit
            {
                VisitCode = dto.VisitCode,
                VisitDate = dto.VisitDate,

                EmployeeId = dto.EmployeeId,
                CompanyId = dto.CompanyId,
                OrganisationId = dto.OrganisationId,
                DepartmentId = dto.DepartmentId,
                ContactPersonId = dto.ContactPersonId,
                VisitPurposeId = dto.VisitPurposeId,

                DiscussionSummary = dto.DiscussionSummary,
                NextAction = dto.NextAction,
                NextFollowUpDate = dto.NextFollowUpDate,

                VehicleTypeId = dto.VehicleTypeId,
                DistanceKm = dto.DistanceKm,
                RateAppliedPerKm = rate,                  // ✅ auto filled
                TravelExpenseAmount = expense,            // ✅ auto calculated

                FunnelStageId = dto.FunnelStageId,
                OutcomeTypeId = dto.OutcomeTypeId,

                ExpectedBusinessValue = dto.ExpectedBusinessValue,
                ActualBusinessValue = dto.ActualBusinessValue,
                ProbabilityPercent = (int?)dto.ProbabilityPercent,

                Status = dto.Status,
                CheckInTime = dto.CheckInTime,
                CheckOutTime = dto.CheckOutTime,

                Latitude = !string.IsNullOrWhiteSpace(dto.Latitude) ? decimal.Parse(dto.Latitude) : (decimal?)null,
                Longitude = !string.IsNullOrWhiteSpace(dto.Longitude) ? decimal.Parse(dto.Longitude) : (decimal?)null,

                Remarks = dto.Remarks,
                AttachmentPath = dto.AttachmentPath,

                InsertedBy = dto.InsertedBy.ToString(),
                InsertedDate = DateTime.UtcNow,
                UpdatedBy = dto.UpdatedBy?.ToString(), // <-- Fix: convert int? to string
                UpdatedDate = dto.UpdatedDate,
               
            };

            await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(int id, VisitDto dto)
        {
            var data = await _repository.GetByIdAsync(id);
            if (data == null) return;

            data.Status = dto.Status;
            data.UpdatedDate = DateTime.UtcNow; // Set the update timestamp

            await _repository.UpdateAsync(data);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
