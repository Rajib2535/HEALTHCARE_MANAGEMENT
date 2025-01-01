using System;
using System.Collections.Generic;

namespace DATA.Models;

public partial class User
{
    public int Id { get; set; }

    public string Userid { get; set; }

    public string Password { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Mobile { get; set; }

    public string Type { get; set; }

    public bool Status { get; set; }

    public string LoginType { get; set; }

    public DateTime? CreateTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public bool IsPasswordReset { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
