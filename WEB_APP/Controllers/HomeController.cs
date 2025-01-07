
using CORPORATE_DISBURSEMENT_UTILITY;
using Microsoft.AspNetCore.Mvc;
using SERVICE.Interface;
using System.Diagnostics;
using UTILITY;
using WEB_APP.Extensions;
using WEB_APP.Models;
using ILogger = Serilog.ILogger;

namespace WEB_APP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;
        private readonly IDashboardManager _dashboardManager;

        public HomeController(ILogger logger, IDashboardManager dashboardManager)
        {
            _logger = logger;
            _dashboardManager = dashboardManager;
        }
        //[TypeFilter(typeof(CheckSessionWithRedirect))]
        public IActionResult Index()
        {
            return View();
        }
        [TypeFilter(typeof(CheckSessionWithRedirect))]
        [TypeFilter(typeof(CheckPermission))]
        public IActionResult Dashboard()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> DashboardCardData()
        {
            return Json(await _dashboardManager.DashboardCards());
        }
        public async Task<IActionResult> APIConnectivityCheck()
        {
            var response = await _dashboardManager.APIConnectivityCheck();
            var data = response.data as List<APIConnectivityStatus>;
            if (data != null)
            {
                response.html = RazorHelper.RenderRazorViewToString(this, "APIConnectivityStatusList_ViewAll", data);
            }
            return Json(response);
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult PermissionDenied()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}