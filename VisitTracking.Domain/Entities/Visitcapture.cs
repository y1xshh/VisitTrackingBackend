using System;

namespace VisitTracking.Domain.Entities;

public class VisitCapture
{
    public int Id { get; set; }
    public string? ImageUrl { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
    public DateTime CaptureDate { get; set; }
    public bool? IsActive { get; set; }
    public string? InsertedBy { get; set; }
    public DateTime? InsertedDate { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
