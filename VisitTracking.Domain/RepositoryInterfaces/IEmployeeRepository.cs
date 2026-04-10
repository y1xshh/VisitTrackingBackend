using VisitTracking.Domain.Entities;

namespace VisitTracking.Domain.RepositoryInterfaces
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task UpdateAsync(Employee employee);
    }
}