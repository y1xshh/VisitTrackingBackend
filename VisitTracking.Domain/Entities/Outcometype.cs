namespace VisitTracking.Domain.Entities;

public partial class Outcometype
{
    public int Id { get; set; }

    public string? OutcomeName { get; set; }

    public bool? IsRevenueLinked { get; set; }

    public bool? IsActive { get; set; }

    public string? InsertedBy { get; set; }

    public DateTime? InsertedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
