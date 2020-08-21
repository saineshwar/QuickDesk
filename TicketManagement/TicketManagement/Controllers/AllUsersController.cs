using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketManagement.Concrete;
using TicketManagement.Filters;
using TicketManagement.Interface;
using TicketManagement.ViewModels;

namespace TicketManagement.Controllers
{
    [AuthorizeSuperAdminAttribute]
    public class AllUsersController : Controller
    {
        private readonly IUserMaster _userMaster;
        public AllUsersController(IUserMaster userMaster)
        {
            _userMaster = userMaster;
        }

        // GET: AllUsers
        public ActionResult Show()
        {
            return View();
        }

        [HttpPost]//Gets the todo Lists.  
        public JsonResult UserList(string username, int? roleid, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                var userCount = _userMaster.GetUserCount(username, roleid);
                var roles = _userMaster.GetUserList(username, roleid, jtStartIndex, jtPageSize, jtSorting);
                return Json(new { Result = "OK", Records = roles, TotalRecordCount = userCount });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult RemoveUser(int userId)
        {
            try
            {
                using (var db = new DatabaseContext())
                {
                    var roleId = (from sar in db.SavedAssignedRoles
                                  where sar.UserId == userId
                                  select sar.RoleId).FirstOrDefault();

                    if (roleId != null)
                    {
                        var role = db.RoleMasters.Find(roleId);

                        if (role != null && role.RoleId == Convert.ToInt32(ConfigurationManager.AppSettings["SuperAdminRolekey"]))
                        {
                            return Json(new { Result = "ERROR", Message = "Cannot Delete Super Admin" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            var usermaster = db.Usermasters.Find(userId);
                            if (usermaster != null) db.Usermasters.Remove(usermaster);
                            db.SaveChanges();

                            var password = db.PasswordMaster.Find(userId);
                            if (password != null) db.PasswordMaster.Remove(password);
                            db.SaveChanges();

                            var savedAssignedRoles = db.SavedAssignedRoles.Find(userId);
                            if (savedAssignedRoles != null) db.SavedAssignedRoles.Remove(savedAssignedRoles);
                            db.SaveChanges();
                        }
                    }
                }
                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}