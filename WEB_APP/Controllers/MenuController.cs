
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.ViewModels.UserPermissionViewModels;
using System.Net;
using System.Text.Json;
using DATA.Models;
using SERVICE.Interface;
using WEB_APP.Extensions;

namespace WEB_APP.Controllers
{
    /// <summary>
    /// used for CRUD operation of MonitoringMenu
    /// </summary>
    public class MenuController : Controller
    {
        private readonly IMenuManager _menuManager;
        private readonly Serilog.ILogger _logger;
        public MenuController(IMenuManager MenuManager, Serilog.ILogger logger)
        {
            _menuManager = MenuManager;
            _logger = logger;
        }
        /// <summary>
        /// Action for loading all menus
        /// </summary>
        /// <returns></returns>
        [TypeFilter(typeof(CheckSessionWithRedirect), Order = 1)]
        [TypeFilter(typeof(CheckPermission), Order = 2)]
        public async Task<ActionResult> Index()
        {
            var result = new List<MenuViewModel>();
            var data = await _menuManager.GetMenuList();
            if (data != null)
            {                
                foreach (var item in data)
                {
                    string parent_menu_name = string.Empty;
                    if(item.ParentMenu !=null && item.ParentMenu != 0)
                    {
                        var parent_menu_item = await _menuManager.GetMenuById((int)item.ParentMenu);
                        if (parent_menu_item != null)
                        {
                            parent_menu_name = parent_menu_item.MenuName??string.Empty;
                        }
                    }
                    result.Add(new MenuViewModel
                    {
                        CreatedBy = item.CreatedBy,
                        CreatedDate = item.CreatedDate,
                        Id = item.Id,
                        IsActive = item.IsActive,
                        MenuName = item.MenuName,
                        MenuOrder = item.MenuOrder,
                        ParentMenu = item.ParentMenu,
                        UpdatedBy = item.UpdatedBy,
                        UpdatedDate = item.UpdatedDate,
                        Url = item.Url,
                        ParentMenuName = parent_menu_name,
                    });
                }
            }
            _logger.Information($"{GetActionWithControllerName()} ==> {WebUtility.HtmlEncode(JsonSerializer.Serialize(data))}");
            return View(result);

        }
        /// <summary>
        /// Gets the current action and controller name
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
        /// action to get the information of a menu by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //
        // GET: /MonitoringMenu/Details/5
        [TypeFilter(typeof(CheckSessionWithRedirect))]
        public async Task<ActionResult> Details(int id = 0)
        {

            Menu menu = await _menuManager.GetMenuById(id);
            if (menu == null)
            {
                return null;
            }
            Menu parentMenu = await _menuManager.GetMenuByParentId(Convert.ToInt32(menu.ParentMenu));

            Data d = new Data();
            d.menu = menu;
            if (parentMenu != null)
            {
                d.parentMenu = parentMenu.Url;
            }
            else
            {
                d.parentMenu = "Top MonitoringMenu";
            }
            _logger.Information($"{GetActionWithControllerName()} ==> {WebUtility.HtmlEncode(JsonSerializer.Serialize(d))}");
            return View(d);

        }

        //
        // GET: /MonitoringMenu/Create

        //List<DisplayTable> list = DBContext.GetMenuList();
        /// <summary>
        /// loads the menu creation page
        /// </summary>
        /// <returns></returns>
        [TypeFilter(typeof(CheckSessionWithRedirect))]
        public async Task<ActionResult> Create()
        {

            var list = await _menuManager.GetMenuDisplayTableList();
            var menu = new CommonMenus();
            menu.DropDownList = new SelectList(list, "Key", "Display");
            _logger.Information($"{GetActionWithControllerName()} ==> {WebUtility.HtmlEncode(JsonSerializer.Serialize(menu))}");
            return View(menu);

        }

        //
        // POST: /MonitoringMenu/Create
        /// <summary>
        /// saves menu from the menu creation page
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        [HttpPost]
        [TypeFilter(typeof(CheckSessionWithRedirect))]
        public async Task<ActionResult> Create(CommonMenus commonMenus)
        {
            Menu menu = new()
            {
                MenuName = commonMenus.MenuName,
                IsActive = commonMenus.IsActive,
                MenuOrder = commonMenus.MenuOrder,
                ParentMenu = commonMenus.ParentMenu,
                Url = commonMenus.Url
            };
            if (menu.ParentMenu == null)
            {
                menu.ParentMenu = 0;
            }

            menu.IsActive = true;
            try
            {
                int result = await _menuManager.CreateMenu(menu);
                TempData["success"] = "MonitoringMenu created successfully";
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                TempData["failed"] = "MonitoringMenu registration failed";
                _logger.Information($"{GetActionWithControllerName()} ==> {WebUtility.HtmlEncode(JsonSerializer.Serialize(menu))}");
                return View(menu);
            }            
        }

