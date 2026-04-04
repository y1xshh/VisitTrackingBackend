using VisitTracking.Application.DTOs;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;

public class VisitPurposeService : IVisitPurposeService
{
    private readonly IVisitPurposeRepository _repository;

    public VisitPurposeService(IVisitPurposeRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Visitpurpose>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Visitpurpose> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task Create(VisitPurposeDto dto)
    {
        var entity = new Visitpurpose
        {
            PurposeName = dto.PurposeName,
            IsActive = dto.IsActive
        };

        await _repository.AddAsync(entity);
    }

    public async Task UpdateAsync(int id, VisitPurposeDto dto)
    {
        var data = await _repository.GetByIdAsync(id);
        if (data == null) return;

        data.PurposeName = dto.PurposeName;
        data.IsActive = dto.IsActive;

        await _repository.UpdateAsync(data);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}