using Microsoft.EntityFrameworkCore;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;
using VisitTracking.Infrastructure.Data;

namespace VisitTracking.Infrastructure.Repositories
{
    public class UserListRepository : IUserListRepository
    {
        private readonly AppDbContext _context;

        public UserListRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllAsync()
        { 

            return await _context.Users
                .Include(u => u.Employees)
                .Include(u => u.Role)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
            .Include(u => u.Employees)
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _context.Users.FindAsync(id);
            if (data != null)
            {
                _context.Users.Remove(data);
                await _context.SaveChangesAsync();
            }
        }
    }
}