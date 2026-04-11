using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interfaces;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;

namespace VisitTracking.Application.Services;

public class DesignationService : IDesignationService
{
    private readonly IDesignationRepository _repo;

    public DesignationService(IDesignationRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<DesignationDto>> GetAllAsync()
    {
        var data = await _repo.GetAllAsync();

        return data.Select(x => new DesignationDto
        {
            Id = x.Id,
            DesignationName = x.DesignationName,
            DepartmentId = x.DepartmentId,
            IsActive = x.IsActive
        }).ToList();
    }

    public async Task<DesignationDto?> GetByIdAsync(int id)
    {
        var x = await _repo.GetByIdAsync(id);
        if (x == null) return null;

        return new DesignationDto
        {
            Id = x.Id,
            DesignationName = x.DesignationName,
            DepartmentId = x.DepartmentId,
            IsActive = x.IsActive
        };
    }

    public async Task<string> CreateAsync(DesignationDto dto)
    {
        var entity = new MstDesignation
        {
            DesignationName = dto.DesignationName,
            DepartmentId = dto.DepartmentId,
            IsActive = true,
            InsertedDate = DateTime.UtcNow
        };

        await _repo.AddAsync(entity);
        return "Created Successfully";
    }

    public async Task<string> UpdateAsync(int id, DesignationDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return "Not Found";

        entity.DesignationName = dto.DesignationName;
        entity.DepartmentId = dto.DepartmentId;
        entity.IsActive = dto.IsActive;
        entity.UpdatedDate = DateTime.UtcNow;

        await _repo.UpdateAsync(entity);
        return "Updated Successfully";
    }

    public async Task<string> DeleteAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return "Not Found";

        await _repo.DeleteAsync(entity);
        return "Deleted Successfully";
    }
}