using DATA.Models;

namespace CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.ViewModels.UserPermissionViewModels
{
    public class DataObjectsForRolePermission
    {
        public Comapiroleperm rolePermission { get; set; }
        public Role role { get; set; }
        public DataObjectsForRolePermission()
        {
            rolePermission = null;
            role = null;
        }
    }
}
