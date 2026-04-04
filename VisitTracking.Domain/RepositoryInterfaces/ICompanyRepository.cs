namespace VisitTracking.Domain.RepositoryInterfaces
{
    using VisitTracking.Domain.Entities;

    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetAllAsync();
        Task<Company?> GetByIdAsync(int id);
        Task AddAsync(Company entity);
        Task UpdateAsync(Company entity);
        Task DeleteAsync(int id);
    }
}
