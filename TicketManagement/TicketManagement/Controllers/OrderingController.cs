using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketManagement.Filters;
using TicketManagement.Interface;
using TicketManagement.ViewModels;

namespace TicketManagement.Controllers
{
    [AuthorizeSuperAdmin]
    public class OrderingController : Controller
    {
        private readonly IMenu _menu;
        private readonly IRole _role;
        private readonly ISubMenu _subMenu;
        private IMenuCategory _menuCategory;

        public OrderingController(IMenu menu, IRole role, ISubMenu subMenu, IMenuCategory menuCategory)
        {
            _menu = menu;
            _role = role;
            _subMenu = subMenu;
            _menuCategory = menuCategory;
        }


        // GET: OrderingMainMenu
        public ActionResult MenuCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MenuCategory(RequestMenuCategoryOrderVm request)
        {
            int preference = 1;
            var listofStoringOrders = new List<MenuCategoryStoringOrder>();
            if (request.SelectedOrder != null)
            {
                foreach (int menuId in request.SelectedOrder)
                {
                    listofStoringOrders.Add(new MenuCategoryStoringOrder()
                    {
                        RoleId = request.RoleId,
                        SortingOrder = preference,
                        MenuCategoryId = menuId
                    });
                    preference += 1;
                }
            }

            var result = _menuCategory.UpdateMenuCategoryOrder(listofStoringOrders);

            return View();
        }

        public ActionResult MainMenu()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MainMenu(RequestMenuMasterOrderVm request)
        {
            int preference = 1;
            var listofStoringOrders = new List<MenuStoringOrder>();
            if (request.SelectedOrder != null)
            {
                foreach (int menuId in request.SelectedOrder)
                {
                    listofStoringOrders.Add(new MenuStoringOrder()
                    {
                        RoleId = request.RoleId,
                        MenuId = menuId,
                        SortingOrder = preference
                    });
                    preference += 1;
                }
            }

            var result = _menu.UpdateMenuOrder(listofStoringOrders);

            return View();
        }

        public JsonResult GetAllRoles()
        {
            return Json(_role.GetAllActiveRoles(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllMenuCategorybyRoleId(int roleId)
        {
            return Json(_menu.ListofMenubyRoleCategoryId(roleId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCategorybyRoleId(int roleId)
        {
            return Json(_menuCategory.GetCategorybyRoleId(roleId), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAllMenubyRoleId(RequestMenu requestMenu)
        {
            return Json(_menu.GetListofMenu(requestMenu.RoleId, requestMenu.MenuCategoryId), JsonRequestBehavior.AllowGet);
        }

        
        public JsonResult GetAllMenubyRoleIdSelectListItem(int roleId, int menuCategoryId)
        {
            return Json(_menu.ListofMenubyRoleIdSelectListItem(roleId, menuCategoryId), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAllSubMenubyRoleId(RequestSubMenu requestSubMenu)
        {
            return Json(_subMenu.ListofSubMenubyRoleId(requestSubMenu.RoleId, requestSubMenu.MenuId), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SubMenu()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SubMenu(RequestSubMenuMasterOrderVm request)
        {
            int preference = 1;
            var listofStoringOrders = new List<SubMenuStoringOrder>();
            if (request.SelectedOrder != null)
            {
                foreach (int subMenuId in request.SelectedOrder)
                {
                    listofStoringOrders.Add(new SubMenuStoringOrder()
                    {
                        RoleId = request.RoleId,
                        MenuId = request.MenuId,
                        SortingOrder = preference,
                        SubMenuId = subMenuId
                    });
                    preference += 1;
                }
            }

            var result = _subMenu.UpdateSubMenuOrder(listofStoringOrders);

            return View();
        }


    }
}