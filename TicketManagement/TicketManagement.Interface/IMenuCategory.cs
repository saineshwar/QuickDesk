using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Interface
{
    public interface IMenuCategory
    {
        List<SelectListItem> GetAllActiveSelectListItemCategory();
        EditCategoriesVM GetCategoryById(int? menuCategoryId);
        int? AddCategory(MenuCategory category);
        int? UpdateCategory(MenuCategory category);
        int? DeleteCategory(int? categoryId);
        bool CheckCategoryNameExists(string menuCategoryName, int roleId);
        int GetCategoryCount(string menuCategoryName);
        IQueryable<MenuCategoryViewModel> ShowAllMenusCategory(string sortColumn, string sortColumnDir, string search);
        List<SelectListItem> GetCategorybyRoleId(int? roleId);
        List<MenuCategoryCacheViewModel> ShowCategories(int roleId);
        bool UpdateMenuCategoryOrder(List<MenuCategoryStoringOrder> menuStoringOrder);
    }
}