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
           
           var data = await _repository.GetAllAsync();
            return data;    


        }

        public async Task<Department> GetByIdAsync(int id)
        {
            var dep = await _repository.GetByIdAsync(id);   
            return dep;

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
            var dep = await _repository.GetByIdAsync(id);
            if (dep == null) return;

            await _repository.DeleteAsync(dep);
        }
    }
}