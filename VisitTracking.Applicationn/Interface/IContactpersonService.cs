using VisitTracking.Application.DTOs;

namespace VisitTracking.Application.Interface
{
    public interface IContactpersonService
    {
        Task<List<ContactpersonDto>> GetAllAsync();
        Task<ContactpersonDto?> GetByIdAsync(int id);
        Task Create(ContactpersonDto dto);
        Task UpdateAsync(int id, ContactpersonDto dto);
        Task DeleteAsync(int id);
        Task<ContactpersonDto?> GetByEmailAsync(string email);
    }
}
