using Microsoft.EntityFrameworkCore;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;
using VisitTracking.Infrastructure.Data;

namespace VisitTracking.Infrastructure.Repositories
{
    public class ExpenserateRepository : IExpenserateRepository
    {
        private readonly AppDbContext _context;

        public ExpenserateRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Expenserate>> GetAllAsync()
        {
            return await _context.Expenserates.ToListAsync();
        }

        public async Task<Expenserate> GetByIdAsync(int id)
        {
            return await _context.Expenserates.FindAsync(id);
        }

        public async Task AddAsync(Expenserate entity)
        {
            await _context.Expenserates.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Expenserate entity)
        {
            _context.Expenserates.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _context.Expenserates.FindAsync(id);
            if (data != null)
            {
                _context.Expenserates.Remove(data);
                await _context.SaveChangesAsync();
            }
        }
    }
}
