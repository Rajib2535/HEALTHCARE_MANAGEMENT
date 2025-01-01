using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.Models.ViewModels
{
    public class PatientRegistrationViewModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }
     


        public int? Age { get; set; }

        public decimal? Weight { get; set; }

        public string Gender { get; set; }

        public string Address { get; set; }

        public string MedicalHistory { get; set; }

        public string EmergencyContactName { get; set; }

        public string EmergencyContactPhone { get; set; }
    }
}
