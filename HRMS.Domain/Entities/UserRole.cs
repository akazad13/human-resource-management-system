using Microsoft.AspNetCore.Identity;

namespace HRMS.Domain.Entities
{
    public class UserRole : IdentityUserRole<long>
    {
        public User? User { get; set; }
        public Role? Role { get; set; }
    }
}
