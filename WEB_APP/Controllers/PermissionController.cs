
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CORPORATE_DISBURSEMENT_ADMIN.Extensions;
using CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.ViewModels.UserPermissionViewModels;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using ILogger = Serilog.ILogger;
using DATA.Models;
using SERVICE.Interface;

namespace WEB_APP.Controllers
{
    /// <summary>
    /// used for CRUD Operation of permission
    /// </summary>
    public class PermissionController : Controller
    {
        //desco_app_db_admin_dbcontext db = new desco_app_db_admin_dbcontext();
        private readonly IPermissionManager _iPermissionManager;
        private readonly ILogger _logger;
        private readonly IMenuManager _menuManager;
        public PermissionController(IPermissionManager iPermissionManager, ILogger logger, IMenuManager menuManager)
        {
            _iPermissionManager = iPermissionManager;
            _logger = logger;
            _menuManager = menuManager;
        }
        /// <summary>
        /// action to get all menu permissions
        /// </summary>
        /// <returns></returns>
        [TypeFilter(typeof(CheckSessionWithRedirect), Order = 1)]
        [TypeFilter(typeof(CheckPermission), Order = 2)]
        public async Task<ActionResult> Index()
        {

            var permissions = await _iPermissionManager.GetAllPermissions();
            _logger.Information($"{GetActionWithControllerName()} ==> {WebUtility.HtmlEncode(JsonSerializer.Serialize(permissions, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve }))}");
            return View(permissions);

        }
        /// <summary>
        /// gets current action and controller name
        /// </summary>
        /// <returns></returns>
        private object GetActionWithControllerName()
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
        /// <summary>
        /// get the detailed info of a menu permission by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [TypeFilter(typeof(CheckSessionWithRedirect))]
        public async Task<ActionResult> Details(int id = 0)
        {

            Permission permission = await _iPermissionManager.GetPermissionById(id);
            _logger.Information($"{GetActionWithControllerName()} ==> {WebUtility.HtmlEncode(JsonSerializer.Serialize(permission, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve }))}");
            if (permission == null)
            {
                return null;
            }
            return View(permission);

        }

        //List<DisplayTable> list1 = DBContext.GetMenuList();
        /// <summary>
        /// action to load the menu permission creation page
        /// </summary>
        /// <returns></returns>
        [TypeFilter(typeof(CheckSessionWithRedirect))]
        public async Task<ActionResult> Create()
        {

            var permission = new ComRolePermissionscs();
            var list = await _menuManager.GetMenuDisplayTableList();
            permission.DropDownList = new SelectList(list, "Key", "Display");

            // ViewBag.MENU_ID = new SelectList(db.Menus, "ID", "URL");
            return View(permission);

        }

        //
        // POST: /MonitoringPermission/Create
        /// <summary>
        /// action to create a new menu permission
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        [HttpPost]
        [TypeFilter(typeof(CheckSessionWithRedirect))]
        public async Task<ActionResult> Create(Permission permission)
        {

            _logger.Information($"{GetActionWithControllerName()} ==> {WebUtility.HtmlEncode(JsonSerializer.Serialize(permission))}");
            if (ModelState.IsValid)
            {
                permission.IsActive = true;
                int result = await _iPermissionManager.AddPermission(permission);
                TempData["success"] = "MonitoringPermission created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["failed"] = "MonitoringPermission registration failed";
            }
            var menu_list = await _menuManager.GetMenuList();
            ViewBag.MENU_ID = new SelectList(menu_list, "ID", "URL", permission.MenuId);
            return View(permission);

        }
        /// <summary>
        /// action to load the information of a menu permission for editing
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [TypeFilter(typeof(CheckSessionWithRedirect))]
        public async Task<ActionResult> Edit(int id = 0)
        {

            Permission permission = await _iPermissionManager.GetPermissionById(id);
            _logger.Information($"{GetActionWithControllerName()} ==> {WebUtility.HtmlEncode(JsonSerializer.Serialize(permission))}");
            if (permission == null)
            {
                return null;
            }


            ComRolePermissionscs comRole = new ComRolePermissionscs();
            comRole.ID = permission.Id;
            comRole.NAME = permission.Name??string.Empty;
            comRole.MENU_ID = permission.MenuId;
            comRole.IS_ACTIVE = permission.IsActive;
            var list = await _menuManager.GetMenuDisplayTableList();
            comRole.DropDownList = new SelectList(list, "Key", "Display");
            _logger.Information($"{GetActionWithControllerName()} ==> {WebUtility.HtmlEncode(JsonSerializer.Serialize(comRole))}");

            return View(comRole);

        }

        //
        // POST: /MonitoringPermission/Edit/5
        /// <summary>
        /// action to update the information of a menu permission
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        [HttpPost]
        [TypeFilter(typeof(CheckSessionWithRedirect))]
        public async Task<ActionResult> Edit(Permission permission)
        {

            _logger.Information($"{GetActionWithControllerName()} ==> {WebUtility.HtmlEncode(JsonSerializer.Serialize(permission, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve }))}");
            if (ModelState.IsValid)
            {
                permission.IsActive = true;
                int result = await _iPermissionManager.UpdatePermission(permission);
                TempData["success"] = "MonitoringPermission updated successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["failed"] = "MonitoringPermission update failed";
            }
            var menu_list = await _menuManager.GetMenuList();
            ViewBag.MenuId = new SelectList(menu_list, "ID", "URL", permission.MenuId);
            return View(permission);

        }
        /// <summary>
        /// action to delete a menu permission
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [TypeFilter(typeof(CheckSessionWithRedirect))]
        public async Task<ActionResult> Delete(int id = 0)
        {

            Permission permission = await _iPermissionManager.GetPermissionById(id);
            _logger.Information($"{GetActionWithControllerName()} ==> {WebUtility.HtmlEncode(JsonSerializer.Serialize(permission, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve }))}");
            int result = await _iPermissionManager.DeletePermission(permission);
            TempData["success"] = "MonitoringPermission Delete Successfully!";
            return RedirectToAction("Index");


        }
    }
}
