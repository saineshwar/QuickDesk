using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using Dapper;
using TicketManagement.Concrete.CacheLibrary;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Concrete
{
    public static class RenderSubMenuConcrete
    {
        public static List<SubMenuMasterViewModel> ShowSubMenu(int menuId, int roleId)
        {
            try
            {
                string keySubMenu = "SubMenu_Cache_" + roleId;

                var subMenuList = (List<SubMenuMasterViewModel>)CacheHelper.GetStoreCachebyKey(keySubMenu);
                if (subMenuList != null)
                {
                    subMenuList = (from tempsubmenu in subMenuList
                        where tempsubmenu.RoleId == roleId && tempsubmenu.MenuId == menuId
                        select tempsubmenu).ToList();
                    return subMenuList;
                }
                else
                {
                    return new List<SubMenuMasterViewModel>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
