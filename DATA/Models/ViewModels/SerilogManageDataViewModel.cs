using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.ViewModels
{
    public class SerilogManageDataViewModel
    {
        public List<LogData> logDatas { get; set; } = new List<LogData>();
    }
    public class LogData
    {
        public int id { get; set; }
        public string? level { get; set; }
        public string? date { get; set; }
        public string? content { get; set; }
    }
}
