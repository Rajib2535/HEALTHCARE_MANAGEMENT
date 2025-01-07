using DATA.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DATA.Models.ViewModels.UserPermissionViewModels
{
    public partial class Comapiroleperm
    {
        public int Id { get; set; }
        public int? RoleId { get; set; }
        public int? PermissionId { get; set; }

        public virtual RolePermission Permission { get; set; }
        public virtual Role Role { get; set; }

        public IEnumerable<SelectListItem> DropDownListForPermission { get; set; }

        public SelectList DropDownListForRole { get; set; }

        public int[] SelectedValues { get; set; }
    }
}
