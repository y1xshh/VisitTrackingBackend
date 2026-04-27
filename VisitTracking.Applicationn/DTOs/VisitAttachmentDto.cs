namespace VisitTracking.Application.DTOs
{
    public class VisitAttachmentDto
    {
        public int? VisitId { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public string? FileType { get; set; }
        public bool? IsActive { get; set; }
    }
}