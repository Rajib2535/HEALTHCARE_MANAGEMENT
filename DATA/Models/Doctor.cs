using System;
using System.Collections.Generic;

namespace DATA.Models;

public partial class Doctor
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public string Specialty { get; set; }

    public string Qualification { get; set; }

    public int? YearsOfExperience { get; set; }

    public string ClinicName { get; set; }

    public string Address { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UserId { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
