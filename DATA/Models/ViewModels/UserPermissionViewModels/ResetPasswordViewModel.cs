using System.ComponentModel.DataAnnotations;

namespace CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.ViewModels.UserPermissionViewModels
{
    public partial class ResetPasswordViewModel
    {
        public int UserId { get; set; }
        [Required(ErrorMessage ="Password cannot be empty")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Confirm password cannot be empty")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Passwords do not match! Please try again!")]
        public string? ConfirmPassword { get; set; }
        [Required]
        public string? CurrentPassword { get; set; }
    }
    public partial class ResetPasswordViewModel : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();
            if (Password.Trim() == CurrentPassword.Trim())
            {
                validationResults.Add(new ValidationResult("You cannot use current password as your new password", new[] { "Password" }));
            }
            else if(ConfirmPassword.Trim() == CurrentPassword.Trim())
            {
                validationResults.Add(new ValidationResult("You cannot use current password as your new password", new[] { "ConfirmPassword" }));
            }
            return validationResults;
        }
    }
}
