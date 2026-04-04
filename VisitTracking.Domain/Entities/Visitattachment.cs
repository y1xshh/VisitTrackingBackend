namespace VisitTracking.Domain.Entities;

public partial class Visitattachment
{
    public int Id { get; set; }

    public int? VisitId { get; set; }

    public string? FileName { get; set; }

    public string? FilePath { get; set; }

    public string? FileType { get; set; }

    public bool? IsActive { get; set; }

    public string? InsertedBy { get; set; }

    public DateTime? InsertedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual Visit? Visit { get; set; }
}
