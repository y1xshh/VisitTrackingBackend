using Microsoft.EntityFrameworkCore;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;
using VisitTracking.Infrastructure.Data;

namespace VisitTracking.Infrastructure.Repositories
{
    public class VisitRepository : IVisitRepository
    {
        private readonly AppDbContext _context;

        public VisitRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Visit>> GetAllAsync()
        {
            return await _context.Visits
                .Where(v => v.IsActive != false)
                .Include(v => v.Company)
                .Include(v => v.Employee)
                    .ThenInclude(e => e!.User)
                .Include(v => v.ReportingManager)
                    .ThenInclude(m => m!.User)
                .ToListAsync();
        }

        public async Task<Visit?> GetByIdAsync(int id)
        {
            return await _context.Visits
                .Where(v => v.IsActive != false)
                .Include(v => v.Company)
                .Include(v => v.Employee)
                    .ThenInclude(e => e!.User)
                .Include(v => v.ReportingManager)
                    .ThenInclude(m => m!.User)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task AddAsync(Visit entity)
        {
            await _context.Visits.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Visit entity)
        {
            entity.UpdatedDate = DateTime.UtcNow;

            _context.Visits.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _context.Visits.FirstOrDefaultAsync(v => v.Id == id);
            if (data != null)
            {
                data.IsActive = false;
                data.UpdatedDate = DateTime.UtcNow;
                _context.Visits.Update(data);
                await _context.SaveChangesAsync();
            }
        }
    }
}
