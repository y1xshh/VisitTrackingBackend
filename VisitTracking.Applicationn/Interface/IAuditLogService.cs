using VisitTracking.Application.DTOs;

namespace VisitTracking.Application.Interface
{
    public interface IAuditLogService
    {
        Task<IEnumerable<AuditLogDto>> GetAllAsync();
        Task CreateAsync(AuditLogDto dto);
    }
}
