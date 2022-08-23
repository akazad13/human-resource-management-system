using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Domain.Entities
{
    public class User : IdentityUser<long>
    {
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(50, MinimumLength = 3,
        ErrorMessage = "First Name should be minimum 3 characters and a maximum of 50 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(50, MinimumLength = 3,
        ErrorMessage = "Last Name should be minimum 3 characters and a maximum of 50 characters")]
        public string LastName { get; set; }
        public ICollection<UserRole>? UserRoles { get; set; }

    }
}
