using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.Models.ReportModels
{
    public class LoginHistoryReport
    {
        public string FullName { get; set; }
        public string Role { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime LoginTime { get; set; }
    }
}
