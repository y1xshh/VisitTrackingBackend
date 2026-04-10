using System;
using System.Collections.Generic;

namespace VisitTracking.Domain.Entities;

public partial class Contactperson
{
    public int Id { get; set; }

    public int? CompanyId { get; set; }

    public int? OrganisationId { get; set; }

    public int? DepartmentId { get; set; }

    public string? Name { get; set; }

    public string? Designation { get; set; }

    public string? Mobile { get; set; }

    public string? Email { get; set; }

    public string? Remarks { get; set; }

    public bool? IsActive { get; set; }

    public string? InsertedBy { get; set; }

    public DateTime InsertedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual Company? Company { get; set; }

    public virtual Department? Department { get; set; }

    public virtual Organisation? Organisation { get; set; }
}
