using Microsoft.AspNetCore.Identity;

namespace DATA.Models
{
    public class AppRole : IdentityRole<long>
    {
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
