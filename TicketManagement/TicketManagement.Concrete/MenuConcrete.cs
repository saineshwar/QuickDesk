using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using TicketManagement.Interface;
using TicketManagement.Models;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using TicketManagement.Concrete.CacheLibrary;
using TicketManagement.ViewModels;

namespace TicketManagement.Concrete
{
    public class MenuConcrete : IMenu
    {
        private readonly DatabaseContext _context;

        public MenuConcrete(DatabaseContext context)
        {
            _context = context;
        }


        public List<MenuMaster> GetAllMenu()
        {
            try
            {
                return _context.MenuMaster.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<SelectListItem> GetAllActiveMenu()
        {
            try
            {
                var listofActiveMenu = (from menu in _context.MenuMaster
                                        where menu.Status == true
                                        select new SelectListItem
                                        {
                                            Value = menu.MenuId.ToString(),
                                            Text = menu.MenuName
                                        }).ToList();


                listofActiveMenu.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "---Select---"
                });

                return listofActiveMenu;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public EditMenuMasterViewModel GetMenuById(int? menuId)
        {
            try
            {
                var editmenu = (from menu in _context.MenuMaster
                                where menu.MenuId == menuId
                                select new EditMenuMasterViewModel()
                                {
                                    Status = menu.Status,
                                    ActionMethod = menu.ActionMethod,
                                    MenuName = menu.MenuName,
                                    ControllerName = menu.ControllerName,
                                    MenuId = menu.MenuId,
                                    RoleId = menu.RoleId,
                                    MenuCategoryId = menu.CategoryId
                                }).FirstOrDefault();

                return editmenu;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public MenuViewModel GetMenutoDeleteById(int? menuId)
        {
            try
            {
                var editmenu = (from menu in _context.MenuMaster
                                where menu.MenuId == menuId
                                select new MenuViewModel()
                                {
                                    Status = menu.Status,
                                    ActionMethod = menu.ActionMethod,
                                    MenuName = menu.MenuName,
                                    ControllerName = menu.ControllerName
                                }).FirstOrDefault();

                return editmenu;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int? AddMenu(MenuMaster menuMaster)
        {
            try
            {
                int? result = -1;

                if (menuMaster != null)
                {
                    _context.MenuMaster.Add(menuMaster);
                    _context.SaveChanges();
                    result = menuMaster.MenuId;
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int? UpdateMenu(MenuMaster menuMaster)
        {
            try
            {
                int? result = -1;

                if (menuMaster != null)
                {
                    using (var db = new DatabaseContext())
                    {
                        menuMaster.CreateDate = DateTime.Now;
                        _context.Entry(menuMaster).State = EntityState.Modified;
                        _context.SaveChanges();
                        result = menuMaster.MenuId;
                    }
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void DeleteMenu(int? menuId)
        {
            try
            {
                MenuMaster menuMaster = _context.MenuMaster.Find(menuId);
                if (menuMaster != null)
                    menuMaster.Status = false;
                _context.Entry(menuMaster).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool CheckMenuNameExists(string menuName)
        {
            try
            {
                var result = (from menu in _context.MenuMaster
                              where menu.MenuName == menuName
                              select menu).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IQueryable<MenuViewModel> ShowAllMenus(string sortColumn, string sortColumnDir, string search)
        {
            try
            {
                var queryableMenuMaster = (from menu in _context.MenuMaster
                                           join category in _context.MenuCategory on menu.CategoryId equals category.MenuCategoryId
                                           join roleMaster in _context.RoleMasters on menu.RoleId equals roleMaster.RoleId
                                           orderby menu.MenuId descending
                                           select new MenuViewModel()
                                           {
                                               Status = menu.Status,
                                               ActionMethod = menu.ActionMethod,
                                               MenuName = menu.MenuName,
                                               ControllerName = menu.ControllerName,
                                               MenuId = menu.MenuId,
                                               RoleName = roleMaster.RoleName,
                                               MenuCategoryName = category.MenuCategoryName
                                           }
                    );

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    queryableMenuMaster = queryableMenuMaster.OrderBy(sortColumn + " " + sortColumnDir);
                }


                if (!string.IsNullOrEmpty(search))
                {
                    queryableMenuMaster = queryableMenuMaster.Where(m => m.MenuName.Contains(search) || m.MenuName.Contains(search));
                }

                return queryableMenuMaster;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<MenuMasterCacheViewModel> GetAllActiveMenu(long roleId)
        {
            try
            {
                string keyMainMenu = "MainMenu_Cache_" + roleId;
                var menuList = (List<MenuMasterCacheViewModel>)CacheHelper.GetStoreCachebyKey(keyMainMenu);
                return menuList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<MenuMaster> GetAllActiveMenuSuperAdmin()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    return con.Query<MenuMaster>("Usp_GetMenusByRoleID_SuperAdmin", param, null, false, 0, CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<MenuCategoryOrderingVm> ListofMenubyRoleCategoryId(int roleId)
        {
            var listofactiveMenus = (from tempmenu in _context.MenuCategory
                                     where tempmenu.Status == true && tempmenu.RoleId == roleId
                                     orderby tempmenu.SortingOrder ascending
                                     select new MenuCategoryOrderingVm
                                     {
                                         MenuCategoryId = tempmenu.MenuCategoryId,
                                         MenuCategoryName = tempmenu.MenuCategoryName
                                     }).ToList();

            return listofactiveMenus;
        }

        public List<MenuMasterOrderingVm> GetListofMenu(int roleId, int menuCategoryId)
        {
            var listofactiveMenus = (from tempmenu in _context.MenuMaster
                                     where tempmenu.Status == true && tempmenu.RoleId == roleId && tempmenu.CategoryId == menuCategoryId
                                     orderby tempmenu.SortingOrder ascending
                                     select new MenuMasterOrderingVm
                                     {
                                         MenuId = tempmenu.MenuId,
                                         MenuName = tempmenu.MenuName
                                     }).ToList();

            return listofactiveMenus;
        }

        public List<SelectListItem> ListofMenubyRoleId(RequestMenus requestMenus)
        {
            var listofactiveMenus = (from tempmenu in _context.MenuMaster
                                     join menu in _context.MenuMaster on tempmenu.MenuId equals menu.MenuId
                                     where tempmenu.Status == true && tempmenu.RoleId == requestMenus.RoleID && tempmenu.CategoryId == requestMenus.CategoryID
                                     orderby tempmenu.MenuId ascending
                                     select new SelectListItem
                                     {
                                         Value = menu.MenuId.ToString(),
                                         Text = menu.MenuName
                                     }).ToList();

            listofactiveMenus.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = "---Select---"
            });
            return listofactiveMenus;
        }

        public List<SelectListItem> ListofMenubyRoleIdSelectListItem(int roleId, int menuCategoryId)
        {
            var listofactiveMenus = (from tempmenu in _context.MenuMaster
                                     where tempmenu.Status == true && tempmenu.RoleId == roleId && tempmenu.CategoryId == menuCategoryId
                                     orderby tempmenu.SortingOrder ascending
                                     select new SelectListItem
                                     {
                                         Value = tempmenu.MenuId.ToString(),
                                         Text = tempmenu.MenuName
                                     }).ToList();

            listofactiveMenus.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = "---Select Main Menu---"
            });

            return listofactiveMenus;
        }



        public bool UpdateMenuOrder(List<MenuStoringOrder> menuStoringOrder)
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
                        param.Add("@MenuId", menu.MenuId);
                        param.Add("@RoleId", menu.RoleId);
                        param.Add("@SortingOrder", menu.SortingOrder);
                        con.Execute("Usp_UpdateMenuOrder", param, transaction, 0, CommandType.StoredProcedure);
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

        public List<SelectListItem> GetAllAssignedMenu()
        {
            try
            {
                var menulist = (from menu in _context.MenuMaster
                                where menu.RoleId != null && menu.Status == true
                                join roles in _context.RoleMasters on menu.RoleId equals roles.RoleId
                                select new SelectListItem()
                                {
                                    Text = menu.MenuName + " | " + roles.RoleName,
                                    Value = menu.MenuId.ToString()
                                }).ToList();
                return menulist;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<SelectListItem> GetAllAssignedMenuWithRoles()
        {
            try
            {
                var menulist = (from menu in _context.MenuMaster
                                where menu.RoleId != null && menu.Status == true
                                join roles in _context.RoleMasters on menu.RoleId equals roles.RoleId
                                select new SelectListItem()
                                {
                                    Text = menu.MenuName + " | " + roles.RoleName,
                                    Value = menu.MenuId.ToString()
                                }).ToList();


                menulist.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "---Select---"
                });

                return menulist;
            }
            catch (Exception)
            {

                throw;
            }
        }

       

        public bool EditValidationCheck(int? menuId, EditMenuMasterViewModel editMenu)
        {
            var result = (from menu in _context.MenuMaster.AsNoTracking()
                          where menu.MenuId == menuId
                          select menu).SingleOrDefault();

            if (result != null && (editMenu.MenuCategoryId == result.CategoryId && editMenu.RoleId == result.RoleId && editMenu.MenuName == result.MenuName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckMenuNameExists(string menuName, int? roleId, int? categoryId)
        {
            try
            {
                var result = (from menu in _context.MenuMaster.AsNoTracking()
                              where menu.MenuName == menuName && menu.RoleId == roleId && menu.CategoryId == categoryId
                              select menu).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
