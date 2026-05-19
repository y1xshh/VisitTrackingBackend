using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;
using VisitTracking.Application.Validators;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;
using VisitTracking.Infrastructure.Data;
using VisitTracking.Infrastructure.Repositories;

namespace VisitTracking.Application.Services
{
    public class VisitService : IVisitService
    {
        private readonly IVisitRepository _repository;
        private readonly IVehicleTypeRepository _vehicleRepo;
        private readonly IAuditLogService _auditService;
        private readonly IEmailService _emailService;
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<VisitService> _logger;

        public VisitService(
            IVisitRepository repository,
            IVehicleTypeRepository vehicleRepo,
            IAuditLogService auditLogService,
            IEmailService emailService,
            AppDbContext context,
            IHttpContextAccessor httpContextAccessor,
            ILogger<VisitService> logger)
        {
            _repository = repository;
            _vehicleRepo = vehicleRepo;
            _auditService = auditLogService;
            _emailService = emailService;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
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

            const int systemUserId = 0;

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
                Status = ParseVisitStatus(dto.Status),
                CheckInTime = dto.CheckInTime,
                CheckOutTime = dto.CheckOutTime,
                Latitude = latitude,
                Longitude = longitude,
                Remarks = dto.Remarks,
                AttachmentPath = dto.AttachmentPath,
                InsertedBy = systemUserId.ToString(),
                InsertedDate = DateTime.UtcNow,
                UpdatedBy = systemUserId.ToString(),
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
                ActionBy = systemUserId
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
            const int systemUserId = 0;

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
            data.Status = ParseVisitStatus(dto.Status);
            data.CheckInTime = dto.CheckInTime;
            data.CheckOutTime = dto.CheckOutTime;
            data.Latitude = latitude;
            data.Longitude = longitude;
            data.Remarks = dto.Remarks;
            data.AttachmentPath = dto.AttachmentPath;
            data.UpdatedBy = systemUserId.ToString();
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
                ActionBy = systemUserId
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

        public async Task<ApiResponse<VisitApprovalResponseDto>> ApproveVisitAsync(
            int visitId,
            VisitApprovalRequestDto request)
        {
            var validation = new VisitApprovalValidator().Validate(request);
            if (!validation.IsValid)
            {
                var message = string.Join(" | ", validation.Errors.Select(x => x.ErrorMessage));
                return ApiResponse<VisitApprovalResponseDto>.FailResponse(message);
            }

            const int systemUserId = 0;

            try
            {
                var visit = await _context.Visits
                    .FirstOrDefaultAsync(v => v.Id == visitId);

                if (visit == null)
                {
                    return ApiResponse<VisitApprovalResponseDto>.FailResponse("Visit not found.");
                }

                if (visit.Status != VisitStatus.Pending)
                {
                    return ApiResponse<VisitApprovalResponseDto>.FailResponse("Visit already processed.");
                }

                var previousStatus = visit.Status;
                var actionDateUtc = DateTime.UtcNow;
                visit.Status = request.IsApproved ? VisitStatus.Approved : VisitStatus.Rejected;
                visit.UpdatedBy = systemUserId.ToString();
                visit.UpdatedDate = actionDateUtc;

                await _context.SaveChangesAsync();

                var response = new VisitApprovalResponseDto
                {
                    VisitId = visit.Id,
                    Status = visit.Status.ToString(),
                    ActionDateUtc = actionDateUtc,
                    Message = request.IsApproved
                        ? "Visit approved successfully."
                        : "Visit rejected successfully."
                };

                await TryRecordApprovalHistoryAsync(visit, previousStatus, actionDateUtc, request, systemUserId);
                await TryRecordApprovalAuditAsync(visit, previousStatus, actionDateUtc, request, systemUserId);
                await SendApprovalNotificationAsync(visit, request);

                return ApiResponse<VisitApprovalResponseDto>.SuccessResponse(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process visit approval for VisitId {VisitId}", visitId);
                return ApiResponse<VisitApprovalResponseDto>.FailResponse("Unable to process visit approval.");
            }
        }

        private async Task TryRecordApprovalHistoryAsync(
            Visit visit,
            VisitStatus previousStatus,
            DateTime actionDateUtc,
            VisitApprovalRequestDto request,
            int systemUserId)
        {
            try
            {
                await _context.VisitApprovalHistories.AddAsync(new VisitApprovalHistory
                {
                    VisitId = visit.Id,
                    PreviousStatus = previousStatus.ToString(),
                    NewStatus = visit.Status.ToString(),
                    ActionByUserId = systemUserId,
                    ActionDateUtc = actionDateUtc,
                    IpAddress = TryGetClientIpAddress(),
                    Remark = request.Remark,
                    InsertedBy = systemUserId.ToString(),
                    InsertedDate = actionDateUtc,
                    UpdatedBy = systemUserId.ToString(),
                    UpdatedDate = actionDateUtc,
                    IsActive = true
                });

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Approval history save failed for VisitId {VisitId}", visit.Id);
            }
        }

        private async Task TryRecordApprovalAuditAsync(
            Visit visit,
            VisitStatus previousStatus,
            DateTime actionDateUtc,
            VisitApprovalRequestDto request,
            int systemUserId)
        {
            try
            {
                await _auditService.CreateAsync(new AuditLogDto
                {
                    TableName = "VisitApprovalHistory",
                    RecordId = visit.Id,
                    ActionType = request.IsApproved ? "APPROVE" : "REJECT",
                    OldValueJson = JsonConvert.SerializeObject(new
                    {
                        VisitId = visit.Id,
                        PreviousStatus = previousStatus.ToString()
                    }),
                    NewValueJson = JsonConvert.SerializeObject(new
                    {
                        VisitId = visit.Id,
                        NewStatus = visit.Status.ToString(),
                        request.Remark,
                        ActionByUserId = systemUserId,
                        ActionDateUtc = actionDateUtc
                    }),
                    ActionBy = systemUserId
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Approval audit save failed for VisitId {VisitId}", visit.Id);
            }
        }

        private async Task SendApprovalNotificationAsync(Visit visit, VisitApprovalRequestDto request)
        {
            try
            {
                var employee = visit.Employee;
                var user = employee?.User;

                if (user == null || string.IsNullOrWhiteSpace(user.Email))
                {
                    return;
                }

                var recipientEmail = user.Email;

                var subject = request.IsApproved
                    ? $"Visit {visit.Id} approved"
                    : $"Visit {visit.Id} rejected";

                var body = request.IsApproved
                    ? $"Your visit #{visit.Id} has been approved.<br/><b>Remark:</b> {request.Remark}"
                    : $"Your visit #{visit.Id} has been rejected.<br/><b>Remark:</b> {request.Remark}";

                await _emailService.SendEmailAsync(recipientEmail, subject, body);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Approval notification failed for VisitId {VisitId}", visit.Id);
            }
        }

        private string? TryGetClientIpAddress()
        {
            return _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        }

        private static VisitStatus ParseVisitStatus(string? status)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                return VisitStatus.Pending;
            }

            if (Enum.TryParse<VisitStatus>(status, true, out var parsedStatus))
            {
                return parsedStatus;
            }

            throw new Exception("Invalid Status");
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
                Status = entity.Status.ToString(),
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
