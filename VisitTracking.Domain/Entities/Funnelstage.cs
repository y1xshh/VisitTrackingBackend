namespace VisitTracking.Domain.Entities;

public partial class Funnelstage
{
    public int Id { get; set; }

    public string? StageName { get; set; }

    public int? StageOrder { get; set; }

    public bool? IsClosedStage { get; set; }

    public bool? IsWonStage { get; set; }

    public bool? IsLostStage { get; set; }

    public bool? IsActive { get; set; }

    public string? InsertedBy { get; set; }

    public DateTime? InsertedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
