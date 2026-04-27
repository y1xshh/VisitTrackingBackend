namespace VisitTracking.Application.DTOs
{
    public class OutcomeTypeResponseDto
    {
        public int Id { get; set; }
        public string? OutComeName { get; set; }
        public bool? IsRevenueLinked { get; set; }
        public bool? IsActive { get; set; }
    }
}