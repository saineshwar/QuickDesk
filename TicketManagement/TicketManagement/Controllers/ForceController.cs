using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using TicketManagement.Common;
using TicketManagement.CommonData;
using TicketManagement.Concrete.CacheLibrary;
using TicketManagement.Filters;
using TicketManagement.Helpers;
using TicketManagement.Interface;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Controllers
{
    [ValidateIsPasswordChange]
    public class ForceController : Controller
    {
        private readonly IPassword _password;
        private readonly IUserMaster _userMaster;
        private readonly IAgentCheckInStatus _agentCheckInStatus;
        private readonly ISavedAssignedRoles _savedAssignedRoles;
        private readonly IProfile _iProfile;
        private readonly ICategory _category;

        public ForceController(IPassword password, IUserMaster iUserMaster, IAgentCheckInStatus agentCheckInStatus, ISavedAssignedRoles savedAssignedRoles, IProfile iProfile, ICategory category)
        {
            _password = password;
            _userMaster = iUserMaster;
            _agentCheckInStatus = agentCheckInStatus;
            _savedAssignedRoles = savedAssignedRoles;
            _iProfile = iProfile;
            _category = category;
        }

        // GET: Force
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordFirstLoginModel changePassword)
        {
            if (ModelState.IsValid)
            {
                var storedUserpassword = _password.GetPasswordbyUserId(Convert.ToInt64(Session["ChangePasswordUserId"]));
                var usersalt = _userMaster.GetUserSaltbyUserid(Convert.ToInt64(Session["ChangePasswordUserId"]));
                var userrole = Session["ChangeRoleId"];
                var generatehash = GenerateHashSha512.Sha512(changePassword.CurrentPassword, usersalt.PasswordSalt);
                var rolesModel = _savedAssignedRoles.GetAssignedRolesbyUserId(Convert.ToInt64(Session["ChangePasswordUserId"]));


                if (changePassword.CurrentPassword == changePassword.NewPassword)
                {
                    ModelState.AddModelError("", @"New Password Cannot be same as Old Password");
                    return View(changePassword);
                }

                if (!string.Equals(storedUserpassword, generatehash, StringComparison.Ordinal))
                {
                    ModelState.AddModelError("", "Current Password Entered is InValid");
                    return View(changePassword);
                }

                if (!string.Equals(changePassword.NewPassword, changePassword.ConfirmPassword, StringComparison.Ordinal))
                {
                    TempData["ChangePasswordErrorMessage"] = "Something Went Wrong Please Try Again!";
                    return View(changePassword);
                }
                else
                {
                    var userid = Convert.ToInt64(Session["ChangePasswordUserId"]);
                    var usermasterModel = _userMaster.GetUserById(userid);
                    var salt = GenerateRandomNumbers.RandomNumbers(20);
                    var saltedpassword = GenerateHashSha512.Sha512(changePassword.NewPassword, salt);
                    var result = _password.UpdatePasswordandHistory(userid, saltedpassword, salt, "C");
                    var resultIsFirstLogin = _userMaster.UpdateIsFirstLogin(userid);
                    if (result && resultIsFirstLogin > 0)
                    {
                        TempData["ChangePasswordMessage"] = "Password Changed Successfully You might need to sign in again";
                        AssignSessionValues(usermasterModel, rolesModel);
                        return RedirectionManager(usermasterModel, rolesModel);
                    }
                    else
                    {
                        TempData["ChangePasswordErrorMessage"] = "Something Went Wrong Please Try Again!";
                        return View(changePassword);
                    }

                }

            }

            return View(changePassword);
        }

        public ActionResult RedirectionManager(Usermaster usermasterModel, SavedAssignedRolesViewModel rolesModel)
        {
            if (rolesModel.RoleId == Convert.ToInt32(StatusMain.Roles.SuperAdmin))
            {
                return RedirectToAction("Dashboard", "SuperDashboard");
            }

            if (rolesModel.RoleId == Convert.ToInt32(StatusMain.Roles.User))
            {
                return RedirectToAction("Dashboard", "UserDashboard");
            }

            if (rolesModel.RoleId == Convert.ToInt32(StatusMain.Roles.Admin))
            {
                return RedirectToAction("Dashboard", "AdminDashboard");
            }

            if (rolesModel.RoleId == Convert.ToInt32(StatusMain.Roles.AgentAdmin))
            {
                return RedirectToAction("Dashboard", "AgentAdminDashboard");
            }

            if (rolesModel.RoleId == Convert.ToInt32(StatusMain.Roles.Hod))
            {
                return RedirectToAction("Dashboard", "HODDashboard");
            }

            if (rolesModel.RoleId == Convert.ToInt32(StatusMain.Roles.Agent))
            {
                if (_agentCheckInStatus.CheckIsalreadyCheckedIn(usermasterModel.UserId))
                {
                    return RedirectToAction("Dashboard", "AgentDashboard");
                }
                else
                {
                    return RedirectToAction("Alerts", "CheckInAlert");
                }
            }

            return RedirectToAction("Login", "Login");

        }

        private void AssignSessionValues(Usermaster usermasterModel, SavedAssignedRolesViewModel rolesModel)
        {
            var sessionHandler = new SessionHandler
            {
                UserId = Convert.ToString(usermasterModel.UserId),
                UserName = usermasterModel.FirstName + " " + usermasterModel.LastName,
                EmailId = usermasterModel.EmailId,
                RoleId = Convert.ToString(rolesModel.RoleId),
                RoleName = Convert.ToString(rolesModel.RoleName),
                CacheProfileKey = "Cache_" + usermasterModel.UserId
            };

            if (rolesModel.RoleId == Convert.ToInt32(StatusMain.Roles.AgentAdmin))
            {
                sessionHandler.AgentAdminCategoryId = Convert.ToString(_category.GetAdminCategory(usermasterModel.UserId));
            }

            if (rolesModel.RoleId == Convert.ToInt32(StatusMain.Roles.Hod))
            {
                sessionHandler.HodCategoryId = Convert.ToString(_category.GetHodCategory(usermasterModel.UserId));
            }


            var result = _iProfile.IsProfileImageExists(Convert.ToInt64(sessionHandler.UserId));

            if (result)
            {
                string cacheProfileKey = Convert.ToString(sessionHandler.CacheProfileKey);

                if (!CacheHelper.CheckExists(cacheProfileKey))
                {
                    var imageBase64String = _iProfile.GetProfileImageBase64String(Convert.ToInt64(sessionHandler.UserId));
                    var tempimageBase64String = string.Concat("data:image/png;base64,", imageBase64String);
                    CacheHelper.AddToCacheWithNoExpiration(cacheProfileKey, tempimageBase64String);
                }
            }
        }
    }
}