using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.Models.ViewModels
{
    public class DoctorRegisterViewModel
    {
        public string UserName {  get; set; }
        public string Password { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Specialty { get; set; }

        public string Qualification { get; set; }

        public int? YearsOfExperience { get; set; }

        public string ClinicName { get; set; }

        public string Address { get; set; }

        public TimeOnly? StartTime { get; set; }

        public TimeOnly? EndTime { get; set; }
    }
}
