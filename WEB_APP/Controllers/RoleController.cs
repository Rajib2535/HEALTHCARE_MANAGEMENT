
using DATA.Models;
using Microsoft.AspNetCore.Mvc;
using SERVICE.Interface;
using System.Net;
using System.Text.Json;
using WEB_APP.Extensions;
using ILogger = Serilog.ILogger;

namespace WEB_APP.Controllers
{
    /// <summary>
    /// used for CRUD operation of MonitoringRole
    /// </summary>
    public class RoleController : Controller
    {
        //AdminDBContext db = new AdminDBContext();
        private readonly IRoleManager _iRoleManager;
        private readonly ILogger _logger;
        public RoleController(IRoleManager iRoleManager, ILogger logger)
        {
            _iRoleManager = iRoleManager;
            _logger = logger;
        }
        /// <summary>
        /// action to load the list of role page
        /// </summary>
        /// <returns></returns>
        //[TypeFilter(typeof(CheckSessionWithRedirect), Order = 1)]
        //[TypeFilter(typeof(CheckPermission), Order = 2)]
        public async Task<IActionResult> Index()
        {

            var data = await _iRoleManager.GetAllRoles();
            _logger.Information($"{GetActionWithControllerName()} ==> {WebUtility.HtmlEncode(JsonSerializer.Serialize(data))}");
            return View(data);

        }
        /// <summary>
        /// loads the details of a role by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
       // [TypeFilter(typeof(CheckSessionWithRedirect))]
        public async Task<ActionResult> Details(int id = 0)
        {

            Role role = await _iRoleManager.GetRoleById(id);
            _logger.Information($"{GetActionWithControllerName()} ==> {WebUtility.HtmlEncode(JsonSerializer.Serialize(role))}");
            if (role == null)
            {
                return null;
            }
            return View(role);

        }
        /// <summary>
        /// loads the role creation page
        /// </summary>
        /// <returns></returns>
        //[TypeFilter(typeof(CheckSessionWithRedirect))]
        public ActionResult Create()
        {
            //if (!DBContext.hasPermission("MonitoringRole Management"))
            //{
            //    _logger.Error($"{GetActionWithControllerName()} ==> permission denied");
            //    return RedirectToAction("Action", "Error");
            //}
            //else
            //{
            return View();
            //}
        }

        //
        // POST: /MonitoringRole/Create
        /// <summary>
        /// action to save a new role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        // [TypeFilter(typeof(CheckSessionWithRedirect))]
        public async Task<ActionResult> Create(Role role)
        {
            //if (!DBContext.hasPermission("MonitoringRole Management"))
            //{
            //    _logger.Error($"{GetActionWithControllerName()} ==> permission denied");
            //    return RedirectToAction("Action", "Error");
            //}
            //else
            //{
            _logger.Information($"{GetActionWithControllerName()} ==> {WebUtility.HtmlEncode(JsonSerializer.Serialize(role))}");
            role.IsActive = true;
            if (ModelState.IsValid)
            {
                role.Id = await _iRoleManager.AddRole(role);
                TempData["success"] = "MonitoringRole created successfully";
                return RedirectToAction("Index");
            }
            else
            {


                TempData["failed"] = "MonitoringRole registration failed";
            }

            return View(role);
            //}
        }
        /// <summary>
        /// loads the role edit page by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //
        // GET: /MonitoringRole/Edit/5
        //[TypeFilter(typeof(CheckSessionWithRedirect))]
        public async Task<ActionResult> Edit(int id = 0)
        {

            Role role = await _iRoleManager.GetRoleById(id);
            _logger.Information($"{GetActionWithControllerName()} ==> {WebUtility.HtmlEncode(JsonSerializer.Serialize(role))}");
            if (role == null)
            {
                return null;
            }
            return View(role);

        }

        //
        // POST: /MonitoringRole/Edit/5
        /// <summary>
        /// action to update the role 
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        //[TypeFilter(typeof(CheckSessionWithRedirect))]
        public async Task<ActionResult> Edit(Role role)
        {

            if (ModelState.IsValid)
            {
                //role.IsActive = 1;
                _logger.Information($"{GetActionWithControllerName()} ==> {WebUtility.HtmlEncode(JsonSerializer.Serialize(role))}");
                int result = await _iRoleManager.UpdateRole(role);
                TempData["success"] = "MonitoringRole updated successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["failed"] = "MonitoringRole update failed";
            }
            return View(role);

        }
        /// <summary>
        /// loads the role delete page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //
        // GET: /MonitoringRole/Delete/5
        // [TypeFilter(typeof(CheckSessionWithRedirect))]
        public async Task<ActionResult> Delete(int id = 0)
        {

            Role role = await _iRoleManager.GetRoleById(id);
            _logger.Information($"{GetActionWithControllerName()} ==> {WebUtility.HtmlEncode(JsonSerializer.Serialize(role))}");
            if (role == null)
            {
                return null;
            }
            return View(role);

        }

        //
        // POST: /MonitoringRole/Delete/5
        /// <summary>
        /// action to delete a role by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [TypeFilter(typeof(CheckSessionWithRedirect))]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {

            Role role = await _iRoleManager.GetRoleById(id);
            _logger.Information($"{GetActionWithControllerName()} ==> {WebUtility.HtmlEncode(JsonSerializer.Serialize(role))}");
            int result = await _iRoleManager.DeleteRole(role);
            return RedirectToAction("Index");

        }
        /// <summary>
        /// gets current action and controller name
        /// </summary>
        /// <returns></returns>
        private string GetActionWithControllerName()
        {
            string result = string.Empty;
            try
            {
                string actionName = ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                result = controllerName + "/" + actionName;
                _logger.Information("URL Generation ==> " + WebUtility.HtmlEncode(result));
            }
            catch (Exception ex)
            {
                _logger.Error($"{WebUtility.HtmlEncode(ex.ToString())}");
                result = string.Empty;
            }
            return WebUtility.HtmlEncode(result);
        }
    }
}
