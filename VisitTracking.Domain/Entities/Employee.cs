using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VisitTracking.Domain.Entities;

public partial class Employee
{
    public int Id { get; set; }

    public string? EmployeeCode { get; set; }

    public int? UserId { get; set; }

    public int? DesignationId { get; set; }

    public int? ReportingManagerId { get; set; }

    public int? LocationId { get; set; }

    public bool? IsActive { get; set; }

    public string? InsertedBy { get; set; }

    public DateTime? InsertedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual MstDesignation? Designation { get; set; }

    [JsonIgnore]
    public virtual ICollection<Employee> InverseReportingManager { get; set; } = new List<Employee>();

    public virtual MstLocation? Location { get; set; }

    public virtual Employee? ReportingManager { get; set; }

    public virtual User? User { get; set; }

    [JsonIgnore]
    public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();
}
