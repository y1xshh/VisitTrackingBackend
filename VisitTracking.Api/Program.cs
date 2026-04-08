using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VisitTracking.Application.Filter;
using VisitTracking.Application.Interface;
using VisitTracking.Application.Services;
using VisitTracking.Domain.RepositoryInterfaces;
using VisitTracking.Infrastructure.Data;
using VisitTracking.Infrastructure.Repositories;




var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 36))
    ));

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IOrganisationRepository, OrganisationRepository>();
builder.Services.AddScoped<IOrganisationService, OrganisationService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IContactpersonRepository, ContactpersonRepository>();
builder.Services.AddScoped<IContactpersonService, ContactpersonService>();
builder.Services.AddScoped<IVisitPurposeRepository, VisitpurposeRepository>();
builder.Services.AddScoped<IVisitPurposeService, VisitPurposeService>();
builder.Services.AddScoped<IVisitRepository, VisitRepository>();
builder.Services.AddScoped<IVisitService, VisitService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IVisitRepository, VisitRepository>();
builder.Services.AddScoped<IVehicleTypeRepository, VehicleTypeRepository>();
builder.Services.AddScoped<IVehicleTypeService, VehicleTypeService>(); // ✅ ADD THIS
builder.Services.AddScoped<IVisitService, VisitService>();
builder.Services.AddScoped<IExpenserateRepository, ExpenserateRepository>();
builder.Services.AddScoped<IExpenserateService, ExpenserateService>();
builder.Services.AddScoped<IFunnelStageRepository, FunnelStageRepository>();
builder.Services.AddScoped<IFunnelStageService, FunnelStageService>();
builder.Services.AddScoped<IOutcomeTypeRepository, OutcomeTypeRepository>();
builder.Services.AddScoped<IOutcomeTypeService, OutcomeTypeService>();
builder.Services.AddScoped<IVisitFollowUpRepository, VisitFollowUpRepository>();
builder.Services.AddScoped<IVisitFollowUpService, VisitFollowUpService>();
builder.Services.AddScoped<IVisitAttachmentRepository, VisitAttachmentRepository>();
builder.Services.AddScoped<IVisitAttachmentService, VisitAttachmentService>();
builder.Services.AddScoped<IExpenseApprovalRepository, ExpenseApprovalRepository>();
builder.Services.AddScoped<IExpenseApprovalService, ExpenseApprovalService>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();
builder.Services.AddScoped<IUserListRepository, UserListRepository>();
builder.Services.AddScoped<IUserListService, UserListService>();
builder.Services.AddScoped<FirstLoginCheckFilter>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy
            .WithOrigins("http://localhost:5174", "https://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
    app.UseSwagger();
}
app.UseRouting();

// CORS must be before Authentication/Authorization
app.UseCors("AllowLocalhost");

// HTTPS redirection - be careful in development with self-signed certs
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
