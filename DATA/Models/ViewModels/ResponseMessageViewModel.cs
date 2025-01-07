using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.Models.ViewModels
{
    public class ResponseMessageViewModel
    {
        public int response_code { get; set; }
        public string response_message { get; set; } = string.Empty;
    }
}
