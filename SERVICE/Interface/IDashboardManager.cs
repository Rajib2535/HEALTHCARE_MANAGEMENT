using DATA.Models.RequestReponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICE.Interface
{
    public interface IDashboardManager
    {
        Task<ResponseEntity> DashboardCards();
        Task<ResponseEntity> APIConnectivityCheck();
    }
}
