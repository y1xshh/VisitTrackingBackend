using System;
using System.Collections.Generic;

namespace VisitTracking.Domain.Entities;

public partial class Company
{
    public int Id { get; set; }

    public string? CompanyName { get; set; }

    public string? CompanyType { get; set; }

    public string? IndustryType { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Pincode { get; set; }

    public bool? IsActive { get; set; }

    public string? InsertedBy { get; set; }

    public DateTime? InsertedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual ICollection<Contactperson> Contactpeople { get; set; } = new List<Contactperson>();

    public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();
}
