using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using Dapper;
using TicketManagement.Concrete.CacheLibrary;
using TicketManagement.Interface;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Concrete
{
    public class MenuCategoryConcrete : IMenuCategory
    {
        private readonly DatabaseContext _context;
        public MenuCategoryConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public int? AddCategory(MenuCategory category)
        {
            int? result = -1;

            if (category != null)
            {
                category.CreatedOn = DateTime.Now;
                _context.MenuCategory.Add(category);
                _context.SaveChanges();
                result = category.MenuCategoryId;
            }
            return result;
        }

        public bool CheckCategoryNameExists(string menuCategoryName, int roleId)
        {
            try
            {
                var result = (from category in _context.MenuCategory.AsNoTracking()
                              where category.MenuCategoryName == menuCategoryName && category.RoleId == roleId
                              select category).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int? DeleteCategory(int? categoryId)
        {
            try
            {
                var category = _context.MenuCategory.Find(categoryId);
                _context.Entry(category).State = EntityState.Deleted;
                _context.SaveChanges();
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SelectListItem> GetAllActiveSelectListItemCategory()
        {
            var categoryList = (from cat in _context.MenuCategory
                                where cat.Status == true
                                select new SelectListItem()
                                {
                                    Text = cat.MenuCategoryName,
                                    Value = cat.MenuCategoryId.ToString()
                                }).ToList();

            categoryList.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = "-----Select-----"
            });

            return categoryList;
        }

        public EditCategoriesVM GetCategoryById(int? menuCategoryId)
        {
            try
            {
                var result = (from category in _context.MenuCategory.AsNoTracking()
                              where category.MenuCategoryId == menuCategoryId
                              select new EditCategoriesVM()
                              {
                                  RoleId = category.RoleId,
                                  Status = category.Status,
                                  MenuCategoryId = category.MenuCategoryId,
                                  MenuCategoryName = category.MenuCategoryName

                              }).SingleOrDefault();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<SelectListItem> GetCategorybyRoleId(int? roleId)
        {
            var categoryList = (from cat in _context.MenuCategory
                                where cat.Status == true && cat.RoleId == roleId
                                select new SelectListItem()
                                {
                                    Text = cat.MenuCategoryName,
                                    Value = cat.MenuCategoryId.ToString()
                                }).ToList();

            categoryList.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = "-----Select-----"
            });

            return categoryList;
        }

        public int GetCategoryCount(string menuCategoryName)
        {
            try
            {
                using (var db = new DatabaseContext())
                {
                    if (!string.IsNullOrEmpty(menuCategoryName))
                    {
                        var result = (from category in db.MenuCategory
                                      where category.MenuCategoryName == menuCategoryName
                                      select category).Count();
                        return result;
                    }
                    else
                    {
                        var result = (from category in db.MenuCategory
                                      select category).Count();
                        return result;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<MenuCategoryViewModel> ShowAllMenusCategory(string sortColumn, string sortColumnDir, string search)
        {
            try
            {
                var queryableMenuMaster = (from menuCategory in _context.MenuCategory
                                           join roleMaster in _context.RoleMasters on menuCategory.RoleId equals roleMaster.RoleId
                                           select new MenuCategoryViewModel()
                                           {
                                               Status = menuCategory.Status,
                                               MenuCategoryId = menuCategory.MenuCategoryId,
                                               MenuCategoryName = menuCategory.MenuCategoryName,
                                               RoleName = roleMaster.RoleName
                                           }
                    );

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    queryableMenuMaster = queryableMenuMaster.OrderBy(sortColumn + " " + sortColumnDir);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    queryableMenuMaster = queryableMenuMaster.Where(m => m.MenuCategoryName.Contains(search) || m.MenuCategoryName.Contains(search));
                }

                return queryableMenuMaster;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public int? UpdateCategory(MenuCategory category)
        {
            try
            {
                int? result = -1;
                if (category != null)
                {
                    category.CreatedOn = DateTime.Now;
                    _context.Entry(category).State = EntityState.Modified;
                    _context.SaveChanges();
                    result = category.MenuCategoryId;
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<MenuCategoryCacheViewModel> ShowCategories(int roleId)
        {
            List<MenuCategoryCacheViewModel> renderCategoriesList;
            var key = $"MenuCategory_Cache_{roleId}";
            if (!CacheHelper.CheckExists(key))
            {
                using (var db = new DatabaseContext())
                {
                    var data = (from cat in db.MenuCategory
                                orderby cat.SortingOrder
                                where cat.Status == true && cat.RoleId == roleId
                                select new MenuCategoryCacheViewModel()
                                {
                                    MenuCategoryName = cat.MenuCategoryName,
                                    MenuCategoryId = cat.MenuCategoryId
                                }).ToList();

                    CacheHelper.AddToCacheWithNoExpiration(key, data);
                    return data;
                }
            }
            else
            {
                renderCategoriesList = (List<MenuCategoryCacheViewModel>)CacheHelper.GetStoreCachebyKey(key);
            }

            return renderCategoriesList;
        }

        public bool UpdateMenuCategoryOrder(List<MenuCategoryStoringOrder> menuStoringOrder)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                try
                {
                    foreach (var menu in menuStoringOrder)
                    {
                        var param = new DynamicParameters();
                        param.Add("@MenuCategoryId", menu.MenuCategoryId);
                        param.Add("@RoleId", menu.RoleId);
                        param.Add("@SortingOrder", menu.SortingOrder);
                        con.Execute("Usp_UpdateMenuCategoryOrder", param, transaction, 0, CommandType.StoredProcedure);
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}