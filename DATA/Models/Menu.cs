using System;
using System.Collections.Generic;

namespace DATA.Models;

public partial class Menu
{
    public int Id { get; set; }

    public string Url { get; set; }

    public int? ParentMenu { get; set; }

    public bool? IsActive { get; set; }

    public string MenuName { get; set; }

    public int? MenuOrder { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}
