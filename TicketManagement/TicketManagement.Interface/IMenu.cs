using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Interface
{
    public interface IMenu
    {
        List<MenuMaster> GetAllMenu();
        List<SelectListItem> GetAllActiveMenu();
        EditMenuMasterViewModel GetMenuById(int? menuId);
        MenuViewModel GetMenutoDeleteById(int? menuId);
        int? AddMenu(MenuMaster menuMaster);
        int? UpdateMenu(MenuMaster menuMaster);
        void DeleteMenu(int? menuId);
        bool CheckMenuNameExists(string menuName);
        List<MenuCategoryOrderingVm> ListofMenubyRoleCategoryId(int roleId);
        IQueryable<MenuViewModel> ShowAllMenus(string sortColumn, string sortColumnDir, string search);
        List<MenuMasterCacheViewModel> GetAllActiveMenu(long roleId);
        List<MenuMaster> GetAllActiveMenuSuperAdmin();
        List<MenuMasterOrderingVm> GetListofMenu(int roleId, int menuCategoryId);
        List<SelectListItem> ListofMenubyRoleIdSelectListItem(int roleId, int menuCategoryId);
        bool UpdateMenuOrder(List<MenuStoringOrder> menuStoringOrder);
        List<SelectListItem> GetAllAssignedMenu();
        
        List<SelectListItem> GetAllAssignedMenuWithRoles();
        bool EditValidationCheck(int? menuId, EditMenuMasterViewModel editMenu);
        bool CheckMenuNameExists(string menuName, int? roleId, int? categoryId);
        List<SelectListItem> ListofMenubyRoleId(RequestMenus requestMenus);
    }
}