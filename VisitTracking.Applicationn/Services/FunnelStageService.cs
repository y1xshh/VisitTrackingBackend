using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;

public class FunnelStageService : IFunnelStageService
{
    private readonly IFunnelStageRepository _repo;

    private static bool? NormalizeStageFlag(bool? value) => value == true ? true : null;

    public FunnelStageService(IFunnelStageRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<FunnelStageDto>> GetAllAsync()
    {
        var data = await _repo.GetAllAsync();

          return data
      .OrderBy(x => x.StageOrder)
      .Select(static x => new FunnelStageDto
   {
            Id = x.Id,
            StageName = x.StageName,
            Stagecode = $"F-{(x.StageOrder ?? 0).ToString("D2")}",
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
            Stagecode = $"F-{(x.StageOrder ?? 0).ToString("D2")}",
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
        var all = await _repo.GetAllAsync();

        int nextOrder = (all.Max(x => x.StageOrder) ?? 0) + 1;

        var entity = new Funnelstage
        {
            StageName = dto.StageName,
            StageOrder = nextOrder, // auto assign
            IsClosedStage = NormalizeStageFlag(dto.IsClosedStage),
            IsWonStage = NormalizeStageFlag(dto.IsWonStage),
            IsLostStage = NormalizeStageFlag(dto.IsLostStage),
            IsActive = dto.IsActive,
            InsertedDate = DateTime.Now
        };

        await _repo.AddAsync(entity);
    }

    private static int? NewMethod(IEnumerable<Funnelstage> all)
    {
        return all.Max(x => x.StageOrder);
    }

    public async Task UpdateAsync(FunnelStageDto dto)
    {
        var entity = new Funnelstage
        {
            Id = dto.Id,
            StageName = dto.StageName,

            StageOrder = dto.StageOrder,
            IsClosedStage = NormalizeStageFlag(dto.IsClosedStage),
            IsWonStage = NormalizeStageFlag(dto.IsWonStage),
            IsLostStage = NormalizeStageFlag(dto.IsLostStage),
            IsActive = dto.IsActive,
            UpdatedDate = DateTime.Now
        };

        await _repo.UpdateAsync(entity);
    }
    public async Task<IEnumerable<object>> GetDropdownAsync()
    {
        var data = await _repo.GetAllAsync();

        return data
            .OrderBy(x => x.StageOrder)
            .Select(x => new
            {
                value = x.Id,
                code = $"F-{(x.StageOrder ?? 0).ToString("D2")}",
                label = $"F-{(x.StageOrder ?? 0).ToString("D2")} - {x.StageName}"
            });
    }
    public async Task DeleteAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);


    }
}
