using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;
using VisitTracking.Domain.Entities;

public class VisitFollowUpService : IVisitFollowUpService
{
    private readonly IVisitFollowUpRepository _repo;

    public VisitFollowUpService(IVisitFollowUpRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<VisitFollowUpDto>> GetAllAsync()
    {
        var data = await _repo.GetAllAsync();

        return data.Select(x => new VisitFollowUpDto
        {
            Id = x.Id,
            VisitId = x.VisitId,
            FollowUpDate = x.FollowUpDate,
            FollowUpRemarks = x.FollowUpRemarks,
            FunnelStageId = x.FunnelStageId,
            OutcomeTypeId = x.OutcomeTypeId,
            ExpectedBusinessValue = x.ExpectedBusinessValue,
            ActualBusinessValue = x.ActualBusinessValue,
            NextFollowUpDate = x.NextFollowUpDate,
            IsActive = x.IsActive
        });
    }

    public async Task<VisitFollowUpDto?> GetByIdAsync(int id)
    {
        var x = await _repo.GetByIdAsync(id);
        if (x == null) return null;

        return new VisitFollowUpDto
        {
            Id = x.Id,
            VisitId = x.VisitId,
            FollowUpDate = x.FollowUpDate,
            FollowUpRemarks = x.FollowUpRemarks,
            FunnelStageId = x.FunnelStageId,
            OutcomeTypeId = x.OutcomeTypeId,
            ExpectedBusinessValue = x.ExpectedBusinessValue,
            ActualBusinessValue = x.ActualBusinessValue,
            NextFollowUpDate = x.NextFollowUpDate,
            IsActive = x.IsActive
        };
    }

    public async Task CreateAsync(VisitFollowUpDto dto)
    {
        var entity = new Visitfollowup
        {
            VisitId = dto.VisitId,
            FollowUpDate = dto.FollowUpDate,
            FollowUpRemarks = dto.FollowUpRemarks,
            FunnelStageId = dto.FunnelStageId,
            OutcomeTypeId = dto.OutcomeTypeId,
            ExpectedBusinessValue = dto.ExpectedBusinessValue,
            ActualBusinessValue = dto.ActualBusinessValue,
            NextFollowUpDate = dto.NextFollowUpDate,
            IsActive = dto.IsActive,
            InsertedDate = DateTime.Now
        };
        // 🔥 AUTO NEXT FOLLOW-UP LOGIC HERE
        if (dto.NextFollowUpDate == null && dto.FollowUpDate != null)
        {
            entity.NextFollowUpDate = dto.FollowUpDate.Value.AddDays(2);
        }
        else
        {
            entity.NextFollowUpDate = dto.NextFollowUpDate;
        }

        await _repo.AddAsync(entity);
        // 🔥 WON LOGIC HERE
        if (dto.OutcomeTypeId == 1) // 👈 WON ID (change as per DB)
        {
            entity.ActualBusinessValue = dto.ExpectedBusinessValue;
        }
        else
        {
            entity.ActualBusinessValue = dto.ActualBusinessValue;
        }

        await _repo.AddAsync(entity); 
}
    public async Task UpdateAsync(VisitFollowUpDto dto)
    {
        var entity = new Visitfollowup
        {
            Id = dto.Id,
            VisitId = dto.VisitId,
            FollowUpDate = dto.FollowUpDate,
            FollowUpRemarks = dto.FollowUpRemarks,
            FunnelStageId = dto.FunnelStageId,
            OutcomeTypeId = dto.OutcomeTypeId,
            ExpectedBusinessValue = dto.ExpectedBusinessValue,
            ActualBusinessValue = dto.ActualBusinessValue,
            NextFollowUpDate = dto.NextFollowUpDate,
            IsActive = dto.IsActive,
            UpdatedDate = DateTime.Now
        };

        // 🔥 SAME LOGIC
        if (dto.NextFollowUpDate == null && dto.FollowUpDate != null)
        {
            entity.NextFollowUpDate = dto.FollowUpDate.Value.AddDays(2);
        }
        else
        {
            entity.NextFollowUpDate = dto.NextFollowUpDate;
        }

        await _repo.UpdateAsync(entity);
    // 🔥 SAME LOGIC
    if (dto.OutcomeTypeId == 1) // WON
    {
        entity.ActualBusinessValue = dto.ExpectedBusinessValue;
    }
    else
    {
        entity.ActualBusinessValue = dto.ActualBusinessValue;
    }

    await _repo.UpdateAsync(entity);
}

    public async Task DeleteAsync(int id)
    {
        await _repo.DeleteAsync(id);
    }
}