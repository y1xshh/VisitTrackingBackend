using Newtonsoft.Json;
using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;
using VisitTracking.Infrastructure.Data;

namespace VisitTracking.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _repository;
        private readonly IAuditLogService _auditService;

        public DepartmentService(IDepartmentRepository repository, IAuditLogService auditLogService)
        {
            _repository = repository;
            _auditService = auditLogService;
        }

        public async Task<List<DepartmentDto>> GetAllAsync()
        {
            var data = await _repository.GetAllAsync();

            return data.Select(x => new DepartmentDto
            {
                Id = x.Id,
                DepartmentName = x.DepartmentName ?? string.Empty,
                OrganisationId = (int)(x.OrganisationId ?? 0),
                Designations = x.DesignationName.Select(d => d.DesignationName ?? string.Empty).ToList()
            }).ToList();
        }

        public async Task<DepartmentDto?> GetByIdAsync(int id)
        {
            var dep = await _repository.GetByIdAsync(id);
            if (dep == null) return null;

            return new DepartmentDto
            {
                Id = dep.Id,
                DepartmentName = dep.DepartmentName ?? string.Empty,
                OrganisationId = (int)(dep.OrganisationId ?? 0),
                Designations = dep.DesignationName.Select(d => d.DesignationName ?? string.Empty).ToList()
            };
        }

        public async Task Create(DepartmentDto dto)
        {
            var dep = new Department
            {
                DepartmentName = dto.DepartmentName,
                OrganisationId = dto.OrganisationId,
            };

            await _repository.AddAsync(dep);

            await _auditService.CreateAsync(new AuditLogDto
            {
                TableName = "Department",
                RecordId = dep.Id,
                ActionType = "INSERT",
                OldValueJson = string.Empty,
                NewValueJson = JsonConvert.SerializeObject(dep, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }) ?? string.Empty,
                ActionBy = 1
            });
        }

        public async Task UpdateAsync(int id, DepartmentDto dto)
        {
            var dep = await _repository.GetByIdAsync(id);
            if (dep == null) return;

            var oldValueJson = JsonConvert.SerializeObject(dep, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }) ?? string.Empty;

            dep.DepartmentName = dto.DepartmentName;
            dep.OrganisationId = dto.OrganisationId;

            await _repository.UpdateAsync(dep);

            await _auditService.CreateAsync(new AuditLogDto
            {
                TableName = "Department",
                RecordId = dep.Id,
                ActionType = "UPDATE",
                OldValueJson = oldValueJson,
                NewValueJson = JsonConvert.SerializeObject(dep, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }) ?? string.Empty,
                ActionBy = 1
            });
        }

        public async Task DeleteAsync(int id)
        {
            var dep = await _repository.GetByIdAsync(id);
            if (dep == null) return;

            await _repository.DeleteAsync(dep.Id);

            await _auditService.CreateAsync(new AuditLogDto
            {
                TableName = "Department",
                RecordId = dep.Id,
                ActionType = "DELETE",
                OldValueJson = JsonConvert.SerializeObject(dep, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }) ?? string.Empty,
                NewValueJson = string.Empty,
                ActionBy = 1
            });
        }
    }
}