        //
        // GET: /MonitoringMenu/Edit/5
        /// <summary>
        /// gets menu information for editing by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [TypeFilter(typeof(CheckSessionWithRedirect))]
        public async Task<ActionResult> Edit(int id = 0)
        {

            Menu menu = await _menuManager.GetMenuById(id);

            if (menu == null)
            {
                return null;
            }

            CommonMenus commonMenus = new CommonMenus();
            commonMenus.Id = menu.Id;
            commonMenus.ParentMenu = menu.ParentMenu;
            commonMenus.Url = menu.Url??String.Empty;
            commonMenus.IsActive = menu.IsActive;
            commonMenus.MenuName = menu.MenuName??string.Empty;
            commonMenus.MenuOrder = menu.MenuOrder;
            commonMenus.CreatedBy = menu.CreatedBy;
            commonMenus.CreatedDate = menu.CreatedDate;
            commonMenus.UpdatedBy = menu.UpdatedBy;
            commonMenus.UpdatedDate = menu.UpdatedDate;
            var list1 = new List<SelectListItem>
                {
                    new SelectListItem{ Text="Active", Value = "1" },
                    new SelectListItem{ Text="InActive", Value ="0" },

                };

            ViewData["IsActive"] = list1;
            var list = await _menuManager.GetMenuDisplayTableList();
            commonMenus.DropDownList = new SelectList(list, "Key", "Display");
            _logger.Information($"{GetActionWithControllerName()} ==> {WebUtility.HtmlEncode(JsonSerializer.Serialize(commonMenus))}");
            return View(commonMenus);

        }

        //
        // POST: /MonitoringMenu/Edit/5
        /// <summary>
        /// updates a menu by model
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        [HttpPost]
        [TypeFilter(typeof(CheckSessionWithRedirect))]
        public async Task<ActionResult> Edit(CommonMenus commonMenus)
        {
            Menu menu = new()
            {
                MenuName = commonMenus.MenuName,
                IsActive = commonMenus.IsActive,
                MenuOrder = commonMenus.MenuOrder,
                ParentMenu = commonMenus.ParentMenu,
                Url = commonMenus.Url,
                Id = commonMenus.Id,
                CreatedBy = commonMenus.CreatedBy,
                CreatedDate = commonMenus.CreatedDate
            };
            if (menu.ParentMenu == null)
            {
                menu.ParentMenu = 0;
            }

            int result = await _menuManager.EditMenu(menu);
            if (result > 0)
            {
                TempData["success"] = "MonitoringMenu updated successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["failed"] = "MonitoringMenu update failed";
            }
            _logger.Information($"{GetActionWithControllerName()} ==> {WebUtility.HtmlEncode(JsonSerializer.Serialize(menu))}");
            return View(commonMenus);

        }

        //
        // GET: /MonitoringMenu/Delete/5
        /// <summary>
        /// gets menu information for deleting
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [TypeFilter(typeof(CheckSessionWithRedirect))]
        public async Task<ActionResult> Delete(int id = 0)
        {

            Menu menus = await _menuManager.GetMenuById(id);
            int result = await _menuManager.DeleteMenu(menus);
            _logger.Information($"{GetActionWithControllerName()} ==> {WebUtility.HtmlEncode(JsonSerializer.Serialize(menus))}");
            TempData["success"] = "MonitoringMenu Delete Successfully!";
            return RedirectToAction("Index");


        }

        //
        // POST: /MonitoringMenu/Delete/5
        /// <summary>
        /// deletes a menu by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [TypeFilter(typeof(CheckSessionWithRedirect))]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {

            Menu menus = await _menuManager.GetMenuById(id);
            _logger.Information($"{GetActionWithControllerName()} ==> {WebUtility.HtmlEncode(JsonSerializer.Serialize(menus))}");
            int result = await _menuManager.DeleteMenu(menus);
            return RedirectToAction("Index");

        }

        //protected override void Dispose(bool disposing)
        //{
        //    db.Dispose();
        //    base.Dispose(disposing);
        //}
    }
}
