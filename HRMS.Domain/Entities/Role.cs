using Microsoft.AspNetCore.Identity;

namespace HRMS.Domain.Entities
{
    public class Role : IdentityRole<long>
    {
        public ICollection<UserRole>? UserRoles { get; set; }
    }
}
