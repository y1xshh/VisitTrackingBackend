using Microsoft.EntityFrameworkCore;
using VisitTracking.Domain.Entities;

namespace VisitTracking.Infrastructure.Data;

public partial class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }


    public virtual DbSet<Auditlog> Auditlogs { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Contactperson> Contactpersons { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Expenseapproval> Expenseapprovals { get; set; }

    public virtual DbSet<Expenserate> Expenserates { get; set; }

    public virtual DbSet<Funnelstage> Funnelstages { get; set; }

    public virtual DbSet<MstDesignation> MstDesignations { get; set; }

    public virtual DbSet<MstLocation> MstLocations { get; set; }

    public virtual DbSet<Organisation> Organisations { get; set; }

    public virtual DbSet<Outcometype> Outcometypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vehicletype> Vehicletypes { get; set; }

    public virtual DbSet<Visit> Visits { get; set; }

    public virtual DbSet<Visitattachment> Visitattachments { get; set; }

    public virtual DbSet<Visitfollowup> Visitfollowups { get; set; }

    public virtual DbSet<Visitpurpose> Visitpurposes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql("name=ConnectionStrings:DefaultConnection", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.45-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Auditlog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("auditlogs");

            entity.Property(e => e.ActionType).HasMaxLength(50);
            entity.Property(e => e.InsertedBy)
                .HasMaxLength(50)
                .HasColumnName("inserted_by");
            entity.Property(e => e.InsertedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("inserted_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("'1'")
                .HasColumnName("is_active");
            entity.Property(e => e.NewValueJson).HasColumnType("text");
            entity.Property(e => e.OldValueJson).HasColumnType("text");
            entity.Property(e => e.TableName).HasMaxLength(100);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("updated_date");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("companies");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.CompanyName).HasMaxLength(200);
            entity.Property(e => e.CompanyType).HasMaxLength(100);
            entity.Property(e => e.IndustryType).HasMaxLength(100);
            entity.Property(e => e.InsertedBy)
                .HasMaxLength(50)
                .HasColumnName("inserted_by");
            entity.Property(e => e.InsertedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("inserted_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("'1'")
                .HasColumnName("is_active");
            entity.Property(e => e.Pincode).HasMaxLength(10);
            entity.Property(e => e.State).HasMaxLength(100);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("updated_date");
        });

        modelBuilder.Entity<Contactperson>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("contactpersons");

            entity.HasIndex(e => e.CompanyId, "CompanyId");

            entity.HasIndex(e => e.OrganisationId, "OrganisationId");

            entity.HasIndex(e => e.DepartmentId, "ck_contactp_department");

            entity.Property(e => e.Designation).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.InsertedBy)
                .HasMaxLength(50)
                .HasColumnName("inserted_by");
            entity.Property(e => e.InsertedDate)
                .HasColumnType("datetime")
                .HasColumnName("inserted_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("'1'")
                .HasColumnName("is_active");
            entity.Property(e => e.Mobile).HasMaxLength(20);
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.Remarks).HasMaxLength(255);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.Company).WithMany(p => p.Contactpeople)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("contactpersons_ibfk_1");

            entity.HasOne(d => d.Department).WithMany(p => p.Contactpeople)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("ck_contactp_department");

            entity.HasOne(d => d.Organisation).WithMany(p => p.Contactpeople)
                .HasForeignKey(d => d.OrganisationId)
                .HasConstraintName("contactpersons_ibfk_2");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("departments");

            entity.HasIndex(e => e.OrganisationId, "OrganisationId");

            entity.Property(e => e.DepartmentName).HasMaxLength(150);
            entity.Property(e => e.InsertedBy)
                .HasMaxLength(50)
                .HasColumnName("inserted_by");
            entity.Property(e => e.InsertedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("inserted_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("'1'")
                .HasColumnName("is_active");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.Organisation).WithMany(p => p.Departments)
                .HasForeignKey(d => d.OrganisationId)
                .HasConstraintName("departments_ibfk_1");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("employees");

            entity.HasIndex(e => e.ReportingManagerId, "ReportingManagerId");

            entity.HasIndex(e => e.UserId, "UserId");

            entity.HasIndex(e => e.LocationId, "ck_employee_location");

            entity.HasIndex(e => e.DesignationId, "fk_employees_designation");

            entity.Property(e => e.EmployeeCode).HasMaxLength(50);
            entity.Property(e => e.InsertedBy)
                .HasMaxLength(50)
                .HasColumnName("inserted_by");
            entity.Property(e => e.InsertedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("inserted_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("'1'")
                .HasColumnName("is_active");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.Designation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DesignationId)
                .HasConstraintName("fk_employees_designation");

            entity.HasOne(d => d.Location).WithMany(p => p.Employees)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("ck_employee_location");

            entity.HasOne(d => d.ReportingManager).WithMany(p => p.InverseReportingManager)
                .HasForeignKey(d => d.ReportingManagerId)
                .HasConstraintName("employees_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.Employees)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("employees_ibfk_1");
        });

        modelBuilder.Entity<Expenseapproval>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("expenseapprovals");

            entity.HasIndex(e => e.VisitId, "VisitId");

            entity.Property(e => e.ApprovalRemarks).HasMaxLength(255);
            entity.Property(e => e.ApprovalStatus).HasMaxLength(50);
            entity.Property(e => e.ApprovedAt).HasColumnType("datetime");
            entity.Property(e => e.InsertedBy)
                .HasMaxLength(50)
                .HasColumnName("inserted_by");
            entity.Property(e => e.InsertedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("inserted_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("'1'")
                .HasColumnName("is_active");
            entity.Property(e => e.SubmittedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.Visit).WithMany(p => p.Expenseapprovals)
                .HasForeignKey(d => d.VisitId)
                .HasConstraintName("expenseapprovals_ibfk_1");
        });

        modelBuilder.Entity<Expenserate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("expenserates");

            entity.HasIndex(e => e.VehicleTypeId, "VehicleTypeId");

            entity.Property(e => e.InsertedBy)
                .HasMaxLength(50)
                .HasColumnName("inserted_by");
            entity.Property(e => e.InsertedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("inserted_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("'1'")
                .HasColumnName("is_active");
            entity.Property(e => e.RatePerKm).HasPrecision(10, 2);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.VehicleType).WithMany(p => p.Expenserates)
                .HasForeignKey(d => d.VehicleTypeId)
                .HasConstraintName("expenserates_ibfk_1");
        });

        modelBuilder.Entity<Funnelstage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("funnelstages");

            entity.Property(e => e.InsertedBy)
                .HasMaxLength(50)
                .HasColumnName("inserted_by");
            entity.Property(e => e.InsertedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("inserted_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("'1'")
                .HasColumnName("is_active");
            entity.Property(e => e.StageName).HasMaxLength(150);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("updated_date");
        });

        modelBuilder.Entity<MstDesignation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mst_designation");

            entity.HasIndex(e => e.DepartmentId, "idx_department");

            entity.HasIndex(e => new { e.DesignationName, e.DepartmentId }, "uk_designation_department").IsUnique();

            entity.Property(e => e.DesignationName).HasMaxLength(150);
            entity.Property(e => e.InsertedBy)
                .HasMaxLength(50)
                .HasColumnName("inserted_by");
            entity.Property(e => e.InsertedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("inserted_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("'1'")
                .HasColumnName("is_active");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.Department).WithMany(p => p.DesignationName)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("fk_designation_department");
        });

        modelBuilder.Entity<MstLocation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mst_location");

            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .HasColumnName("country");
            entity.Property(e => e.InsertedBy)
                .HasMaxLength(50)
                .HasColumnName("inserted_by");
            entity.Property(e => e.InsertedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("inserted_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("'1'")
                .HasColumnName("is_active");
            entity.Property(e => e.LocationName).HasMaxLength(50);
            entity.Property(e => e.State).HasMaxLength(50);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("updated_date");
        });

        modelBuilder.Entity<Organisation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("organisations");

            entity.HasIndex(e => e.CompanyId, "CompanyId");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.InsertedBy)
                .HasMaxLength(50)
                .HasColumnName("inserted_by");
            entity.Property(e => e.InsertedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("inserted_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("'1'")
                .HasColumnName("is_active");
            entity.Property(e => e.OrganisationName).HasMaxLength(200);
            entity.Property(e => e.State).HasMaxLength(100);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("updated_date");
        });

        modelBuilder.Entity<Outcometype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("outcometypes");

            entity.Property(e => e.InsertedBy)
                .HasMaxLength(50)
                .HasColumnName("inserted_by");
            entity.Property(e => e.InsertedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("inserted_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("'1'")
                .HasColumnName("is_active");
            entity.Property(e => e.OutcomeName).HasMaxLength(150);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("updated_date");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("roles");

            entity.Property(e => e.InsertedBy)
                .HasMaxLength(50)
                .HasColumnName("inserted_by");
            entity.Property(e => e.InsertedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("inserted_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("'1'")
                .HasColumnName("is_active");
            entity.Property(e => e.RoleName).HasMaxLength(100);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("updated_date");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "Email").IsUnique();

            entity.HasIndex(e => e.RoleId, "RoleId");

            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FullName).HasMaxLength(150);
            entity.Property(e => e.InsertedBy)
                .HasMaxLength(50)
                .HasColumnName("inserted_by");
            entity.Property(e => e.InsertedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("inserted_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("'1'")
                .HasColumnName("is_active");
            entity.Property(e => e.IsFirstLogin)
                .IsRequired()
                .HasDefaultValueSql("'1'");
            entity.Property(e => e.Mobile).HasMaxLength(20);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("users_ibfk_1");
        });

        modelBuilder.Entity<Vehicletype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("vehicletypes");

            entity.Property(e => e.DefaultRatePerKm).HasPrecision(10, 2);
            entity.Property(e => e.InsertedBy)
                .HasMaxLength(50)
                .HasColumnName("inserted_by");
            entity.Property(e => e.InsertedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("inserted_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("'1'")
                .HasColumnName("is_active");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("updated_date");
            entity.Property(e => e.VehicleName).HasMaxLength(100);
        });

        modelBuilder.Entity<Visit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("visits");

            entity.HasIndex(e => e.CompanyId, "CompanyId");

            entity.HasIndex(e => e.EmployeeId, "EmployeeId");

            entity.Property(e => e.ActualBusinessValue).HasPrecision(18, 2);
            entity.Property(e => e.AttachmentPath).HasMaxLength(255);
            entity.Property(e => e.CheckInTime).HasColumnType("datetime");
            entity.Property(e => e.CheckOutTime).HasColumnType("datetime");
            entity.Property(e => e.DiscussionSummary).HasColumnType("text");
            entity.Property(e => e.DistanceKm).HasPrecision(10, 2);
            entity.Property(e => e.ExpectedBusinessValue).HasPrecision(18, 2);
            entity.Property(e => e.InsertedBy)
                .HasMaxLength(50)
                .HasColumnName("inserted_by");
            entity.Property(e => e.InsertedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("inserted_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("'1'")
                .HasColumnName("is_active");
            entity.Property(e => e.Latitude).HasPrecision(10, 6);
            entity.Property(e => e.Longitude).HasPrecision(10, 6);
            entity.Property(e => e.NextAction).HasMaxLength(255);
            entity.Property(e => e.NextFollowUpDate).HasColumnType("datetime");
            entity.Property(e => e.RateAppliedPerKm).HasPrecision(10, 2);
            entity.Property(e => e.Remarks).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TravelExpenseAmount).HasPrecision(10, 2);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("updated_date");
            entity.Property(e => e.VisitCode).HasMaxLength(50);
            entity.Property(e => e.VisitDate).HasColumnType("datetime");

            entity.HasOne(d => d.Company).WithMany(p => p.Visits)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("visits_ibfk_2");

            entity.HasOne(d => d.Employee).WithMany(p => p.Visits)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("visits_ibfk_1");
        });

        modelBuilder.Entity<Visitattachment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("visitattachments");

            entity.HasIndex(e => e.VisitId, "VisitId");

            entity.Property(e => e.FileName).HasMaxLength(255);
            entity.Property(e => e.FilePath).HasMaxLength(255);
            entity.Property(e => e.FileType).HasMaxLength(255);
            entity.Property(e => e.InsertedBy)
                .HasMaxLength(50)
                .HasColumnName("inserted_by");
            entity.Property(e => e.InsertedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("inserted_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("'1'")
                .HasColumnName("is_active");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.Visit).WithMany(p => p.Visitattachments)
                .HasForeignKey(d => d.VisitId)
                .HasConstraintName("visitattachments_ibfk_1");
        });

        modelBuilder.Entity<Visitfollowup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("visitfollowups");

            entity.HasIndex(e => e.VisitId, "VisitId");

            entity.Property(e => e.ActualBusinessValue).HasPrecision(18, 2);
            entity.Property(e => e.ExpectedBusinessValue).HasPrecision(18, 2);
            entity.Property(e => e.FollowUpDate).HasColumnType("datetime");
            entity.Property(e => e.FollowUpRemarks).HasMaxLength(255);
            entity.Property(e => e.InsertedBy)
                .HasMaxLength(50)
                .HasColumnName("inserted_by");
            entity.Property(e => e.InsertedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("inserted_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("'1'")
                .HasColumnName("is_active");
            entity.Property(e => e.NextFollowUpDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.Visit).WithMany(p => p.Visitfollowups)
                .HasForeignKey(d => d.VisitId)
                .HasConstraintName("visitfollowups_ibfk_1");
        });

        modelBuilder.Entity<Visitpurpose>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("visitpurposes");

            entity.Property(e => e.InsertedBy)
                .HasMaxLength(50)
                .HasColumnName("inserted_by");
            entity.Property(e => e.InsertedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("inserted_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("'1'")
                .HasColumnName("is_active");
            entity.Property(e => e.PurposeName).HasMaxLength(150);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("updated_date");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
