using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;
using VisitTracking.Infrastructure.Data;

namespace VisitTracking.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _repository;

        public DepartmentService(IDepartmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Department>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Department> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task Create(DepartmentDto dto)
        {
            var dep = new Department
            {
                DepartmentName = dto.DepartmentName,
                OrganisationId = dto.OrganisationId,
               
            };

            await _repository.AddAsync(dep);
        }

        public async Task UpdateAsync(int id, DepartmentDto dto)
        {
            var dep = await _repository.GetByIdAsync(id);
            if (dep == null) return;

            dep.DepartmentName = dto.DepartmentName;
            dep.OrganisationId = dto.OrganisationId;
            

            await _repository.UpdateAsync(dep);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}