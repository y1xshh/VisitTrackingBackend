using VisitTracking.Domain.Entities;

namespace VisitTracking.Domain.RepositoryInterfaces
{
    public interface IFunnelStageRepository
    {
        Task<IEnumerable<Funnelstage>> GetAllAsync();
        Task<Funnelstage?> GetByIdAsync(int id);
        Task AddAsync(Funnelstage entity);
        Task UpdateAsync(Funnelstage entity);
        Task DeleteAsync(int id);
    }
}