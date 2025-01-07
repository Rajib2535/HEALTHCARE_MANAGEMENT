using System.ComponentModel.DataAnnotations;

namespace DATA.Models.ViewModels.UserPermissionViewModels
{
    public class Login
    {
        [Display(Name = "Username")]
        public string? userName { get; set; }


        [Display(Name = "Password")]
        public string? password { get; set; }

    }
}
