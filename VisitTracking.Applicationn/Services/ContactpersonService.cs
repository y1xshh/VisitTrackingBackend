using VisitTracking.Application.DTOs;
using VisitTracking.Application.DTOs.VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;

namespace VisitTracking.Application.Services
{
    public class ContactpersonService : IContactpersonService
    {
        private readonly IContactpersonRepository _repository;

        public ContactpersonService(IContactpersonRepository repository)
        {
            _repository = repository;
        }

        // ✅ GET ALL
        public async Task<List<ContactpersonDto>> GetAllAsync()
        {
            var data = await _repository.GetAllAsync();

            return data.Select(x => new ContactpersonDto
            {
                Id = x.Id,
                CompanyId = x.CompanyId,
                OrganisationId = x.OrganisationId,
                DepartmentId = x.DepartmentId,

                Name = x.Name,
                Designation = x.Designation,
                Mobile = x.Mobile,
                Email = x.Email,
                Remark = x.Remarks,
                IsActive = x.IsActive ?? false,

                CompanyName = x.Company?.CompanyName,
                DepartmentName = x.Department?.DepartmentName,
                OrganisationName = x.Organisation?.OrganisationName
            }).ToList();
        }

        // ✅ GET BY ID
        public async Task<ContactpersonDto?> GetByIdAsync(int id)
        {
            var x = await _repository.GetByIdAsync(id);

            if (x == null)
                return null;

            return new ContactpersonDto
            {
                Id = x.Id,
                CompanyId = x.CompanyId,
                OrganisationId = x.OrganisationId,
                DepartmentId = x.DepartmentId,

                Name = x.Name,
                Designation = x.Designation,
                Mobile = x.Mobile,
                Email = x.Email,
                Remark = x.Remarks,
                IsActive = x.IsActive ?? false,

                CompanyName = x.Company?.CompanyName,
                DepartmentName = x.Department?.DepartmentName,
                OrganisationName = x.Organisation?.OrganisationName
            };
        }

        // ✅ CREATE
        public async Task Create(ContactpersonDto dto)
        {
            var entity = new Contactperson
            {
                CompanyId = dto.CompanyId,
                OrganisationId = dto.OrganisationId,
                DepartmentId = dto.DepartmentId,

                Name = dto.Name,
                Designation = dto.Designation,
                Mobile = dto.Mobile,
                Email = dto.Email,
                Remarks = dto.Remark,
                IsActive = dto.IsActive,

                InsertedBy = "system", // 🔥 later replace with logged-in user
                InsertedDate = DateTime.UtcNow
            };

            await _repository.AddAsync(entity);
        }

        // ✅ UPDATE
        public async Task UpdateAsync(int id, ContactpersonDto dto)
        {
            var data = await _repository.GetByIdAsync(id);

            if (data == null)
                throw new Exception("Contact person not found");

            data.CompanyId = dto.CompanyId;
            data.OrganisationId = dto.OrganisationId;
            data.DepartmentId = dto.DepartmentId;

            data.Name = dto.Name;
            data.Designation = dto.Designation;
            data.Mobile = dto.Mobile;
            data.Email = dto.Email;
            data.Remarks = dto.Remark;
            data.IsActive = dto.IsActive;

            data.UpdatedBy = "system"; // 🔥 later replace with logged-in user
            data.UpdatedDate = DateTime.UtcNow;

            await _repository.UpdateAsync(data);
        }

        // ✅ DELETE (Soft Delete Recommended)
        public async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
                throw new Exception("Contact person not found");

            // 🔥 Soft delete
            entity.IsActive = false;
            entity.UpdatedDate = DateTime.UtcNow;

            await _repository.UpdateAsync(entity);
        }

        // ✅ GET BY EMAIL
        public async Task<ContactpersonDto?> GetByEmailAsync(string email)
        {
            var data = await _repository.GetAllAsync();

            var x = data.FirstOrDefault(x => x.Email == email);

            if (x == null)
                return null;

            return new ContactpersonDto
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                Mobile = x.Mobile,
                Designation = x.Designation
            };
        }
    }
}