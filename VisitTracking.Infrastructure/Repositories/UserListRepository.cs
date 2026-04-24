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
                .AsNoTracking()
                .Include(u => u.Employees)
                .Include(u => u.Role)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return null;

            user.Employees = await _context.Employees
                .AsNoTracking()
                .Where(e => e.UserId == id)
                .ToListAsync();

            if (user.RoleId.HasValue)
            {
                user.Role = await _context.Roles
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.Id == user.RoleId.Value);
            }

            return user;
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
