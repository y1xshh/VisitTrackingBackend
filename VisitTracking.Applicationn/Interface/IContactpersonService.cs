using VisitTracking.Application.DTOs;

public interface IContactpersonService
{
    Task<List<ContactpersonDto>> GetAllAsync();
    Task<ContactpersonDto?> GetByIdAsync(int id);
    Task<ContactpersonDto?> GetByEmailAsync(string email);
    Task Create(ContactpersonDto dto);
    Task UpdateAsync(int id, ContactpersonDto dto);
    Task DeleteAsync(int id);
}