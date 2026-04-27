namespace VisitTracking.Application.DTOs
{
    public class FunnelStageDto
    {
        public string? StageName { get; set; }
        public string? Stagecode { get; set; }
        public int? StageOrder { get; set; }

        public bool? IsClosedStage { get; set; }
        public bool? IsWonStage { get; set; }
        public bool? IsLostStage { get; set; }

        public bool? IsActive { get; set; }
    }
}