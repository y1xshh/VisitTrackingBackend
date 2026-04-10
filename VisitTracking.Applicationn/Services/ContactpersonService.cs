using VisitTracking.Application.DTOs;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;

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

        return [.. data.Select(x => new ContactpersonDto
        {
            Id = x.Id,
            Name = x.Name,
            Designation = x.Designation,
            Mobile = x.Mobile,
            Email = x.Email,
            IsActive = x.IsActive ?? false,

            CompanyName = x.Company?.CompanyName,
            DepartmentName = x.Department?.DepartmentName,
            OrganisationName = x.Organisation?.OrganisationName
        })];
    }
    // ✅ GET BY ID
    public async Task<ContactpersonDto?> GetByIdAsync(int id)
    {
        var x = await _repository.GetByIdAsync(id);
        if (x == null) return null;

        return new ContactpersonDto
        {
            Id = x.Id,
            Name = x.Name,
            Designation = x.Designation,
            Mobile = x.Mobile,
            Email = x.Email,
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

            Designation = dto.Designation,

            Name = dto.Name,
            Mobile = dto.Mobile,
            Email = dto.Email,
            Remarks = dto.Remark,
            IsActive = dto.IsActive,
            InsertedDate = DateTime.Now
        };

        await _repository.AddAsync(entity);
    }

    // ✅ UPDATE
  public async Task UpdateAsync(int id, ContactpersonDto dto)
{
    var data = await _repository.GetByIdAsync(id);
    if (data == null) return;

    data.CompanyId = dto.CompanyId;
    data.OrganisationId = dto.OrganisationId;
    data.DepartmentId = dto.DepartmentId;

    data.Designation = dto.Designation;
    data.Name = dto.Name;
    data.Mobile = dto.Mobile;
    data.Email = dto.Email;
    data.Remarks = dto.Remark;
    data.IsActive = dto.IsActive;
    data.UpdatedDate = DateTime.Now;

    await _repository.UpdateAsync(data);
}

    // ✅ DELETE
    public async Task DeleteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null) return;

        await _repository.DeleteAsync(entity);
    }

    public Task<ContactpersonDto?> GetByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }
}