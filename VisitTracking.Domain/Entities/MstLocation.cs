using System;
using System.Collections.Generic;

namespace VisitTracking.Domain.Entities;

public partial class MstLocation
{
    public int Id { get; set; }

    public string LocationName { get; set; } = null!;

    public string? State { get; set; }

    public string? Country { get; set; }

    public bool? IsActive { get; set; }

    public string? InsertedBy { get; set; }

    public DateTime? InsertedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
