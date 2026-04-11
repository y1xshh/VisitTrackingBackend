using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;

public class FunnelStageService : IFunnelStageService
{
    private readonly IFunnelStageRepository _repo;

    public FunnelStageService(IFunnelStageRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<FunnelStageDto>> GetAllAsync()
    {
        var data = await _repo.GetAllAsync();

        return data.Select(x => new FunnelStageDto
        {
            Id = x.Id,
            StageName = x.StageName,
            StageOrder = x.StageOrder,
            IsClosedStage = x.IsClosedStage,
            IsWonStage = x.IsWonStage,
            IsLostStage = x.IsLostStage,
            IsActive = x.IsActive
        });
    }

    public async Task<FunnelStageDto?> GetByIdAsync(int id)
    {
        var x = await _repo.GetByIdAsync(id);

        if (x == null) return null;

        return new FunnelStageDto
        {
            Id = x.Id,
            StageName = x.StageName,
            StageOrder = x.StageOrder,
            IsClosedStage = x.IsClosedStage,
            IsWonStage = x.IsWonStage,
            IsLostStage = x.IsLostStage,
            IsActive = x.IsActive
        };
    }

    public async Task CreateAsync(FunnelStageDto dto)
    {
        var entity = new Funnelstage
        {
            StageName = dto.StageName,
            StageOrder = dto.StageOrder,
            IsClosedStage = dto.IsClosedStage,
            IsWonStage = dto.IsWonStage,
            IsLostStage = dto.IsLostStage,
            IsActive = dto.IsActive,
            InsertedDate = DateTime.Now
        };

        await _repo.AddAsync(entity);
    }

    public async Task UpdateAsync(FunnelStageDto dto)
    {
        var entity = new Funnelstage
        {
            Id = dto.Id,
            StageName = dto.StageName,
            StageOrder = dto.StageOrder,
            IsClosedStage = dto.IsClosedStage,
            IsWonStage = dto.IsWonStage,
            IsLostStage = dto.IsLostStage,
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