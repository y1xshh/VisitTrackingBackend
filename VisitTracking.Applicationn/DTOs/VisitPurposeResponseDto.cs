using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitTracking.Application.DTOs
{
    public class VisitPurposeResponseDto
    {
        public int Id { get; set; }
        public string? PurposeName { get; set; }
        public bool? IsActive { get; set; }
    }
}