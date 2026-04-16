using Microsoft.EntityFrameworkCore;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;
using VisitTracking.Infrastructure.Data;

public class VehicleTypeRepository : IVehicleTypeRepository
{
    private readonly AppDbContext _context;

    public VehicleTypeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Vehicletype>> GetAllAsync()
    {
        return await _context.Vehicletypes.ToListAsync();
    }

    public async Task<Vehicletype?> GetByIdAsync(int id)
    {
        return await _context.Vehicletypes.FindAsync(id);
    }

    public async Task AddAsync(Vehicletype entity)
    {
        await _context.Vehicletypes.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Vehicletype entity)
    {
        _context.Vehicletypes.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var data = await _context.Vehicletypes.FindAsync(id);
        if (data != null)
        {
            _context.Vehicletypes.Remove(data);
            await _context.SaveChangesAsync();
        }
    }
}