using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _repo;

    public CompanyService(ICompanyRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<CompanyDto>> GetAllAsync()
    {
        var data = await _repo.GetAllAsync();

        return data.Select(x => new CompanyDto
        {
            Id = x.Id,
            CompanyName = x.CompanyName,
            CompanyType = x.CompanyType,
            IndustryType = x.IndustryType,
            Address = x.Address,
            City = x.City,
            State = x.State,
            Pincode = x.Pincode,
            IsActive = x.IsActive
        });
    }

    public async Task<CompanyDto?> GetByIdAsync(int id)
    {
        var x = await _repo.GetByIdAsync(id);
        if (x == null) return null;

        return new CompanyDto
        {
            Id = x.Id,
            CompanyName = x.CompanyName,
            CompanyType = x.CompanyType,
            IndustryType = x.IndustryType,
            Address = x.Address,
            City = x.City,
            State = x.State,
            Pincode = x.Pincode,
            IsActive = x.IsActive
        };
    }

    public async Task CreateAsync(CompanyDto dto)
    {
        // 🔥 BASIC VALIDATION
        if (string.IsNullOrEmpty(dto.CompanyName))
            throw new Exception("Company Name is required");

        var entity = new Company
        {
            CompanyName = dto.CompanyName,
            CompanyType = dto.CompanyType,
            IndustryType = dto.IndustryType,
            Address = dto.Address,
            City = dto.City,
            State = dto.State,
            Pincode = dto.Pincode,
            IsActive = true,
            InsertedDate = DateTime.Now
        };

        await _repo.AddAsync(entity);
    }

    public async Task UpdateAsync(CompanyDto dto)
    {
        var entity = new Company
        {
            Id = dto.Id,
            CompanyName = dto.CompanyName,
            CompanyType = dto.CompanyType,
            IndustryType = dto.IndustryType,
            Address = dto.Address,
            City = dto.City,
            State = dto.State,
            Pincode = dto.Pincode,
            IsActive = dto.IsActive,
            UpdatedDate = DateTime.Now
        };

        await _repo.UpdateAsync(entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _repo.DeleteAsync(id);
    }
}