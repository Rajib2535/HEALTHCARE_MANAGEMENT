using DATA.Models;

namespace DATA.Models.ViewModels.UserPermissionViewModels
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
