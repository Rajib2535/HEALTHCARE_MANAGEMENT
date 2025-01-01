using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.RequestReponseModels
{
    public class CommonResponse
    {
        public string? status { get; set; }
        public string? status_title { get; set; }
        public long? timestamp { get; set; }
        public object data { get; set; } = new object();
        public List<string> errors { get; set; } = new List<string>();
        //public class ErrorResponseData
        //{
        //    public string? error_code { get; set; }
        //    public string? error_message { get; set; }
        //}
    }
}
