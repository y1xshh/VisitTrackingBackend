namespace VisitTracking.Application.DTOs
{
    public class CreateVisitDto
    {
        public DateTime VisitDate { get; set; }
        public int EmployeeId { get; set; }
        public int CompanyId { get; set; }
        public long VehcileTypeId { get; set; }
        public double DistanceKm { get; set; }
        public decimal RateAppliedPerKm { get; set; }
    }
}