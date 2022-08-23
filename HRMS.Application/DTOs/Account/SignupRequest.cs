using System.ComponentModel.DataAnnotations;

namespace HRMS.Application.DTOs.Account
{
    public class SignupRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        public long? ManagerId { get; set; }
    }
}
