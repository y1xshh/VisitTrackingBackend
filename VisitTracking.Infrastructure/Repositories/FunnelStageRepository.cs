using Microsoft.EntityFrameworkCore;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;
using VisitTracking.Infrastructure.Data;

namespace VisitTracking.Infrastructure.Repositories
{
    public class FunnelStageRepository : IFunnelStageRepository
    {
        private readonly AppDbContext _context;

        public FunnelStageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Funnelstage>> GetAllAsync()
        {
            return await _context.Funnelstages
                .OrderBy(x => x.StageOrder)
                .ToListAsync();
        }

        public async Task<Funnelstage?> GetByIdAsync(int id)
        {
            return await _context.Funnelstages.FindAsync(id);
        }

        public async Task AddAsync(Funnelstage entity)
        {
            await _context.Funnelstages.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Funnelstage entity)
        {
            _context.Funnelstages.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _context.Funnelstages.FindAsync(id);
            if (data != null)
            {
                _context.Funnelstages.Remove(data);
                await _context.SaveChangesAsync();
            }
        }
    }
}