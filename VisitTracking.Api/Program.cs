using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using VisitTracking.Api.Middleware;
using VisitTracking.Application.Filter;
using VisitTracking.Application.Interface;
using VisitTracking.Application.Interfaces;
using VisitTracking.Application.Services;
using VisitTracking.Domain.RepositoryInterfaces;
using VisitTracking.Infrastructure.Data;
using VisitTracking.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 36))
    )
);

builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidateActor = false,
            ValidateTokenReplay = false,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            ),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IOrganisationRepository, OrganisationRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IContactpersonRepository, ContactpersonRepository>();
builder.Services.AddScoped<IVisitPurposeRepository, VisitpurposeRepository>();
builder.Services.AddScoped<IVisitRepository, VisitRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IVehicleTypeRepository, VehicleTypeRepository>();
builder.Services.AddScoped<IExpenserateRepository, ExpenserateRepository>();
builder.Services.AddScoped<IFunnelStageRepository, FunnelStageRepository>();
builder.Services.AddScoped<IOutcomeTypeRepository, OutcomeTypeRepository>();
builder.Services.AddScoped<IVisitFollowUpRepository, VisitFollowUpRepository>();
builder.Services.AddScoped<IVisitAttachmentRepository, VisitAttachmentRepository>();
builder.Services.AddScoped<IExpenseApprovalRepository, ExpenseApprovalRepository>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddScoped<IUserListRepository, UserListRepository>();
builder.Services.AddScoped<IDesignationRepository, DesignationRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IOrganisationService, OrganisationService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IContactpersonService, ContactpersonService>();
builder.Services.AddScoped<IVisitPurposeService, VisitPurposeService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IVisitService, VisitService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IVehicleTypeService, VehicleTypeService>();
builder.Services.AddScoped<IExpenserateService, ExpenserateService>();
builder.Services.AddScoped<IFunnelStageService, FunnelStageService>();
builder.Services.AddScoped<IOutcomeTypeService, OutcomeTypeService>();
builder.Services.AddScoped<IVisitFollowUpService, VisitFollowUpService>();
builder.Services.AddScoped<IVisitAttachmentService, VisitAttachmentService>();
builder.Services.AddScoped<IExpenseApprovalService, ExpenseApprovalService>();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();
builder.Services.AddScoped<IUserListService, UserListService>();
builder.Services.AddScoped<IDesignationService, DesignationService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<FirstLoginCheckFilter>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<FirstLoginCheckFilter>();
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler =
        ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Visit Tracking API",
        Version = "v1",
        Description = "Professional Visit Tracking Backend API"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token as: Bearer {your token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Visit Tracking API V1");
    options.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();
app.MapFallbackToFile("index.html");
app.Run();
