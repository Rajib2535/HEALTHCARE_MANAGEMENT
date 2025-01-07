
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using ILogger = Serilog.ILogger;
using DATA.Interface;
using DATA.Models;
using SERVICE.Interface;
using WEB_APP.Extensions;
using DATA.Models.ViewModels.UserPermissionViewModels;

namespace WEB_APP.Controllers
{
    //used for CRUD operation of Role Permission of User
    public class RolePermissionController : Controller
    {
        //desco_app_db_admin_dbcontext db = new desco_app_db_admin_dbcontext();
        private readonly IRoleManager _iRoleManager;
        private readonly IRolePermissionManager _iRolePermissionManager;
        private readonly ILogger _logger;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        public RolePermissionController(IRoleManager iRoleManager, IRolePermissionManager iRolePermissionManager, ILogger logger,IRolePermissionRepository rolePermissionRepository)
        {
            _iRoleManager = iRoleManager;
            _iRolePermissionManager = iRolePermissionManager;
            _logger = logger;
            _rolePermissionRepository = rolePermissionRepository;
        }
        /// <summary>
        /// loads the role permission list page
        /// </summary>
        /// <returns></returns>
        [TypeFilter(typeof(CheckSessionWithRedirect), Order = 1)]
        [TypeFilter(typeof(CheckPermission), Order = 2)]
        public async Task<ActionResult> Index()
        {

            var role_permission = await _iRolePermissionManager.GetRolePermissionList();
            _logger.Information($"{GetActionWithControllerName()} ==> {WebUtility.HtmlEncode(JsonSerializer.Serialize(role_permission, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve }))}");
            return View(role_permission);

        }
        /// <summary>
        /// loads the details page of role permission by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [TypeFilter(typeof(CheckSessionWithRedirect))]
        public async Task<ActionResult> Details(int id = 0)
        {

            RolePermission role_permission = await _iRolePermissionManager.GetRolePermissionById(id);
            _logger.Information($"{GetActionWithControllerName()} ==> {WebUtility.HtmlEncode(JsonSerializer.Serialize(role_permission, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve }))}");
            if (role_permission == null)
            {
                return null;
            }
            return View(role_permission);

        }

        //List<DisplayTable> listPermission = DBContext.GetPermissionList();
        //List<DisplayTable> listRole = DBContext.GetRoleList();
        /// <summary>
        /// loads the role permission creation page
        /// </summary>
        /// <returns></returns>
        [TypeFilter(typeof(CheckSessionWithRedirect))]
        public async Task<ActionResult> Create()
        {

            var rolePermission = new Comapiroleperm();
            var listRole = await _iRoleManager.GetDisplayTableRoleList();
            rolePermission.DropDownListForPermission = await _rolePermissionRepository.GetpermissionSelectList(1);
            rolePermission.DropDownListForRole = new SelectList(listRole, "Key", "Display");
            //ViewBag.PERMISSION_ID = new SelectList(db.Permissions, "ID", "NAME");
            //ViewBag.ROLE_ID = new SelectList(db.Roles, "ID", "NAME");
            return View(rolePermission);

        }

        /// <summary>
        /// gets the search result list page of role permission by role
        /// </summary>
        /// <param name="role_permission"></param>
        /// <returns></returns>
        [HttpPost]
        [TypeFilter(typeof(CheckSessionWithRedirect))]
        public async Task<ActionResult> SearchResult(Comapiroleperm role_permission)
        {

            _logger.Information($"{GetActionWithControllerName()} ==> {WebUtility.HtmlEncode(JsonSerializer.Serialize(role_permission, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve }))}");
            DataObjectsForRolePermission data = new DataObjectsForRolePermission();
            if (role_permission.RoleId != null)
            {
                var rolePermission = new Comapiroleperm();
                int roleId = (int)role_permission.RoleId;
                var listRole = await _iRoleManager.GetDisplayTableRoleList();
                rolePermission.DropDownListForPermission = await _rolePermissionRepository.GetpermissionSelectList(roleId);
                rolePermission.DropDownListForRole = new SelectList(listRole, "Key", "Display");

                //Role role = db.Roles.Find(role_permission.RoleId);
                Role role = await _iRoleManager.GetRoleById(Convert.ToInt32(role_permission.RoleId));

                data.rolePermission = rolePermission;
                data.role = role;

                return View(data);
            }

            return RedirectToAction("Create");

        }
        /// <summary>
        /// action to create a new role permission
        /// </summary>
        /// <param name="rolePermission"></param>
        /// <returns></returns>
        [HttpPost]
        [TypeFilter(typeof(CheckSessionWithRedirect))]
        public async Task<ActionResult> Create(Comapiroleperm rolePermission)
        {

            _logger.Information($"{GetActionWithControllerName()} ==> {WebUtility.HtmlEncode(JsonSerializer.Serialize(rolePermission, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve }))}");
            //if (data.rolePermission != null && data.role != null){
            if (rolePermission.SelectedValues != null && rolePermission.RoleId != null)
            {
                //if (ModelState.IsValid)
                //{
                var role_permissions = await _iRolePermissionManager.GetRolePermissionListByRoleId(Convert.ToInt32(rolePermission.RoleId));

                foreach (RolePermission role_permission1 in role_permissions)
                {
                    await _iRolePermissionManager.DeleteRolePermission(role_permission1);
                }


                foreach (int permissionId in rolePermission.SelectedValues)
                {
                    RolePermission permission = new RolePermission();
                    permission.Id = rolePermission.Id;
                    permission.RoleId = (int)rolePermission.RoleId;
                    permission.PermissionId = permissionId;

                    rolePermission.PermissionId = permissionId;
                    int result = await _iRolePermissionManager.AddRolePermission(permission);
                    TempData["success"] = "Permission Assigned successfully";

                }
                return RedirectToAction("Index");
                //}
                //else
                //{
                //    TempData["failed"] = "Permission Assign failed";
                //}
            }
            //}
            ViewBag.PermissionId = new SelectList(await _iRolePermissionManager.GetRolePermissionList(), "ID", "NAME", rolePermission.PermissionId);
            ViewBag.RoleId = new SelectList(await _iRoleManager.GetAllRoles(), "ID", "NAME", rolePermission.RoleId);
            return View(rolePermission);

        }
        /// <summary>
        /// get current action and controller name
        /// </summary>
        /// <returns></returns>
        private string GetActionWithControllerName()
        {
            string result;
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
        protected override void Dispose(bool disposing)
        {
            //db.Dispose();
            base.Dispose(disposing);
        }

    }
}
