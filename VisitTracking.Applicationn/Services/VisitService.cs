using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;
using VisitTracking.Infrastructure.Data;

namespace VisitTracking.Application.Services
{
    public class VisitService : IVisitService
    {
        private readonly IVisitRepository _repository;
        private readonly IVehicleTypeRepository _vehicleRepo;
        private readonly IAuditLogService _auditService;
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VisitService(
            IVisitRepository repository,
            IVehicleTypeRepository vehicleRepo,
            IAuditLogService auditLogService,
            AppDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _vehicleRepo = vehicleRepo;
            _auditService = auditLogService;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<VisitResponseDto>> GetAllAsync()
        {
            var data = await _repository.GetAllAsync();
            return data.Select(MapToDto).ToList();
        }

        public async Task<VisitResponseDto?> GetByIdAsync(int id)
        {
            var data = await _repository.GetByIdAsync(id);
            if (data == null) return null;

            return MapToDto(data);
        }

        public async Task Create(CreateVisitDto dto)
        {
            if (!await _context.Employees.AnyAsync(x => x.Id == dto.EmployeeId))
                throw new Exception("Invalid EmployeeId");

            if (!await _context.Companies.AnyAsync(x => x.Id == dto.CompanyId))
                throw new Exception("Invalid CompanyId");

            if (!await _context.Organisations.AnyAsync(x => x.Id == dto.OrganisationId))
                throw new Exception("Invalid OrganisationId");

            if (!await _context.Departments.AnyAsync(x => x.Id == dto.DepartmentId))
                throw new Exception("Invalid DepartmentId");

            if (!await _context.Contactpersons.AnyAsync(x => x.Id == dto.ContactPersonId))
                throw new Exception("Invalid ContactPersonId");

            if (!await _context.Visitpurposes.AnyAsync(x => x.Id == dto.VisitPurposeId))
                throw new Exception("Invalid VisitPurposeId");

            if (!await _context.Vehicletypes.AnyAsync(x => x.Id == dto.VehicleTypeId))
                throw new Exception("Invalid VehicleTypeId");

            if (!await _context.Funnelstages.AnyAsync(x => x.Id == dto.FunnelStageId))
                throw new Exception("Invalid FunnelStageId");

            if (!await _context.Outcometypes.AnyAsync(x => x.Id == dto.OutcomeTypeId))
                throw new Exception("Invalid OutcomeTypeId");

            var currentUserId = TryGetCurrentUserId();
            var actionBy = currentUserId ?? dto.EmployeeId;

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

            decimal? latitude = decimal.TryParse(dto.Latitude, out var lat) ? lat : null;
            decimal? longitude = decimal.TryParse(dto.Longitude, out var lng) ? lng : null;

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
                ProbabilityPercent = dto.ProbabilityPercent != null
                    ? (int?)dto.ProbabilityPercent
                    : null,
                Status = dto.Status,
                CheckInTime = dto.CheckInTime,
                CheckOutTime = dto.CheckOutTime,
                Latitude = latitude,
                Longitude = longitude,
                Remarks = dto.Remarks,
                AttachmentPath = dto.AttachmentPath,
                InsertedBy = dto.EmployeeId.ToString(),
                InsertedDate = DateTime.UtcNow,
                UpdatedBy = dto.EmployeeId.ToString(),
                UpdatedDate = DateTime.UtcNow
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
                ActionBy = actionBy
            });
        }

