using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.RequestReponseModels
{
    public class ServerSideDatatableResponseEntity
    {
        public string? draw { get; set; }
        public object? data { get; set; }
        public long recordsTotal { get; set; }
        public long recordsFiltered { get; set; }
    }
}
