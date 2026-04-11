using VisitTracking.Application.DTOs;
using VisitTracking.Domain.Entities;

public class OutcomeTypeService : IOutcomeTypeService
{
    private readonly IOutcomeTypeRepository _repo;

    public OutcomeTypeService(IOutcomeTypeRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<OutcomeTypeDto>> GetAllAsync()
    {
        var data = await _repo.GetAllAsync();

        return data.Select(x => new OutcomeTypeDto
        {
            Id = x.Id,
            OutComeName = x.OutcomeName,
            IsRevenueLinked = x.IsRevenueLinked,
            IsActive = x.IsActive
        });
    }

    public async Task<OutcomeTypeDto?> GetByIdAsync(int id)
    {
        var x = await _repo.GetByIdAsync(id);
        if (x == null) return null;

        return new OutcomeTypeDto
        {
            Id = x.Id,
            OutComeName = x.OutcomeName,
            IsRevenueLinked = x.IsRevenueLinked,
            IsActive = x.IsActive
        };
    }

    public async Task CreateAsync(OutcomeTypeDto dto)
    {
        var entity = new Outcometype
        {
            OutcomeName = dto.OutComeName,
            IsRevenueLinked = dto.IsRevenueLinked,
            IsActive = dto.IsActive,
            InsertedDate = DateTime.Now
        };

        await _repo.AddAsync(entity);
    }

    public async Task UpdateAsync(OutcomeTypeDto dto)
    {
        var entity = new Outcometype
        {
            Id = (int)dto.Id,
            OutcomeName = dto.OutComeName,
            IsRevenueLinked = dto.IsRevenueLinked,
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