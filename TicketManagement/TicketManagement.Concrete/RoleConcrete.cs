using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using Dapper;
using TicketManagement.Concrete.CacheLibrary;
using TicketManagement.Interface;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Concrete
{
    public class RoleConcrete : IRole
    {
        private readonly DatabaseContext _context;


        public RoleConcrete(DatabaseContext context)
        {
            _context = context;
        }

      
        public int? AddRoleMaster(RoleMaster roleMaster)
        {
            try
            {
                int? result = -1;
                if (roleMaster != null)
                {
                    roleMaster.CreateDate = DateTime.Now;
                    _context.RoleMasters.Add(roleMaster);
                    _context.SaveChanges();
                    result = roleMaster.RoleId;
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool CheckRoleMasterNameExists(string roleName)
        {
            try
            {
                var result = (from role in _context.RoleMasters
                              where role.RoleName == roleName
                              select role).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteRoleMaster(int? roleId)
        {
            try
            {
                RoleMaster roleMaster = _context.RoleMasters.Find(roleId);
                if (roleMaster != null) _context.RoleMasters.Remove(roleMaster);
                _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<RoleMaster> GetAllRoleMaster()
        {
            try
            {
                return _context.RoleMasters.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public RoleMaster GetRoleMasterById(int? roleId)
        {
            try
            {
                return _context.RoleMasters.Find(roleId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IQueryable<RoleMaster> ShowAllRoleMaster(string sortColumn, string sortColumnDir, string search)
        {
            try
            {
                var queryablesRoleMasters = (from roleMaster in _context.RoleMasters
                                             select roleMaster);

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    queryablesRoleMasters = queryablesRoleMasters.OrderBy(sortColumn + " " + sortColumnDir);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    queryablesRoleMasters = queryablesRoleMasters.Where(m => m.RoleName.Contains(search) || m.RoleName.Contains(search));
                }

                return queryablesRoleMasters;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public int? UpdateRoleMaster(RoleMaster roleMaster)
        {
            try
            {
                int? result = -1;

                if (roleMaster != null)
                {
                    roleMaster.CreateDate = DateTime.Now;
                    _context.Entry(roleMaster).State = EntityState.Modified;
                    _context.SaveChanges();
                    result = roleMaster.RoleId;
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<SelectListItem> GetAllActiveRoles()
        {
            try
            {
                string key = "Role_Cache";
                List<SelectListItem> listofActiveMenu;
                if (!CacheHelper.CheckExists(key))
                {
                    listofActiveMenu = (from roleMaster in _context.RoleMasters
                        where roleMaster.Status == true
                        select new SelectListItem
                        {
                            Value = roleMaster.RoleId.ToString(),
                            Text = roleMaster.RoleName
                        }).ToList();

                    listofActiveMenu.Insert(0, new SelectListItem()
                    {
                        Value = "",
                        Text = "---Select Role---"
                    });
                    CacheHelper.AddToCacheWithNoExpiration(key, listofActiveMenu);
                }
                else
                {
                    listofActiveMenu = (List<SelectListItem>)CacheHelper.GetStoreCachebyKey(key);
                }

                return listofActiveMenu;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int? UpdateRoleStatus(ViewMenuRoleStatusUpdateModel vmrolemodel)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    con.Open();
                    SqlTransaction trans = con.BeginTransaction();
                    var param = new DynamicParameters();
                    param.Add("@Status", vmrolemodel.Status);
                    param.Add("@SavedMenuRoleId", vmrolemodel.SaveId);
                    var result = con.Execute("Usp_UpdateRoleStatus", param, trans, 0, CommandType.StoredProcedure);
                    if (result > 0)
                    {
                        trans.Commit();
                    }
                    else
                    {
                        trans.Rollback();
                    }

                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int? UpdateSubMenuRoleStatus(ViewSubMenuRoleStatusUpdateModel vmrolemodel)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    con.Open();
                    SqlTransaction trans = con.BeginTransaction();
                    var param = new DynamicParameters();
                    param.Add("@Status", vmrolemodel.Status);
                    param.Add("@SavedSubMenuRoleId", vmrolemodel.SaveId);
                    var result = con.Execute("UpdateSubMenuRoleStatus", param, trans, 0, CommandType.StoredProcedure);
                    if (result > 0)
                    {
                        trans.Commit();
                    }
                    else
                    {
                        trans.Rollback();
                    }

                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SelectListItem> GetAllActiveRolesNotAgent()
        {
            try
            {
                var listofActiveMenu = (from roleMaster in _context.RoleMasters
                                        where roleMaster.Status == true && roleMaster.RoleId != 4
                                        select new SelectListItem
                                        {
                                            Value = roleMaster.RoleId.ToString(),
                                            Text = roleMaster.RoleName
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

        public List<SelectListItem> GetAllActiveRolesAgent()
        {
            try
            {
                var listofActiveMenu = (from roleMaster in _context.RoleMasters
                                        where roleMaster.Status == true && roleMaster.RoleId == 4
                                        select new SelectListItem
                                        {
                                            Value = roleMaster.RoleId.ToString(),
                                            Text = roleMaster.RoleName
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
    }
}
