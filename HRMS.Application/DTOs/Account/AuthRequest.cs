using System.ComponentModel.DataAnnotations;

namespace HRMS.Application.DTOs.Account
{
    public class AuthRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
