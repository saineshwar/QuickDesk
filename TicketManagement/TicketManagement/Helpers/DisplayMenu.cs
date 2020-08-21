using System.Collections.Generic;
using System.Linq;
using TicketManagement.Concrete;
using TicketManagement.Concrete.CacheLibrary;
using TicketManagement.ViewModels;

namespace TicketManagement.Helpers
{
    public static class DisplayMenu
    {
        public static List<MenuMasterCacheViewModel> ShowMenu(int roleId, int categoryId)
        {
            List<MenuMasterCacheViewModel> renderCategoriesList;
            string key = $"MainMenu_Cache_{roleId}_{categoryId}";
            if (!CacheHelper.CheckExists(key))
            {
                using (var db = new DatabaseContext())
                {
                    var data = (from menu in db.MenuMaster
                                where menu.Status == true && menu.RoleId == roleId && menu.CategoryId == categoryId
                                select new MenuMasterCacheViewModel()
                                {
                                    MenuName = menu.MenuName,
                                    ControllerName = menu.ControllerName,
                                    ActionMethod = menu.ActionMethod,
                                    MenuId = menu.MenuId
                                    
                                }).ToList();
                    CacheHelper.AddToCacheWithNoExpiration(key, data);
                    return data;
                }
            }
            else
            {
                renderCategoriesList = (List<MenuMasterCacheViewModel>)CacheHelper.GetStoreCachebyKey(key);
            }

            return renderCategoriesList;
        }

        public static List<SubMenuMasterViewModel> ShowSubMenu(int roleId, int categoryId, int menuid)
        {
            List<SubMenuMasterViewModel> renderSubMenuList;
            var key = $"SubMenu_Cache_{roleId}_{categoryId}_{menuid}";
            if (!CacheHelper.CheckExists(key))
            {
                using (var db = new DatabaseContext())
                {
                    var data = (from submenu in db.SubMenuMasters
                                where submenu.Status == true && submenu.RoleId == roleId && submenu.CategoryId == categoryId &&
                                      submenu.MenuId == menuid
                                select new SubMenuMasterViewModel()
                                {
                                    SubMenuName = submenu.SubMenuName,
                                    ControllerName = submenu.ControllerName,
                                    ActionMethod = submenu.ActionMethod,
                                    SubMenuId = submenu.SubMenuId
                                }).ToList();
                    CacheHelper.AddToCacheWithNoExpiration(key, data);
                    return data;
                }
            }
            else
            {
                renderSubMenuList = (List<SubMenuMasterViewModel>)CacheHelper.GetStoreCachebyKey(key);
            }

            return renderSubMenuList;
        }

        public static int? ShowSubMenuCount(int roleId, int categoryId, int menuid)
        {
            using (var db = new DatabaseContext())
            {
                var data = (from submenu in db.SubMenuMasters
                            where submenu.Status == true && submenu.RoleId == roleId && submenu.CategoryId == categoryId && submenu.MenuId == menuid
                            select submenu).Count();
                return data;
            }
        }
    }
}