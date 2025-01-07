using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.Models.ViewModels
{
    public class Message
    {
        public string[] registration_ids { get; set; }
        public Notification notification { get; set; } = new Notification();
        public object data { get; set; }
        public class Notification
        {
            public string title { get; set; }
            public string body { get; set; }
        }
    }

}
