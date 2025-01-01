using Microsoft.AspNetCore.Identity;

namespace DATA.Models
{
    public class ApplicationUserRole : IdentityUserRole<long>
    {
        public virtual AppUser User { get; set; }
        public virtual AppRole Role { get; set; }
    }
}
