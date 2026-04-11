using VisitTracking.Application.DTOs;

namespace VisitTracking.Application.Interface
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyDto>> GetAllAsync();
        Task<CompanyDto?> GetByIdAsync(int id);
        Task CreateAsync(CompanyDto dto);
        Task UpdateAsync(CompanyDto dto);
        Task DeleteAsync(int id);
    }
}
