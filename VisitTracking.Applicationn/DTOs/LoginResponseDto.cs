using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitTracking.Application.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int EmployeeId { get; set; }
        public bool IsFirstLogin { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? Department { get; set; }
    }
}
