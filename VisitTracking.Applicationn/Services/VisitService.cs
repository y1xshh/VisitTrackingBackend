using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Security.Claims;
using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;
using VisitTracking.Application.Validators;
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
        private readonly IEmailService _emailService;
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<VisitService> _logger;

        public VisitService(
            IVisitRepository repository,
            IVehicleTypeRepository vehicleRepo,
            IAuditLogService auditLogService,
            IEmailService emailService,
            AppDbContext context,
            IHttpContextAccessor httpContextAccessor,
            ICurrentUserService currentUserService,
            ILogger<VisitService> logger)
        {
            _repository = repository;
            _vehicleRepo = vehicleRepo;
            _auditService = auditLogService;
            _emailService = emailService;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<IEnumerable<dynamic>> GetAllAsync(int flag)
        {
            var currentEmployeeId = _currentUserService.EmployeeId;

            return await _repository.GetAllAsync(flag, currentEmployeeId);
        }

        public async Task<VisitResponseDto?> GetByIdAsync(int id)
        {
            var data = await _repository.GetByIdAsync(id);
            if (data == null)
            {
                return null;
            }

            var currentEmployeeId = _currentUserService.EmployeeId;
            var designation = _currentUserService.Designation;
            var role = GetCurrentRole();

            if (!CanViewVisit(data, currentEmployeeId, designation, role))
            {
                throw new Exception("Unauthorized access to visit.");
            }

            return MapToDto(data);
        }

        public async Task Create(CreateVisitDto dto)
        {
            var loggedInEmployee = await GetLoggedInEmployeeAsync();
            var loggedInUserId = _currentUserService.UserId;

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

            var entity = new Visit
            {
                VisitCode = dto.VisitCode,
                VisitDate = dto.VisitDate,
                EmployeeId = loggedInEmployee.Id,
                ReportingManagerId = loggedInEmployee.ReportingManagerId,
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
                ProbabilityPercent = dto.ProbabilityPercent != null ? (int?)dto.ProbabilityPercent : null,
                Status = VisitStatus.Pending,
                CheckInTime = dto.CheckInTime,
                CheckOutTime = dto.CheckOutTime,
                Latitude = latitude,
                Longitude = longitude,
                Remarks = dto.Remarks,
                AttachmentPath = dto.AttachmentPath,
                IsActive = true,
                InsertedBy = loggedInUserId.ToString(),
                InsertedDate = DateTime.UtcNow,
                UpdatedBy = loggedInUserId.ToString(),
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
                ActionBy = loggedInUserId
            });
        }

        public async Task UpdateAsync(int id, CreateVisitDto dto)
        {
            var data = await _repository.GetByIdAsync(id);
            if (data == null)
                return;

            var currentEmployeeId = _currentUserService.EmployeeId;
            var currentDesignation = _currentUserService.Designation;
            var currentRole = GetCurrentRole();
            var currentUserId = _currentUserService.UserId;

            if (!CanEditVisit(data, currentEmployeeId, currentDesignation, currentRole))
                throw new Exception("Unauthorized access to update this visit.");

            var oldValueJson = JsonConvert.SerializeObject(data, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

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

            var loggedInEmployee = await GetLoggedInEmployeeAsync();

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

            data.VisitCode = dto.VisitCode;
            data.VisitDate = dto.VisitDate;
            data.EmployeeId = loggedInEmployee.Id;
            data.ReportingManagerId = loggedInEmployee.ReportingManagerId;
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
            data.ProbabilityPercent = dto.ProbabilityPercent != null ? (int?)dto.ProbabilityPercent : null;
            data.CheckInTime = dto.CheckInTime;
            data.CheckOutTime = dto.CheckOutTime;
            data.Latitude = latitude;
            data.Longitude = longitude;
            data.Remarks = dto.Remarks;
            data.AttachmentPath = dto.AttachmentPath;
            data.UpdatedBy = currentUserId.ToString();
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
                ActionBy = currentUserId
            });
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _repository.GetByIdAsync(id);
            if (data == null)
                return;

            var currentEmployeeId = _currentUserService.EmployeeId;
            var designation = _currentUserService.Designation;
            var role = GetCurrentRole();
            var currentUserId = _currentUserService.UserId;

            if (!CanEditVisit(data, currentEmployeeId, designation, role))
                throw new Exception("Unauthorized access to delete this visit.");

            var oldValueJson = JsonConvert.SerializeObject(data, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            data.IsActive = false;
            data.UpdatedBy = currentUserId.ToString();
            data.UpdatedDate = DateTime.UtcNow;

            await _repository.UpdateAsync(data);

            await _auditService.CreateAsync(new AuditLogDto
            {
                TableName = "Visit",
                RecordId = data.Id,
                ActionType = "DELETE",
                OldValueJson = oldValueJson,
                NewValueJson = JsonConvert.SerializeObject(new
                {
                    data.Id,
                    data.IsActive,
                    data.UpdatedBy,
                    data.UpdatedDate
                }),
                ActionBy = currentUserId
            });
        }

        public async Task<ApiResponse<VisitApprovalResponseDto>> ApproveVisitAsync(
            int visitId,
            VisitApprovalRequestDto request)
        {
            if (request == null)
            {
                return BuildApprovalFailureResponse(visitId, "Invalid request.");
            }

            var validation = new VisitApprovalValidator().Validate(request);
            if (!validation.IsValid)
            {
                var message = string.Join(" | ", validation.Errors.Select(x => x.ErrorMessage));
                return BuildApprovalFailureResponse(visitId, message);
            }

            var currentUserId = _currentUserService.UserId;
            if (currentUserId <= 0)
            {
                return BuildApprovalFailureResponse(visitId, "Invalid user context.");
            }

            var role = NormalizeRole(GetCurrentRole());
            var canForward = IsForwardingRole(role);
            var canFinalApprove = IsFinalApproverRole(role);

            if (!canForward && !canFinalApprove)
            {
                return BuildApprovalFailureResponse(visitId, "Unauthorized approval role.");
            }

            var visit = await _context.Visits
                .Include(v => v.Employee)
                    .ThenInclude(e => e!.User!)
                .FirstOrDefaultAsync(v => v.Id == visitId);

            if (visit == null)
            {
                return BuildApprovalFailureResponse(visitId, "Visit not found");
            }

            if (IsProcessedStatus(visit.Status))
            {
                return BuildApprovalFailureResponse(
                    visitId,
                    $"Visit already processed. Current status: {visit.Status}",
                    visit);
            }

            try
            {
                var actionDateUtc = await ProcessApprovalAsync(visit, request, currentUserId, canForward, canFinalApprove);

                return ApiResponse<VisitApprovalResponseDto>.SuccessResponse(new VisitApprovalResponseDto
                {
                    VisitId = visit.Id,
                    Status = visit.Status.ToString(),
                    ActionDateUtc = actionDateUtc,
                    Message = visit.Status == VisitStatus.Forwarded
                        ? $"Visit {visit.Id} forwarded to {request.ForwardTo}."
                        : $"Visit {visit.Id} approved successfully."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process visit approval for VisitId {VisitId}", visitId);
                return BuildApprovalFailureResponse(visitId, ex.InnerException?.Message ?? ex.Message, visit);
            }
        }

        private static ApiResponse<VisitApprovalResponseDto> BuildApprovalFailureResponse(
            int visitId,
            string message,
            Visit? visit = null)
        {
            return new ApiResponse<VisitApprovalResponseDto>
            {
                Success = false,
                Message = message,
                Data = new VisitApprovalResponseDto
                {
                    VisitId = visit?.Id ?? visitId,
                    Status = visit?.Status.ToString() ?? string.Empty,
                    ActionDateUtc = visit?.UpdatedDate ?? DateTime.UtcNow
                }
            };
        }

        private static bool IsProcessedStatus(VisitStatus status)
        {
            return status is VisitStatus.Approved or VisitStatus.Rejected or VisitStatus.Cancelled or VisitStatus.Completed;
        }

        private async Task<DateTime> ProcessApprovalAsync(
            Visit visit,
            VisitApprovalRequestDto request,
            int currentUserId,
            bool canForward,
            bool canFinalApprove)
        {
            ValidateApproval(visit);

            var actionDateUtc = DateTime.UtcNow;
            var previousStatus = visit.Status;
            var newStatus = UpdateVisitStatus(visit, request, canForward, canFinalApprove, currentUserId, actionDateUtc);

            await AddApprovalHistory(visit, previousStatus, newStatus, request, currentUserId, actionDateUtc, canForward);
            await AddAuditLogAsync(visit, previousStatus, newStatus, currentUserId, actionDateUtc);
            await AddExpenseApprovalAsync(visit, request, newStatus, currentUserId, actionDateUtc);

            await _context.SaveChangesAsync();

            if (newStatus == VisitStatus.Approved)
            {
                await SendApprovalMail(visit, request.Remark);
            }

            return actionDateUtc;
        }

        private static void ValidateApproval(Visit visit)
        {
            if (visit.Status is VisitStatus.Approved or VisitStatus.Rejected or VisitStatus.Cancelled or VisitStatus.Completed)
            {
                throw new Exception($"Visit already processed. Current status: {visit.Status}.");
            }
        }

        private static VisitStatus UpdateVisitStatus(
            Visit visit,
            VisitApprovalRequestDto request,
            bool canForward,
            bool canFinalApprove,
            int currentUserId,
            DateTime actionDateUtc)
        {
            if (canForward && !canFinalApprove)
            {
                if (string.IsNullOrWhiteSpace(request.ForwardTo))
                {
                    throw new Exception("ForwardTo is required for forwarding.");
                }

                visit.Status = VisitStatus.Forwarded;
            }
            else
            {
                visit.Status = VisitStatus.Approved;
            }

            visit.UpdatedBy = currentUserId.ToString();
            visit.UpdatedDate = actionDateUtc;
            return visit.Status;
        }

        private async Task AddApprovalHistory(
            Visit visit,
            VisitStatus previousStatus,
            VisitStatus newStatus,
            VisitApprovalRequestDto request,
            int currentUserId,
            DateTime actionDateUtc,
            bool canForward)
        {
            var remark = canForward && !string.IsNullOrWhiteSpace(request.ForwardTo)
                ? $"Forwarded to {request.ForwardTo}. {request.Remark}".Trim()
                : request.Remark;

            var history = new VisitApprovalHistory
            {
                VisitId = visit.Id,
                PreviousStatus = previousStatus.ToString(),
                NewStatus = newStatus.ToString(),
                ActionByUserId = currentUserId,
                ActionDateUtc = actionDateUtc,
                IpAddress = TryGetClientIpAddress(),
                Remark = remark,
                IsActive = true,
                InsertedBy = currentUserId.ToString(),
                InsertedDate = actionDateUtc,
                UpdatedBy = currentUserId.ToString(),
                UpdatedDate = actionDateUtc
            };

            await _context.VisitApprovalHistories.AddAsync(history);
        }

        private async Task AddAuditLogAsync(
            Visit visit,
            VisitStatus previousStatus,
            VisitStatus newStatus,
            int currentUserId,
            DateTime actionDateUtc)
        {
            var oldSnapshot = new
            {
                visit.Id,
                visit.VisitCode,
                Status = previousStatus.ToString(),
                visit.EmployeeId,
                visit.ReportingManagerId,
                visit.CompanyId,
                visit.OrganisationId,
                visit.DepartmentId,
                visit.ContactPersonId,
                visit.VisitPurposeId,
                visit.DiscussionSummary,
                visit.NextAction,
                visit.NextFollowUpDate,
                visit.VehicleTypeId,
                visit.DistanceKm,
                visit.RateAppliedPerKm,
                visit.TravelExpenseAmount,
                visit.FunnelStageId,
                visit.OutcomeTypeId,
                visit.ExpectedBusinessValue,
                visit.ActualBusinessValue,
                visit.ProbabilityPercent,
                visit.CheckInTime,
                visit.CheckOutTime,
                visit.Latitude,
                visit.Longitude,
                visit.Remarks,
                visit.AttachmentPath,
                visit.IsActive,
                visit.InsertedBy,
                visit.InsertedDate,
                visit.UpdatedBy,
                visit.UpdatedDate
            };

            var auditLog = new Auditlog
            {
                TableName = "visits",
                RecordId = visit.Id,
                ActionType = "UPDATE",
                OldValueJson = JsonConvert.SerializeObject(oldSnapshot, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }),
                NewValueJson = JsonConvert.SerializeObject(new
                {
                    visit.Id,
                    visit.VisitCode,
                    Status = newStatus.ToString(),
                    visit.EmployeeId,
                    visit.ReportingManagerId,
                    visit.CompanyId,
                    visit.OrganisationId,
                    visit.DepartmentId,
                    visit.ContactPersonId,
                    visit.VisitPurposeId,
                    visit.DiscussionSummary,
                    visit.NextAction,
                    visit.NextFollowUpDate,
                    visit.VehicleTypeId,
                    visit.DistanceKm,
                    visit.RateAppliedPerKm,
                    visit.TravelExpenseAmount,
                    visit.FunnelStageId,
                    visit.OutcomeTypeId,
                    visit.ExpectedBusinessValue,
                    visit.ActualBusinessValue,
                    visit.ProbabilityPercent,
                    visit.CheckInTime,
                    visit.CheckOutTime,
                    visit.Latitude,
                    visit.Longitude,
                    visit.Remarks,
                    visit.AttachmentPath,
                    visit.IsActive,
                    visit.InsertedBy,
                    visit.InsertedDate,
                    visit.UpdatedBy,
                    visit.UpdatedDate
                }, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }),
                ActionBy = currentUserId,
                IsActive = true,
                InsertedBy = currentUserId.ToString(),
                InsertedDate = actionDateUtc,
                UpdatedBy = currentUserId.ToString(),
                UpdatedDate = actionDateUtc
            };

            await _context.Auditlogs.AddAsync(auditLog);
        }

        private async Task AddExpenseApprovalAsync(
            Visit visit,
            VisitApprovalRequestDto request,
            VisitStatus newStatus,
            int currentUserId,
            DateTime actionDateUtc)
        {
            if (newStatus != VisitStatus.Approved)
            {
                return;
            }

            var expenseApproval = new ExpenseApproval
            {
                VisitId = visit.Id,
                SubmittedBy = visit.EmployeeId,
                ApprovedBy = currentUserId,
                ApprovalStatus = VisitStatus.Approved.ToString(),
                ApprovalRemarks = request.Remark,
                SubmittedAt = visit.InsertedDate,
                ApprovedAt = actionDateUtc,
                IsActive = true,
                InsertedBy = currentUserId.ToString(),
                InsertedDate = actionDateUtc,
                UpdatedBy = currentUserId.ToString(),
                UpdatedDate = actionDateUtc
            };

            await _context.ExpenseApprovals.AddAsync(expenseApproval);
        }

        private async Task SendApprovalMail(Visit visit, string? remark)
        {
            var recipientEmail = visit.Employee?.User?.Email;

            if (string.IsNullOrWhiteSpace(recipientEmail))
            {
                throw new InvalidOperationException($"Approval notification failed because recipient email is missing for VisitId {visit.Id}.");
            }

            var subject = $"Visit {visit.Id} Approved";
            var body = string.IsNullOrWhiteSpace(remark)
                ? $"Your visit #{visit.Id} has been approved."
                : $"Your visit #{visit.Id} has been approved.<br/><b>Remark:</b> {remark}";

            await _emailService.SendEmailAsync(recipientEmail, subject, body);
        }

        private async Task<Employee> GetLoggedInEmployeeAsync()
        {
            var loggedInEmployeeId = _currentUserService.EmployeeId;
            if (loggedInEmployeeId <= 0)
            {
                throw new Exception("Invalid employee context");
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(x => x.Id == loggedInEmployeeId);

            if (employee == null)
            {
                throw new Exception("Employee not found");
            }

            return employee;
        }

        private string? GetCurrentRole()
        {
            return _currentUserService.Principal?.FindFirstValue(ClaimTypes.Role)
                ?? _currentUserService.Principal?.FindFirstValue("role");
        }

        private static string NormalizeRole(string? role)
        {
            var value = role ?? string.Empty;
            return new string(value.Where(char.IsLetterOrDigit).ToArray()).ToLowerInvariant();
        }

        private static int ResolveUserId(string? userId)
        {
            return int.TryParse(userId, out var parsedUserId) ? parsedUserId : 0;
        }

        private static bool IsAdminDesignation(string? designation)
        {
            return !string.IsNullOrWhiteSpace(designation) &&
                   designation.Contains("admin", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsManagerDesignation(string? designation)
        {
            return !string.IsNullOrWhiteSpace(designation) &&
                   designation.Contains("manager", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsFinalApproverRole(string? role)
        {
            var normalized = NormalizeRole(role);
            return normalized == "admin" || normalized == "superadmin";
        }

        private static bool IsForwardingRole(string? role)
        {
            var normalized = NormalizeRole(role);
            return normalized == "employee" || normalized == "teamlead" || normalized == "manager";
        }

        private static bool CanViewVisit(Visit visit, int currentEmployeeId, string? designation, string? role)
        {
            if (IsFinalApproverRole(role) || IsAdminDesignation(designation))
                return true;

            if (IsManagerDesignation(designation) || NormalizeRole(role) == "manager")
                return visit.ReportingManagerId == currentEmployeeId;

            return visit.EmployeeId == currentEmployeeId;
        }

        private static bool CanEditVisit(Visit visit, int currentEmployeeId, string? designation, string? role)
        {
            if (IsFinalApproverRole(role) || IsAdminDesignation(designation))
                return true;

            if (IsManagerDesignation(designation) || NormalizeRole(role) == "manager")
                return visit.ReportingManagerId == currentEmployeeId || visit.EmployeeId == currentEmployeeId;

            return visit.EmployeeId == currentEmployeeId;
        }

        private static bool CanApproveVisit(Visit visit, int currentEmployeeId, string? designation, string? role)
        {
            if (IsFinalApproverRole(role) || IsAdminDesignation(designation))
                return true;

            return visit.ReportingManagerId == currentEmployeeId;
        }

        private static IEnumerable<Visit> FilterByAccess(
            IEnumerable<Visit> visits,
            int currentEmployeeId,
            string? designation,
            string? role)
        {
            if (IsFinalApproverRole(role) || IsAdminDesignation(designation))
                return visits;

            if (IsManagerDesignation(designation) || NormalizeRole(role) == "manager")
                return visits.Where(v => v.ReportingManagerId == currentEmployeeId);

            return visits.Where(v => v.EmployeeId == currentEmployeeId);
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
                EmployeeId = entity.EmployeeId,
                ReportingManagerId = entity.ReportingManagerId,
                CompanyId = entity.CompanyId,
                OrganisationId = entity.OrganisationId,
                DepartmentId = entity.DepartmentId,
                ContactPersonId = entity.ContactPersonId,
                VisitPurposeId = entity.VisitPurposeId,
                DiscussionSummary = entity.DiscussionSummary,
                NextAction = entity.NextAction,
                NextFollowUpDate = entity.NextFollowUpDate,
                VehicleTypeId = entity.VehicleTypeId,
                DistanceKm = entity.DistanceKm,
                RateAppliedPerKm = entity.RateAppliedPerKm,
                TravelExpenseAmount = entity.TravelExpenseAmount,
                FunnelStageId = entity.FunnelStageId,
                OutcomeTypeId = entity.OutcomeTypeId,
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
