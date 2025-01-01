using Microsoft.AspNetCore.Mvc;
using COMMON_SERVICE.Interface;

namespace WEB_APP.Controllers
{
    public class CommonController : Controller
    {
        private readonly ICommonService _commonManager;
        public CommonController(ICommonService commonManager)
        {
            _commonManager = commonManager;
        }
        public async Task<IActionResult> GetRefundRequestStatusDropdown()
        {
            return Ok(await _commonManager.GetRefundRequestStatusDropdown());
        }
    }
}
