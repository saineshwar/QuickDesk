using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Interface
{
    public interface ICategory
    {
        List<SelectListItem> GetAllActiveSelectListItemCategory();
        Category GetCategoryById(int? categoryId);
        int? AddCategory(Category category);
        int? UpdateCategory(Category category);
        int? DeleteCategory(int? categoryId);
        bool CheckCategoryNameExists(string categoryName);
        List<CategoryViewModel> GridGetCategory(string categoryName, int startIndex, int count, string sorting);
        int GetCategoryCount(string categoryName);
        string GetCategoryCodeByCategoryId(int? categoryId);
        List<SelectListItem> GetAllActiveCategoryforListbox();
        short GetCategoryIdsByUserId(long? userId);
        int? AddCategoryConfigration(CategoryConfigration category);
        CategoryConfigration GetCategoryConfigrationDetails(int? categoryConfigrationId);
        int GetCategoryConfigrationCount(string userName);
        List<ShowCategoryConfigration> GridGetCategoryConfigration(string userName, int startIndex,
            int count, string sorting);
        int? UpdateCategoryConfigration(CategoryConfigration category);
        bool CheckDuplicateCategoryConfigration(long adminuserId, long hoduserId, int categoryId);
        int? DeleteCategoryConfigration(int? categoryConfigrationId);
        int GetAdminCategory(long userId);
        int GetHodCategory(long userId);
    }
}
