using VisitTracking.Application.DTOs;

namespace VisitTracking.Application.Interface
{
    public interface IContactpersonService
    {
        Task<List<ContactpersonResponseDto>> GetAllAsync();
        Task<ContactpersonResponseDto?> GetByIdAsync(int id);
        Task Create(ContactpersonDto dto);
        Task UpdateAsync(int id, ContactpersonDto dto);
        Task DeleteAsync(int id);
        Task<ContactpersonResponseDto?> GetByEmailAsync(string email);
    }
}
