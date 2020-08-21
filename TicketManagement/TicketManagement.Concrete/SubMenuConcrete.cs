using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Interface;
using TicketManagement.Models;
using TicketManagement.ViewModels;
using System.Linq.Dynamic;
using System.Web.Mvc;
using Dapper;

namespace TicketManagement.Concrete
{
    public class SubMenuConcrete : ISubMenu
    {
        private readonly DatabaseContext _context;

        public SubMenuConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public int? AddSubMenu(SubMenuMaster subMenuMaster)
        {
            try
            {
                int? result = -1;

                if (subMenuMaster != null)
                {
                    subMenuMaster.CreateDate = DateTime.Now;
                    _context.SubMenuMasters.Add(subMenuMaster);
                    _context.SaveChanges();
                    result = subMenuMaster.MenuId;
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int DeleteSubMenu(int? subMenuId)
        {
            try
            {
                SubMenuMaster subMenuMaster = _context.SubMenuMasters.Find(subMenuId);
                if (subMenuMaster != null)
                    _context.Entry(subMenuMaster).State = EntityState.Deleted;
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<SubMenuMaster> GetAllSubMenu()
        {
            try
            {
                return _context.SubMenuMasters.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public EditSubMenuMaster GetSubMenuById(int? subMenuId)
        {
            try
            {
                var result = (from submenu in _context.SubMenuMasters
                              where submenu.SubMenuId == subMenuId
                              select new EditSubMenuMaster()
                              {
                                  RoleID = submenu.RoleId,
                                  Status = submenu.Status,
                                  MenuCategoryId = submenu.CategoryId,
                                  SubMenuName = submenu.SubMenuName,
                                  ControllerName = submenu.ControllerName,
                                  ActionMethod = submenu.ActionMethod,
                                  MenuId = submenu.MenuId,
                                  SubMenuId = submenu.SubMenuId,

                              }).SingleOrDefault();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int? UpdateSubMenu(SubMenuMaster subMenuMaster)
        {
            try
            {
                int? result = -1;

                if (subMenuMaster != null)
                {
                    subMenuMaster.CreateDate = DateTime.Now;
                    _context.Entry(subMenuMaster).State = EntityState.Modified;
                    _context.SaveChanges();
                    result = subMenuMaster.SubMenuId;
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckSubMenuNameExists(string subMenuName, int menuId)
        {
            try
            {
                var result = (from submenu in _context.SubMenuMasters
                              where submenu.SubMenuName == subMenuName && submenu.MenuId == menuId
                              select submenu).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckSubMenuNameExists(string subMenuName, int? menuId, int? roleId, int? categoryId)
        {
            try
            {
                var result = (from subMenu in _context.SubMenuMasters.AsNoTracking()
                              where subMenu.SubMenuName == subMenuName
                                    && subMenu.MenuId == menuId
                                    && subMenu.RoleId == roleId
                                    && subMenu.CategoryId == categoryId
                              select subMenu).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<SubMenuMasterViewModel> ShowAllSubMenus(string sortColumn, string sortColumnDir, string search)
        {
            try
            {
                var queryablesSubMenuMasters = (from submenu in _context.SubMenuMasters
                                                join category in _context.MenuCategory on submenu.CategoryId equals category.MenuCategoryId
                                                join roleMaster in _context.RoleMasters on submenu.RoleId equals roleMaster.RoleId
                                                join menuMaster in _context.MenuMaster on submenu.MenuId equals menuMaster.MenuId

                                                select new SubMenuMasterViewModel
                                                {
                                                    SubMenuName = submenu.SubMenuName,
                                                    MenuName = menuMaster.MenuName,
                                                    ActionMethod = submenu.ActionMethod,
                                                    ControllerName = submenu.ControllerName,
                                                    Status = submenu.Status,
                                                    SubMenuId = submenu.SubMenuId,
                                                    RoleName = roleMaster.RoleName,
                                                    MenuCategoryName = category.MenuCategoryName
                                                });

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    queryablesSubMenuMasters = queryablesSubMenuMasters.OrderBy(sortColumn + " " + sortColumnDir);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    queryablesSubMenuMasters = queryablesSubMenuMasters.Where(m => m.SubMenuName.Contains(search) || m.SubMenuName.Contains(search));
                }

                return queryablesSubMenuMasters;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SelectListItem> GetAllActiveSubMenu(int menuid)
        {
            try
            {
                var listofActiveMenu = (from submenu in _context.SubMenuMasters
                                        where submenu.Status == true && submenu.MenuId == menuid
                                        select new SelectListItem
                                        {
                                            Value = submenu.SubMenuId.ToString(),
                                            Text = submenu.SubMenuName
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


        public List<SelectListItem> GetAllActiveSubMenuWithoutDefault(int menuid)
        {
            try
            {
                var listofActiveMenu = (from submenu in _context.SubMenuMasters
                                        where submenu.Status == true && submenu.MenuId == menuid
                                        select new SelectListItem
                                        {
                                            Value = submenu.SubMenuId.ToString(),
                                            Text = submenu.SubMenuName
                                        }).ToList();

                return listofActiveMenu;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<SubMenuMaster> GetAllActiveSubMenuByMenuId(int menuid)
        {
            try
            {
                var listofActiveMenu = (from submenu in _context.SubMenuMasters
                                        where submenu.Status == true && submenu.MenuId == menuid
                                        select submenu).ToList();
                return listofActiveMenu;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<SubMenuMasterOrderingVm> ListofSubMenubyRoleId(int roleId, int menuid)
        {
            var listofactiveMenus = (from tempsubmenu in _context.SubMenuMasters
                                     where tempsubmenu.Status == true && tempsubmenu.RoleId == roleId && tempsubmenu.MenuId == menuid
                                     orderby tempsubmenu.SortingOrder ascending
                                     select new SubMenuMasterOrderingVm
                                     {
                                         SubMenuId = tempsubmenu.SubMenuId,
                                         SubMenuName = tempsubmenu.SubMenuName
                                     }).ToList();

            return listofactiveMenus;
        }

        public bool UpdateSubMenuOrder(List<SubMenuStoringOrder> submenuStoringOrder)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                try
                {
                    foreach (var submenu in submenuStoringOrder)
                    {
                        var param = new DynamicParameters();
                        param.Add("@MenuId", submenu.MenuId);
                        param.Add("@RoleId", submenu.RoleId);
                        param.Add("@SortingOrder", submenu.SortingOrder);
                        param.Add("@SubMenuId", submenu.SubMenuId);
                        con.Execute("Usp_UpdateSubMenuOrder", param, transaction, 0, CommandType.StoredProcedure);
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

        public bool EditValidationCheck(int? subMenuId, EditSubMenuMaster editsubMenu)
        {
            var result = (from submenu in _context.SubMenuMasters.AsNoTracking()
                          where submenu.SubMenuId == subMenuId
                          select submenu).SingleOrDefault();

            if (result != null && (editsubMenu.MenuId == result.MenuId
                                   && editsubMenu.MenuCategoryId == result.CategoryId
                                   && editsubMenu.RoleID == result.RoleId
                                   && editsubMenu.SubMenuName == result.SubMenuName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
