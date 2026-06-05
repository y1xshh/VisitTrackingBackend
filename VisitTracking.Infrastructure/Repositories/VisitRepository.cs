using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
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

        public async Task<List<dynamic>> GetAllAsync(int flag, int employeeId)
        {
            var result = new List<dynamic>();

            using var conn = _context.Database.GetDbConnection();

            await conn.OpenAsync();

            using var cmd = conn.CreateCommand();

            cmd.CommandText = "CALL visittrackingdb.sp_GetVisits(@flag, @employeeId)";

            var p1 = cmd.CreateParameter();
            p1.ParameterName = "@flag";
            p1.Value = flag;

            var p2 = cmd.CreateParameter();
            p2.ParameterName = "@employeeId";
            p2.Value = employeeId;

            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                IDictionary<string, object> row = new ExpandoObject();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = reader.GetValue(i);
                }

                result.Add(row);
            }

            return result;
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
