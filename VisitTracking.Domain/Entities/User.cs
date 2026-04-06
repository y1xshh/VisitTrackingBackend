namespace VisitTracking.Domain.Entities
{
 public partial class User
{
    public int Id { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }
   
    public string? Mobile { get; set; }

    public string? PasswordHash { get; set; }

    public int? RoleId { get; set; }

    public int? DesignationId { get; set; }

    public int? DepartmentId { get; set; }

    public bool? IsActive { get; set; }

    public string? InsertedBy { get; set; }

    public DateTime? InsertedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? IsFirstLogin { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual Role? Role { get; set; }
       
    }
}