        public async Task UpdateAsync(int id, CreateVisitDto dto)
        {
            var data = await _repository.GetByIdAsync(id);
            if (data == null) return;

            var oldValueJson = JsonConvert.SerializeObject(data, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            if (!await _context.Employees.AnyAsync(x => x.Id == dto.EmployeeId))
                throw new Exception("Invalid EmployeeId");

            if (!await _context.Companies.AnyAsync(x => x.Id == dto.CompanyId))
                throw new Exception("Invalid CompanyId");

            if (!await _context.Organisations.AnyAsync(x => x.Id == dto.OrganisationId))
                throw new Exception("Invalid OrganisationId");

            if (!await _context.Departments.AnyAsync(x => x.Id == dto.DepartmentId))
                throw new Exception("Invalid DepartmentId");

            if (!await _context.Contactpersons.AnyAsync(x => x.Id == dto.ContactPersonId))
                throw new Exception("Invalid ContactPersonId");

            if (!await _context.Visitpurposes.AnyAsync(x => x.Id == dto.VisitPurposeId))
                throw new Exception("Invalid VisitPurposeId");

            if (!await _context.Vehicletypes.AnyAsync(x => x.Id == dto.VehicleTypeId))
                throw new Exception("Invalid VehicleTypeId");

            if (!await _context.Funnelstages.AnyAsync(x => x.Id == dto.FunnelStageId))
                throw new Exception("Invalid FunnelStageId");

            if (!await _context.Outcometypes.AnyAsync(x => x.Id == dto.OutcomeTypeId))
                throw new Exception("Invalid OutcomeTypeId");

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

            decimal? latitude = decimal.TryParse(dto.Latitude, out var lat) ? lat : null;
            decimal? longitude = decimal.TryParse(dto.Longitude, out var lng) ? lng : null;
            var currentUserId = TryGetCurrentUserId();
            var actionBy = currentUserId ?? dto.EmployeeId;

            data.VisitCode = dto.VisitCode;
            data.VisitDate = dto.VisitDate;
            data.EmployeeId = dto.EmployeeId;
            data.CompanyId = dto.CompanyId;
            data.OrganisationId = dto.OrganisationId;
            data.DepartmentId = dto.DepartmentId;
            data.ContactPersonId = dto.ContactPersonId;
            data.VisitPurposeId = dto.VisitPurposeId;
            data.DiscussionSummary = dto.DiscussionSummary;
            data.NextAction = dto.NextAction;
            data.NextFollowUpDate = dto.NextFollowUpDate;
            data.VehicleTypeId = dto.VehicleTypeId;
            data.DistanceKm = dto.DistanceKm;
            data.RateAppliedPerKm = rate;
            data.TravelExpenseAmount = expense;
            data.FunnelStageId = dto.FunnelStageId;
            data.OutcomeTypeId = dto.OutcomeTypeId;
            data.ExpectedBusinessValue = dto.ExpectedBusinessValue;
            data.ActualBusinessValue = dto.ActualBusinessValue;
            data.ProbabilityPercent = dto.ProbabilityPercent != null
                ? (int?)dto.ProbabilityPercent
                : null;
            data.Status = dto.Status;
            data.CheckInTime = dto.CheckInTime;
            data.CheckOutTime = dto.CheckOutTime;
            data.Latitude = latitude;
            data.Longitude = longitude;
            data.Remarks = dto.Remarks;
            data.AttachmentPath = dto.AttachmentPath;
            data.UpdatedBy = dto.EmployeeId.ToString();
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
                ActionBy = actionBy
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
                RecordId = data.Id,
                ActionType = "DELETE",
                OldValueJson = oldValueJson,
                NewValueJson = null,
                ActionBy = 1
            });
        }

        private int? TryGetCurrentUserId()
        {
            var userIdValue = _httpContextAccessor.HttpContext?.User.FindFirst("id")?.Value;

            if (int.TryParse(userIdValue, out var userId))
            {
                return userId;
            }

            return null;
        }

        private static VisitResponseDto MapToDto(Visit entity)
        {
            int insertedBy = 0;
            int.TryParse(entity.InsertedBy, out insertedBy);

            int? updatedBy = null;
            if (int.TryParse(entity.UpdatedBy, out var parsedUpdatedBy))
            {
                updatedBy = parsedUpdatedBy;
            }

            return new VisitResponseDto
            {
                Id = entity.Id,
                VisitCode = entity.VisitCode,
                VisitDate = entity.VisitDate.GetValueOrDefault(),
                EmployeeId = entity.EmployeeId.GetValueOrDefault(),
                CompanyId = entity.CompanyId.GetValueOrDefault(),
                OrganisationId = entity.OrganisationId.GetValueOrDefault(),
                DepartmentId = entity.DepartmentId.GetValueOrDefault(),
                ContactPersonId = entity.ContactPersonId.GetValueOrDefault(),
                VisitPurposeId = entity.VisitPurposeId.GetValueOrDefault(),
                DiscussionSummary = entity.DiscussionSummary,
                NextAction = entity.NextAction,
                NextFollowUpDate = entity.NextFollowUpDate,
                VehicleTypeId = entity.VehicleTypeId.GetValueOrDefault(),
                DistanceKm = entity.DistanceKm,
                RateAppliedPerKm = entity.RateAppliedPerKm,
                TravelExpenseAmount = entity.TravelExpenseAmount,
                FunnelStageId = entity.FunnelStageId.GetValueOrDefault(),
                OutcomeTypeId = entity.OutcomeTypeId.GetValueOrDefault(),
                ExpectedBusinessValue = entity.ExpectedBusinessValue,
                ActualBusinessValue = entity.ActualBusinessValue,
                ProbabilityPercent = entity.ProbabilityPercent,
                Status = entity.Status,
                CheckInTime = entity.CheckInTime,
                CheckOutTime = entity.CheckOutTime,
                Latitude = entity.Latitude?.ToString(),
                Longitude = entity.Longitude?.ToString(),
                Remarks = entity.Remarks,
                AttachmentPath = entity.AttachmentPath,
                InsertedBy = insertedBy,
                InsertedDate = entity.InsertedDate.GetValueOrDefault(),
                UpdatedBy = updatedBy,
                UpdatedDate = entity.UpdatedDate
            };
        }
    }
}
