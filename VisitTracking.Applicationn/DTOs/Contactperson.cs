using Microsoft.Azure.Management.ResourceManager.Fluent.Core.CollectionActions;

namespace VisitTracking.Application.DTOs
{
    public class ContactpersonDto

    {
        //public int Id { get; set; }
        public int CompanyId { get; set; }
        public int OrganisationId { get; set; }
        public int DepartmentId { get; set; }
        public string? Name { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public string? Remark { get; set; }
        public bool IsActive { get; set; }
        public string? CompanyName { get; internal set; }
        public string? DepartmentName { get; internal set; }
        public string? OrganisationName { get; internal set; }
        public string? Designation { get; internal set; }
    }
}