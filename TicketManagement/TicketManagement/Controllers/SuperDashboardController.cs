using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketManagement.Filters;
using TicketManagement.Helpers;
using TicketManagement.Interface;


namespace TicketManagement.Controllers
{
    [AuthorizeSuperAdminAttribute]
    public class SuperDashboardController : Controller
    {
        private readonly IMenu _iMenu;
        private readonly ISubMenu _iSubMenu;
        private readonly IMenuCategory _menuCategory;
        SessionHandler _sessionHandler = new SessionHandler();
        public SuperDashboardController(IMenu menu, ISubMenu subMenu, IMenuCategory menuCategory)
        {
            _iMenu = menu;
            _iSubMenu = subMenu;
            _menuCategory = menuCategory;
        }

        // GET: SuperDashboard

        public ActionResult ShowMenus()
        {
            try
            {
                var menuList = _menuCategory.ShowCategories(Convert.ToInt32(_sessionHandler.RoleId));
                return PartialView("ShowMenuSuperAdmin", menuList);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult Dashboard()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }
        

    }
}