using Newtonsoft.Json;
using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;
using VisitTracking.Domain.RepositoryInterfaces;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repository;
    private readonly IAuditLogService _auditService;

    public EmployeeService(IEmployeeRepository repository, IAuditLogService auditLogService)
    {
        _repository = repository;
        _auditService = auditLogService;
    }

    public async Task<List<EmployeeDto>> GetAllAsync()
    {
        var data = await _repository.GetAllAsync();

        return data.Select(x => new EmployeeDto
        {
            Id = x.Id,
            EmployeeCode = x.EmployeeCode,
            UserId = x.UserId,
            DesignationId = x.DesignationId,
            ReportingManagerId = x.ReportingManagerId,
            LocationId = x.LocationId,
            IsActive = x.IsActive,

            DesignationName = x.Designation != null ? x.Designation.DesignationName : null,
            LocationName = x.Location != null ? x.Location.LocationName : null

        }).ToList();
    }

    public async Task<EmployeeDto?> GetByIdAsync(int id)
    {
        var x = await _repository.GetByIdAsync(id);

        if (x == null) return null;

        return new EmployeeDto
        {
            Id = x.Id,
            EmployeeCode = x.EmployeeCode,
            UserId = x.UserId,
            DesignationId = x.DesignationId,
            ReportingManagerId = x.ReportingManagerId,
            LocationId = x.LocationId,
            IsActive = x.IsActive,

            DesignationName = x.Designation?.DesignationName,
            LocationName = x.Location?.LocationName
        };
    }

    public async Task UpdateAsync(int id, EmployeeDto dto)
    {
        var emp = await _repository.GetByIdAsync(id);

        if (emp == null) return;

        var oldValueJson = JsonConvert.SerializeObject(emp, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });

        emp.EmployeeCode = dto.EmployeeCode;
        emp.UserId = dto.UserId;
        emp.DesignationId = dto.DesignationId;
        emp.ReportingManagerId = dto.ReportingManagerId;
        emp.LocationId = dto.LocationId;

        emp.UpdatedBy = "System";
        emp.UpdatedDate = DateTime.UtcNow;

        await _repository.UpdateAsync(emp);

        await _auditService.CreateAsync(new AuditLogDto
        {
            TableName = "Employees",
            RecordId = emp.Id,
            ActionType = "UPDATE",
            OldValueJson = oldValueJson,
            NewValueJson = JsonConvert.SerializeObject(emp, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }),
            ActionBy = 1
        });
    }


    public async Task<List<EmployeeDropdownDto>> GetReportingManagerDropdown()
    {
        var data = await _repository.GetAllAsync();
        var employeeList = data.Select(x => new EmployeeDropdownDto
        {
            Id = x.Id,
            DisplayName = x.User != null ? x.User.FullName : $"Employee {x.Id}"
        }).ToList();
        return employeeList;
    }
}