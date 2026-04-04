using System;
using System.Collections.Generic;

namespace VisitTracking.Domain.Entities;

public partial class Organisation
{
    public int Id { get; set; }

    public string? OrganisationName { get; set; }

    public int? CompanyId { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public bool? IsActive { get; set; }

    public string? InsertedBy { get; set; }

    public DateTime? InsertedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual Company? Company { get; set; }

    public virtual ICollection<Contactperson> Contactpeople { get; set; } = new List<Contactperson>();

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();
}
