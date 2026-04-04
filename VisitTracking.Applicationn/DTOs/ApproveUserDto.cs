using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitTracking.Application.DTOs
{
    public class ApproveUserDto
    { 
        public int UserId { get; set; }
        public bool IsApproved { get; set; }    
        public int? ReportingManagerId { get; set; }
    }
}
