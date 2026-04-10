using System;
using System.Collections.Generic;

namespace VisitTracking.Domain.Entities;

public partial class Expenserate
{
    public int Id { get; set; }

    public int? VehicleTypeId { get; set; }

    public decimal? RatePerKm { get; set; }

    public DateOnly? EffectiveFrom { get; set; }

    public DateOnly? EffectiveTo { get; set; }

    public bool? IsActive { get; set; }

    public string? InsertedBy { get; set; }

    public DateTime? InsertedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual Vehicletype? VehicleType { get; set; }
}
