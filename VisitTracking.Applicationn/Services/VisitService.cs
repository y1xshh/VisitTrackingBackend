using Newtonsoft.Json;
using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;

namespace VisitTracking.Application.Services
{
    public class VisitService : IVisitService
    {
        private readonly IVisitRepository _repository;
        private readonly IVehicleTypeRepository _vehicleRepo;
        private readonly IAuditLogService _auditService;

        public VisitService(IVisitRepository repository, IVehicleTypeRepository vehicleRepo, IAuditLogService auditLogService)
        {
            _repository = repository;
            _vehicleRepo = vehicleRepo;
            _auditService = auditLogService;
        }

        public async Task<List<Visit>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Visit?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task Create(VisitDto dto)
        {
            decimal? rate = dto.RateAppliedPerKm;

            if (rate == null || rate == 0)
            {
                var vehicle = await _vehicleRepo.GetByIdAsync(dto.VehicleTypeId);
                rate = vehicle?.DefaultRatePerKm;
            }

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
                RateAppliedPerKm = rate,
                TravelExpenseAmount = expense,

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
                UpdatedBy = dto.UpdatedBy?.ToString(),
                UpdatedDate = dto.UpdatedDate,
            };

            await _repository.AddAsync(entity);

            await _auditService.CreateAsync(new AuditLogDto
            {
                TableName = "Visit",
                RecordId = entity.Id,
                ActionType = "INSERT",
                OldValueJson = null,
                NewValueJson = JsonConvert.SerializeObject(entity, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }),
                ActionBy = 1
            });
        }

        public async Task UpdateAsync(int id, VisitDto dto)
        {
            var data = await _repository.GetByIdAsync(id);
            if (data == null) return;

            var oldValueJson = JsonConvert.SerializeObject(data, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            data.Status = dto.Status;
            data.UpdatedDate = DateTime.UtcNow;

            await _repository.UpdateAsync(data);

            await _auditService.CreateAsync(new AuditLogDto
            {
                TableName = "Visit",
                RecordId = data.Id,
                ActionType = "UPDATE",
                OldValueJson = oldValueJson,
                NewValueJson = JsonConvert.SerializeObject(data, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }),
                ActionBy = 1
            });
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _repository.GetByIdAsync(id);
            if (data == null) return;

            var oldValueJson = JsonConvert.SerializeObject(data, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            await _repository.DeleteAsync(id);

            await _auditService.CreateAsync(new AuditLogDto
            {
                TableName = "Visit",
                RecordId = id,
                ActionType = "DELETE",
                OldValueJson = oldValueJson,
                NewValueJson = null,
                ActionBy = 1
            });
        }
    }
}
