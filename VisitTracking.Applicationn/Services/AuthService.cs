using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VisitTracking.Application.DTOs;
using VisitTracking.Application.Helper;
using VisitTracking.Application.Interface;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;
using VisitTracking.Infrastructure.Data;

namespace VisitTracking.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _repo;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;
        private readonly AppDbContext _context;
        private readonly IAuditLogService _auditService;

        public AuthService(
            IUserRepository repo,
            IConfiguration config,
            IEmailService emailService,
            AppDbContext context,
            IAuditLogService auditLogService)
        {
            _repo = repo;
            _config = config;
            _emailService = emailService;
            _context = context;
            _auditService = auditLogService;
        }

        public async Task<string> Register(RegisterDTo dto)
        {
            var email = dto.Email?.Trim();
            if (string.IsNullOrWhiteSpace(email))
                return "Email is required";

            var existing = await _repo.GetByEmailAsync(email);
            if (existing != null)
                return "User already exists";

            var user = new User
            {
                FullName = dto.FullName,
                Email = email,
                Mobile = dto.Mobile,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash)
            };

            await _repo.AddAsync(user);

            return "Registered successfully. Waiting for admin approval.";
        }

        public async Task<LoginResponseDto> Login(LoginDto dto)
        {
            // 1. User exists?
            var user = await _repo.GetByEmailAsync(dto.Email);

            if (user == null)
            {
                return new LoginResponseDto
                {
                    Token = string.Empty,
                    Role = string.Empty,
                    EmployeeId = 0,
                    IsFirstLogin = false,
                    Message = "Invalid credentials"
                };
            }

            // 3. Active?
            if (user.IsActive != true)
            {
                return new LoginResponseDto
                {
                    Token = string.Empty,
                    Role = string.Empty,
                    EmployeeId = 0,
                    Name = string.Empty,
                    IsFirstLogin = false,
                    Department = string.Empty,
                    Message = "Your account is inactive. Contact admin."
                };
            }

            // 4. First login?
            if (user.IsFirstLogin == true)
            {
                var tempToken = GenerateTempJwt(user.Email!);
                var profile = await GetLoginProfileAsync(user);
                return new LoginResponseDto
                {
                    Token = string.Empty,
                    Role = string.Empty,
                    EmployeeId = profile.EmployeeId,
                    Name = profile.Name,
                    Department = profile.Department,
                    IsFirstLogin = true,
                    IsFirstLoginRequired = true,
                    TempToken = tempToken,
                    Message = "First login detected. Please change your password using the temp token."
                };
            }

            // 5. Generate JWT
            var role = await _context.Set<Role>()
                .Where(r => r.Id == user.RoleId)
                .Select(r => r.RoleName)
                .FirstOrDefaultAsync();

            var loginProfile = await GetLoginProfileAsync(user);

            return new LoginResponseDto
            {
                Token = GenerateJwt(user, role ?? string.Empty, loginProfile.EmployeeId, loginProfile.Designation),
                Role = role ?? string.Empty,
                EmployeeId = loginProfile.EmployeeId,
                Name = loginProfile.Name,
                Department = loginProfile.Department,
                IsFirstLogin = user.IsFirstLogin ?? false,
                Message = "Login successful"
            };
        }

        private async Task<(int EmployeeId, string Name, string Department, string Designation)> GetLoginProfileAsync(User user)
        {
            var employee = await _context.Set<Employee>()
                .Include(e => e.Designation)
                .Where(e => e.UserId == user.Id)
                .FirstOrDefaultAsync();

            var departmentId = user.DepartmentId;

            if (!departmentId.HasValue && user.DesignationId.HasValue)
            {
                departmentId = await _context.Set<MstDesignation>()
                    .Where(d => d.Id == user.DesignationId.Value)
                    .Select(d => d.DepartmentId)
                    .FirstOrDefaultAsync();
            }

            var department = departmentId.HasValue
                ? await _context.Set<Department>()
                    .Where(d => d.Id == departmentId.Value)
                    .Select(d => d.DepartmentName)
                    .FirstOrDefaultAsync()
                : string.Empty;

            var designation = employee?.Designation?.DesignationName ??
                (user.DesignationId.HasValue
                    ? await _context.Set<MstDesignation>()
                        .Where(d => d.Id == user.DesignationId.Value)
                        .Select(d => d.DesignationName)
                        .FirstOrDefaultAsync()
                    : string.Empty);

            return (
                employee?.Id ?? 0,
                user.FullName ?? string.Empty,
                department ?? string.Empty,
                designation ?? string.Empty
            );
        }

        private string GenerateTempJwt(string email)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim("temp", "firstlogin")
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateJwt(User user, string role, int employeeId, string designation)
        {
            var claims = new[]
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim("EmployeeId", employeeId.ToString()),
                new Claim("Designation", designation),
                new Claim(ClaimTypes.Name, user.FullName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, role),
                new Claim("designationId", user.DesignationId?.ToString() ?? string.Empty),
                new Claim("departmentId", user.DepartmentId?.ToString() ?? string.Empty)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> CreateUserByAdmin(CreateUserByAdminDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var roleExists = await _context.Set<Role>().AnyAsync(x => x.Id == dto.RoleId);
                var deptExists = await _context.Set<Department>().AnyAsync(x => x.Id == dto.DepartmentId);

                if (!roleExists) return "Invalid RoleId";
                if (!deptExists) return "Invalid DepartmentId";

                if (dto.ReportingManagerId.HasValue)
                {
                    var managerExists = await _context.Set<Employee>()
                        .AnyAsync(x => x.Id == dto.ReportingManagerId.Value);

                    if (!managerExists)
                        return $"ReportingManagerId {dto.ReportingManagerId} not found in employees table";
                }

                var fullName = dto.FullName?.Trim() ?? throw new Exception("FullName is required");
                var email = dto.Email?.Trim() ?? throw new Exception("Email is required");
                var mobile = dto.Mobile?.Trim() ?? throw new Exception("Mobile is required");
                var randomPassword = GeneratePassword();

                var user = new User
                {
                    FullName = fullName,
                    Email = email,
                    Mobile = mobile,
                    RoleId = dto.RoleId,
                    DepartmentId = dto.DepartmentId,
                    DesignationId = dto.DesignationId,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(randomPassword),
                    IsFirstLogin = true,
                    IsActive = true,
                    InsertedBy = "admin",
                    InsertedDate = DateTime.UtcNow
                };

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                var employee = new Employee
                {
                    UserId = user.Id,
                    EmployeeCode = $"EMP{user.Id}",
                    DesignationId = dto.DesignationId > 0 ? dto.DesignationId : null,
                    ReportingManagerId = dto.ReportingManagerId,
                    IsActive = true,
                    InsertedBy = "admin",
                    InsertedDate = DateTime.UtcNow
                };

                await _context.Set<Employee>().AddAsync(employee);
                await _context.SaveChangesAsync();

                var body = EmpRegEmailTemplate.Build(fullName, email, randomPassword);

                await _emailService.SendEmailAsync(
                    email,
                    "Account Created",
                    body
                );

                await transaction.CommitAsync();
                return "User created successfully";
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task CreateEmployee(EmployeeUserDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var roleExists = await _context.Set<Role>().AnyAsync(x => x.Id == dto.RoleId);
                var deptExists = await _context.Set<Department>().AnyAsync(x => x.Id == dto.DepartmentId);

                if (!roleExists)
                    throw new Exception("Invalid RoleId");

                if (!deptExists)
                    throw new Exception("Invalid DepartmentId");

                var fullName = dto.FullName?.Trim() ?? throw new Exception("FullName is required");
                var email = dto.Email?.Trim() ?? throw new Exception("Email is required");
                var mobile = dto.Mobile?.Trim() ?? throw new Exception("Mobile is required");
                var randomPassword = GeneratePassword();

                var user = new User
                {
                    FullName = fullName,
                    Email = email,
                    Mobile = mobile,
                    RoleId = dto.RoleId,
                    DepartmentId = dto.DepartmentId,
                    DesignationId = dto.DesignationId,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(randomPassword),
                    IsActive = true,
                    IsFirstLogin = true,
                    InsertedBy = "admin",
                    InsertedDate = DateTime.UtcNow
                };

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                // ✅ VALIDATE REPORTING MANAGER (FROM EMPLOYEE TABLE)
                if (dto.ReportingManagerId.HasValue)
                {
                    var managerExists = await _context.Set<Employee>()
                        .AnyAsync(x => x.Id == dto.ReportingManagerId.Value);

                    if (!managerExists)
                    {
                        throw new Exception($"ReportingManagerId {dto.ReportingManagerId} not found in employees table");
                    }
                }

                var employee = new Employee
                {
                    UserId = user.Id,
                    EmployeeCode = string.IsNullOrEmpty(dto.EmployeeCode) ? $"EMP{user.Id}" : dto.EmployeeCode,
                    DesignationId = dto.DesignationId > 0 ? dto.DesignationId : null,

                    ReportingManagerId = dto.ReportingManagerId, 
                    LocationId = dto.LocationId,


                };

                await _context.Set<Employee>().AddAsync(employee);
                await _context.SaveChangesAsync();     
                if (employee.ReportingManagerId == employee.Id)
                {
                    throw new Exception("Employee cannot be their own manager");
                }
                var body = EmpRegEmailTemplate.Build(
                    fullName,
                    email,
                    randomPassword
                );

                await _emailService.SendEmailAsync(
                    email,
                    "Employee Account Created",
                    body
                );
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<string> ChangePassword(ChangePasswordDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
                return "User not found";

            if (!BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.PasswordHash))
                return "Old password is incorrect";

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.IsFirstLogin = false;
            _context.Users.Update(user);
            await _context.SaveChangesAsync(); 

            return "Password changed successfully";
        }

        public async Task<string> ApproveUser(ApproveUserDto dto)
        {
            var user = await _repo.GetByIdAsync(dto.UserId);

            if (user == null)
                return "User not found";

            user.IsActive = true;

            await _repo.UpdateAsync(user);

            return "User approved successfully";
        }

        private static string GeneratePassword()
        {
            return "Emp@" + new Random().Next(1000, 9999);
        }

        public Task User(UserDto dto)
        {
            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Mobile = dto.Mobile,
            };

            return _repo.AddAsync(user);
        }

        public async Task UpdateAsync(User user)
        {
            var existingUser = await _repo.GetByIdAsync(user.Id)
                ?? throw new Exception("User not found");

            existingUser.FullName = user.FullName;
            existingUser.Email = user.Email;
            existingUser.Mobile = user.Mobile;
            existingUser.PasswordHash = user.PasswordHash;
            existingUser.IsFirstLogin = user.IsFirstLogin;

            await _repo.UpdateAsync(existingUser);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var user = await _repo.GetByEmailAsync(email);

            if (user == null)
                throw new Exception("User not found");

            return user;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            var user = await _repo.GetByIdAsync(userId);

            if (user == null)
                throw new Exception("User not found");

            return user;
        }

        public async Task<LoginResponseDto> FirstLoginChangePasswordAsync(FirstLoginChangePasswordDto dto)
        {
            // Parse and validate temp token
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(dto.TempToken);

            if (jsonToken.ValidTo < DateTime.UtcNow)
                return new LoginResponseDto { Message = "Temp token expired", IsFirstLoginRequired = false };

            var emailClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(emailClaim) || jsonToken.Claims.FirstOrDefault(c => c.Type == "temp" && c.Value == "firstlogin") == null)
                return new LoginResponseDto { Message = "Invalid temp token", IsFirstLoginRequired = false };

            var user = await _repo.GetByEmailAsync(emailClaim);
            if (user == null || user.IsFirstLogin != true)
                return new LoginResponseDto { Message = "Invalid first login token", IsFirstLoginRequired = false };

            // Validate password policy
            if (!IsValidPassword(dto.NewPassword))
                return new LoginResponseDto { Message = "Password must be 8+ chars with Upper, Lower, Digit, Special char", IsFirstLoginRequired = false };

            // Update password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.IsFirstLogin = false;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            // Audit log
            await _auditService.CreateAsync(new AuditLogDto
            {
                TableName = "Users",
                RecordId = user.Id,
                ActionType = "FirstLoginPasswordChange",
                ActionBy = 0, // system
                NewValueJson = "{}" // no sensitive data
            });

            // Get login data
            var role = await _context.Set<Role>()
                .Where(r => r.Id == user.RoleId)
                .Select(r => r.RoleName)
                .FirstOrDefaultAsync();

            var loginProfile = await GetLoginProfileAsync(user);

            return new LoginResponseDto
            {
                Token = GenerateJwt(user, role ?? string.Empty, loginProfile.EmployeeId, loginProfile.Designation),
                Role = role ?? string.Empty,
                EmployeeId = loginProfile.EmployeeId,
                Name = loginProfile.Name,
                Department = loginProfile.Department,
                IsFirstLogin = false,
                Message = "Password changed successfully. Welcome!"
            };
        }

        private static bool IsValidPassword(string password)
        {
            if (password.Length < 8) return false;
            if (!System.Text.RegularExpressions.Regex.IsMatch(password, @"[A-Z]")) return false;
            if (!System.Text.RegularExpressions.Regex.IsMatch(password, @"[a-z]")) return false;
            if (!System.Text.RegularExpressions.Regex.IsMatch(password, @"[0-9]")) return false;
            if (!System.Text.RegularExpressions.Regex.IsMatch(password, @"[!@#$%^&*]")) return false;
            return true;
        }
    }
}
