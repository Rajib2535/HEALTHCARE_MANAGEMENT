using System;
using System.Collections.Generic;

namespace DATA.Models;

public partial class Permission
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int? MenuId { get; set; }

    public bool? IsActive { get; set; }

    public virtual Menu Menu { get; set; }

    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
