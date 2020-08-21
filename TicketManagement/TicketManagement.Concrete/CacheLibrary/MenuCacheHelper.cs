using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.ViewModels;

namespace TicketManagement.Concrete.CacheLibrary
{
    public class MenuCacheHelper
    {
        //CacheHelper
        public void LoadMenu()
        {
            DatabaseContext context = new DatabaseContext();

            var listofroles = (from roleMaster in context.RoleMasters
                               where roleMaster.Status == true
                               select roleMaster).ToList();



            foreach (var role in listofroles)
            {
                string keyMenuCategory = "MenuCategory_Cache_" + role.RoleId;

                
                var listofMenuCategory = (from tempmenu in context.MenuCategory
                                          where tempmenu.Status == true && tempmenu.Status == true && tempmenu.RoleId == role.RoleId
                                          orderby tempmenu.SortingOrder ascending
                                          select new MenuCategoryCacheViewModel
                                          {
                                              MenuCategoryId = tempmenu.MenuCategoryId,
                                              MenuCategoryName = tempmenu.MenuCategoryName,
                                              RoleId = tempmenu.RoleId
                                          }).ToList();

                if (listofMenuCategory.Count > 0)
                {
                    CacheHelper.AddToCacheWithNoExpiration(keyMenuCategory, listofMenuCategory);
                }

                var listofmenuCategory = (from menuCategory in context.MenuCategory
                                          where menuCategory.Status == true && menuCategory.RoleId == role.RoleId
                                          select menuCategory).ToList();

                foreach (var category in listofmenuCategory)
                {
                    var listofmainMenus = (from menu in context.MenuMaster

                                           where menu.Status == true && menu.Status == true && menu.RoleId == role.RoleId && menu.CategoryId == category.MenuCategoryId
                                           orderby menu.SortingOrder ascending
                                           select new MenuMasterCacheViewModel
                                           {
                                               MenuId = menu.MenuId,
                                               MenuName = menu.MenuName,
                                               ActionMethod = menu.ActionMethod,
                                               ControllerName = menu.ControllerName,
                                               RoleId = menu.RoleId,
                                           }).ToList();

                    if (listofmainMenus.Count > 0)
                    {
                        string keyMainMenu = $"MainMenu_Cache_{role.RoleId}_{category.MenuCategoryId}";
                        CacheHelper.AddToCacheWithNoExpiration(keyMainMenu, listofmainMenus);
                    }

                }


                foreach (var category in listofmenuCategory)
                {
                    var listofmainMenus = (from menu in context.MenuMaster

                        where menu.Status == true && menu.Status == true && menu.RoleId == role.RoleId && menu.CategoryId == category.MenuCategoryId
                        orderby menu.SortingOrder ascending
                        select new MenuMasterCacheViewModel
                        {
                            MenuId = menu.MenuId,
                            MenuName = menu.MenuName,
                            ActionMethod = menu.ActionMethod,
                            ControllerName = menu.ControllerName,
                            RoleId = menu.RoleId,
                        }).ToList();

                    foreach (var menu in listofmainMenus)
                    {
                        var listofsubMenus = (from submenu in context.SubMenuMasters

                            where submenu.Status == true && submenu.Status == true && submenu.RoleId == role.RoleId && submenu.MenuId == menu.MenuId
                                  && submenu.CategoryId == category.MenuCategoryId
                            orderby submenu.SortingOrder ascending
                            select new SubMenuMasterViewModel
                            {
                                MenuId = submenu.MenuId,
                                MenuName = submenu.SubMenuName,
                                ActionMethod = submenu.ActionMethod,
                                ControllerName = submenu.ControllerName,
                                RoleId = submenu.RoleId,
                                SubMenuId = submenu.SubMenuId,
                                SubMenuName = submenu.SubMenuName
                            }).ToList();

                        if (listofsubMenus.Count > 0)
                        {
                            string keySubMenu = $"SubMenu_Cache_{role.RoleId}_{category.MenuCategoryId}_{menu.MenuId}";
                            CacheHelper.AddToCacheWithNoExpiration(keySubMenu, listofsubMenus);
                        }
                    }
                    
                }
            }

        }
    }
}
