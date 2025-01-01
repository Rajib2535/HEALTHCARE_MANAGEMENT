using DATA.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.ViewModels.UserPermissionViewModels
{
    public partial class ComRolePermissionscs
    {
        public ComRolePermissionscs()
        {
            CommonApiRolePermission = new HashSet<RolePermission>();
        }

        public int ID { get; set; }
        public string? NAME { get; set; }
        public int? MENU_ID { get; set; }
        public bool? IS_ACTIVE { get; set; }
        public virtual Menu Menu { get; set; }
        public virtual ICollection<RolePermission> CommonApiRolePermission { get; set; }
        public SelectList DropDownList { get; set; }
    }
}
