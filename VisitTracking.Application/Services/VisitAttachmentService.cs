using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;
using VisitTracking.Domain.Entities;

public class VisitAttachmentService : IVisitAttachmentService
{
    private readonly IVisitAttachmentRepository _repo;

    public VisitAttachmentService(IVisitAttachmentRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<VisitAttachmentDto>> GetAllAsync()
    {
        var data = await _repo.GetAllAsync();

        return data.Select(x => new VisitAttachmentDto
        {
            Id = x.Id,
            VisitId = x.VisitId,
            FileName = x.FileName,
            FilePath = x.FilePath,
            FileType = x.FileType,
            IsActive = x.IsActive
        });
    }

    public async Task<VisitAttachmentDto?> GetByIdAsync(int id)
    {
        var x = await _repo.GetByIdAsync(id);
        if (x == null) return null;

        return new VisitAttachmentDto
        {
            Id = x.Id,
            VisitId = x.VisitId,
            FileName = x.FileName,
            FilePath = x.FilePath,
            FileType = x.FileType,
            IsActive = x.IsActive
        };
    }

    public async Task CreateAsync(VisitAttachmentDto dto)
    {
        var entity = new Visitattachment
        {
            VisitId = dto.VisitId,
            FileName = dto.FileName,
            FilePath = dto.FilePath,
            FileType = dto.FileType,
            IsActive = dto.IsActive,
            InsertedDate = DateTime.Now
        };

        await _repo.AddAsync(entity);
    }

    public async Task UpdateAsync(VisitAttachmentDto dto)
    {
        var entity = new Visitattachment
        {
            Id = dto.Id,
            VisitId = dto.VisitId,
            FileName = dto.FileName,
            FilePath = dto.FilePath,
            FileType = dto.FileType,
            IsActive = dto.IsActive,
            UpdatedDate = DateTime.Now
        };

        await _repo.UpdateAsync(entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _repo.DeleteAsync(id);
    }
}