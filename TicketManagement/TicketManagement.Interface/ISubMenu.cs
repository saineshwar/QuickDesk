using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Interface
{
    public interface ISubMenu 
    {
        IEnumerable<SubMenuMaster> GetAllSubMenu();
        EditSubMenuMaster GetSubMenuById(int? subMenuId);
        int? AddSubMenu(SubMenuMaster subMenuMaster);
        int? UpdateSubMenu(SubMenuMaster subMenuMaster);
        int DeleteSubMenu(int? subMenuId);
        bool CheckSubMenuNameExists(string subMenuName, int menuId);
        bool CheckSubMenuNameExists(string subMenuName, int? menuId, int? roleId, int? categoryId);
        IQueryable<SubMenuMasterViewModel> ShowAllSubMenus(string sortColumn, string sortColumnDir, string search);
        List<SelectListItem> GetAllActiveSubMenu(int menuid);
        List<SubMenuMaster> GetAllActiveSubMenuByMenuId(int menuid);
        List<SubMenuMasterOrderingVm> ListofSubMenubyRoleId(int roleId, int menuid);
        bool UpdateSubMenuOrder(List<SubMenuStoringOrder> submenuStoringOrder);
        List<SelectListItem> GetAllActiveSubMenuWithoutDefault(int menuid);
        bool EditValidationCheck(int? subMenuId, EditSubMenuMaster editsubMenu);
    }
}