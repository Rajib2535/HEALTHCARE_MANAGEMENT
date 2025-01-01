using DATA.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.ViewModels.UserPermissionViewModels
{
    public class AssignRole
    {
        [Key]
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? RoleId { get; set; }

        public virtual Role? Role { get; set; }
        public virtual User? User { get; set; }

        public SelectList? DropDownListForUser { get; set; }

        public SelectList? DropDownListForRole { get; set; }
    }
}
