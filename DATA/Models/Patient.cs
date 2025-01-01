using System;
using System.Collections.Generic;

namespace DATA.Models;

public partial class Patient
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public int? Age { get; set; }

    public decimal? Weight { get; set; }

    public string Gender { get; set; }

    public string Address { get; set; }

    public string MedicalHistory { get; set; }

    public string EmergencyContactName { get; set; }

    public string EmergencyContactPhone { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
