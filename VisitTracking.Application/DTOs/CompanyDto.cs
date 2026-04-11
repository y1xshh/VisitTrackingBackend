namespace VisitTracking.Application.DTOs
{
    public class CompanyDto
    {
        public int Id { get; set; }

        public string? CompanyName { get; set; }
        public string? CompanyType { get; set; }
        public string? IndustryType { get; set; }

        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Pincode { get; set; }

        public bool? IsActive { get; set; }

    }
}
