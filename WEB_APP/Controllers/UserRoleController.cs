using CORPORATE_DISBURSEMENT_ADMIN.Extensions;
using CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.ViewModels.UserPermissionViewModels;
using DATA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SERVICE.Interface;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using ILogger = Serilog.ILogger;

namespace WEB_APP.Controllers
{
    /// <summary>
    /// used for CRUD operation of User Role
    /// </summary>
    public class UserRoleController : Controller
    {
        private readonly IUserManager _UserManager;
        private readonly IUserRoleManager _UserRoleManager;
        private readonly IRoleManager _RoleManager;
        private readonly ILogger _logger;
        public UserRoleController(IUserManager UserManager, IUserRoleManager UserRoleManager, IRoleManager RoleManager, ILogger logger)
        {
            _UserManager = UserManager;
            _UserRoleManager = UserRoleManager;
            _RoleManager = RoleManager;
            _logger = logger;
        }
        /// <summary>
        /// action to load all user roles page
        /// </summary>
        /// <returns></returns>
        [TypeFilter(typeof(CheckSessionWithRedirect), Order = 1)]
        [TypeFilter(typeof(CheckPermission), Order = 2)]
        public async Task<ActionResult> Index()
        {

            //var user_role = db.UserRole.ToList();
            var user_role = await _UserRoleManager.GetAllUserRoles();
            _logger.Information($"{GetActionWithControllerName()} ==> {WebUtility.HtmlEncode(JsonSerializer.Serialize(user_role, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve }))}");
            return View(user_role);

        }
        /// <summary>
        /// method to get current controller and action name
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
                _logger.Information("URL Generation ==> " + result);
            }
            catch (Exception ex)
            {
                _logger.Error($"{WebUtility.HtmlEncode(ex.ToString())}");
                result = string.Empty;
            }
            return WebUtility.HtmlEncode(result);
        }
        /// <summary>
        /// action to load user role information page by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //
        // GET: /UserRole/Details/5
        [TypeFilter(typeof(CheckSessionWithRedirect))]
        public async Task<ActionResult> Details(int id = 0)
        {

            UserRole user_role = await _UserRoleManager.GetUserRoleById(id);
            if (user_role == null)
            {
                return null;
            }
            return View(user_role);

        }

        //
        // GET: /UserRole/Create
        //List<DisplayTable> listUser = DBContext.GetUserList();
        //List<DisplayTable> listRole = DBContext.GetRoleList();


        [TypeFilter(typeof(CheckSessionWithRedirect))]
        public async Task<ActionResult> Create()
        {

            var userRole = new DummyUserRole();
            var listUser = await _UserManager.GetDisplayTableUserList();
            var listRole = await _RoleManager.GetDisplayTableRoleList();
            userRole.DropDownListForUser = new SelectList(listUser, "Key", "Display");
            userRole.DropDownListForRole = new SelectList(listRole, "Key", "Display");
            //ViewBag.ROLE_ID = new SelectList(db.Roles, "ID", "NAME");
            //ViewBag.USER_ID = new SelectList(db.User_Info, "ID", "USERID");
            return View(userRole);

        }

        //
        // POST: /UserRole/Create

        [HttpPost]
        [TypeFilter(typeof(CheckSessionWithRedirect))]
        public async Task<ActionResult> Create(DummyUserRole user_role)
        {

            if (user_role.USER_ID != null && user_role.ROLE_ID != null)
            {



                //int userId = await _UserManager.SaveUserInfoByDummyUserRole(user_role);


                if (ModelState.IsValid)
                {
                    var userList = await _UserManager.GetUserInfos();
                    foreach (var user in userList)
                    {
                        var userRoles = await _UserRoleManager.GetUserRoleByUserId(user.Id);
                        foreach (UserRole userRole1 in userRoles)
                        {
                            if (userRole1.UserId == user_role.USER_ID)
                            {
                                await _UserRoleManager.DeleteUserRole(userRole1);
                            }
                        }
                    }

                    UserRole userRole = new();
                    userRole.UserId = (int)user_role.USER_ID;
                    userRole.RoleId = (int)user_role.ROLE_ID;
                    int result = await _UserRoleManager.AddUserRole(userRole);
                    TempData["success"] = "Role Assigned successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["failed"] = "Role Assign failed";
                }
            }

            var userRole2 = new UserRole();
            //userRole2.DropDownListForUser = new SelectList(listUser, "Key", "Display");
            //userRole2.DropDownListForRole = new SelectList(listRole, "Key", "Display");
            return View(userRole2);

        }


        [TypeFilter(typeof(CheckSessionWithRedirect))]
        public async Task<ActionResult> Edit(int id = 0)
        {

            UserRole user_role = await _UserRoleManager.GetUserRoleById(id);
            if (user_role == null)
            {
                return null;
            }
            AssignRole assignRole = new AssignRole();
            assignRole.RoleId = user_role.RoleId;
            assignRole.UserId = user_role.UserId;
            var listUser = await _UserManager.GetDisplayTableUserList();
            var listRole = await _RoleManager.GetDisplayTableRoleList();
            assignRole.DropDownListForUser = new SelectList(listUser, "Key", "Display");
            assignRole.DropDownListForRole = new SelectList(listRole, "Key", "Display");

            ViewBag.RoleId = new SelectList((await _RoleManager.GetAllRoles()).AsEnumerable(), "ID", "NAME", assignRole.RoleId);
            ViewBag.UserId = new SelectList((await _UserManager.GetUserInfos()).AsEnumerable(), "ID", "NAME", assignRole.UserId);

            return View(assignRole);

        }

        //
        // POST: /UserRole/Edit/5

        [HttpPost]
        [TypeFilter(typeof(CheckSessionWithRedirect))]
        public async Task<ActionResult> Edit(UserRoleViewModel user_role)
        {

            if (user_role.UserId != null && user_role.RoleId != null)
            {
                if (ModelState.IsValid)
                {
                    await _UserRoleManager.UpdateUserRole(user_role);
                    //db.Entry(user_role).State = EntityState.Modified;
                    //db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            AssignRole assignRole = new AssignRole();
            assignRole.RoleId = user_role.RoleId;
            assignRole.UserId = user_role.UserId;
            var listUser = await _UserManager.GetDisplayTableUserList();
            var listRole = await _RoleManager.GetDisplayTableRoleList();
            assignRole.DropDownListForUser = new SelectList(listUser, "Key", "Display");
            assignRole.DropDownListForRole = new SelectList(listRole, "Key", "Display");
            ViewBag.RoleId = new SelectList((await _RoleManager.GetAllRoles()).AsEnumerable(), "ID", "NAME", user_role.RoleId);
            ViewBag.UserId = new SelectList((await _UserManager.GetUserInfos()).AsEnumerable(), "ID", "NAME", user_role.UserId);
            return View(assignRole);

        }

        //
        // GET: /UserRole/Delete/5
        [TypeFilter(typeof(CheckSessionWithRedirect))]
        public async Task<ActionResult> Delete(int id = 0)
        {
            //if (!DBContext.hasPermission("User Role Management"))
            //{
            //    _logger.Error($"{GetActionWithControllerName()} ==> permission denied");
            //    return RedirectToAction("Action", "Error");
            //}
            //else
            //{
            UserRole user_role = await _UserRoleManager.GetUserRoleById(id);
            int result = await _UserRoleManager.DeleteUserRole(user_role);
            TempData["success"] = "Role Delete Successfully!";
            return RedirectToAction("Index");
            //}

        }

        //
        // POST: /UserRole/Delete/5

        [HttpPost, ActionName("Delete")]
        [TypeFilter(typeof(CheckSessionWithRedirect))]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            //if (!DBContext.hasPermission("User Role Management"))
            //{
            //    _logger.Error($"{GetActionWithControllerName()} ==> permission denied");
            //    return RedirectToAction("Action", "Error");
            //}
            //else
            //{
            UserRole user_role = await _UserRoleManager.GetUserRoleById(id);
            int result = await _UserRoleManager.DeleteUserRole(user_role);
            TempData["success"] = "Role Delete Successfully!";
            return RedirectToAction("Index");
            //}
        }


    }
}
