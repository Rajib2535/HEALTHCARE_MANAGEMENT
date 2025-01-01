using System.ComponentModel.DataAnnotations;

namespace CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.ViewModels.UserPermissionViewModels
{
    public class Login
    {
        [Display(Name = "Username")]
        public string? userName { get; set; }


        [Display(Name = "Password")]
        public string? password { get; set; }

    }
}
