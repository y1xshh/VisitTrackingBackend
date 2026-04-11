using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitTracking.Application.DTOs
{
        public class LoginResponseDto
        {
            public string Token { get; set; }
            public string Role { get; set; }
            public bool IsFirstLogin { get; set; }
            public string Message { get; set; }
        }
    }

   
