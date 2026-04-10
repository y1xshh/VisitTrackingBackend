using System;
using System.Collections.Generic;

namespace VisitTracking.Domain.Entities;

public partial class Department
{
    public int Id { get; set; }

    public string? DepartmentName { get; set; }

    public int? OrganisationId { get; set; }

    public bool? IsActive { get; set; }

    public string? InsertedBy { get; set; }

    public DateTime? InsertedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual ICollection<Contactperson> Contactpeople { get; set; } = new List<Contactperson>();

    public virtual ICollection<MstDesignation> MstDesignations { get; set; } = new List<MstDesignation>();

    public virtual Organisation? Organisation { get; set; }
}
