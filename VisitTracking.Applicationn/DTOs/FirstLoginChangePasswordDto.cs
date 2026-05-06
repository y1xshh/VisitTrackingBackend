using System.ComponentModel.DataAnnotations;

namespace VisitTracking.Application.DTOs
{
    public class FirstLoginChangePasswordDto
    {
        [Required]
        public string TempToken { get; set; } = string.Empty;
        
        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public string NewPassword { get; set; } = string.Empty;
    }
}
