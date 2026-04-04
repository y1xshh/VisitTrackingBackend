using System;
using System.Collections.Generic;

namespace VisitTracking.Domain.Entities;

public partial class Visitpurpose
{
    public int Id { get; set; }

    public string? PurposeName { get; set; }

    public bool? IsActive { get; set; }

    public string? InsertedBy { get; set; }

    public DateTime? InsertedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
