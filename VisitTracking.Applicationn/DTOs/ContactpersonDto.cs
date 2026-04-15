namespace VisitTracking.Application.DTOs
{
    public class ContactpersonDto
    {
        public int Id { get; set; }

        public int? CompanyId { get; set; }
        public int? OrganisationId { get; set; }
        public int? DepartmentId { get; set; }

        public string? Name { get; set; }
        public string? Designation { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }

        public string? Remark { get; set; }
        public bool IsActive { get; set; }

        public string? CompanyName { get; set; }
        public string? DepartmentName { get; set; }
        public string? OrganisationName { get; set; }
    }
}
