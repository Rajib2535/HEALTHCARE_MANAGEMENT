using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DATA.Models.ViewModels.UserPermissionViewModels
{
    public class DummyUserRole
    {

        [Display(Name = "User")]
        public string? userName { get; set; }
        public int? ROLE_ID { get; set; }
        public int? USER_ID { get; set; }
        public IEnumerable<SelectListItem>? DropDownListForUser { get; set; }
        public IEnumerable<SelectListItem>? DropDownListForRole { get; set; }
    }
}
