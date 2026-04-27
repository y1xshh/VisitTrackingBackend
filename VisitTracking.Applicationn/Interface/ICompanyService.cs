using VisitTracking.Application.DTOs;

namespace VisitTracking.Application.Interface
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyResponseDto>> GetAllAsync();
        Task<CompanyResponseDto?> GetByIdAsync(int id);
        Task CreateAsync(CompanyDto dto);
        Task UpdateAsync(CompanyResponseDto dto);
        Task DeleteAsync(int id);
    }
}
