using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketManagement.Common;
using TicketManagement.CommonData;
using TicketManagement.Filters;
using TicketManagement.Helpers;
using TicketManagement.Interface;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Controllers
{
    [AuthorizeCommonFormUsersAttribute]
    public class CreateNewUserController : Controller
    {
        private readonly IUserMaster _iUserMaster;
        private readonly IPassword _iPassword;
        private readonly ISavedAssignedRoles _savedAssignedRoles;
        private readonly IRole _iRole;
        readonly SessionHandler _sessionHandler = new SessionHandler();
        public CreateNewUserController(IUserMaster userMaster, IPassword password, ISavedAssignedRoles savedAssignedRoles, IRole role)
        {
            _iUserMaster = userMaster;
            _iPassword = password;
            _savedAssignedRoles = savedAssignedRoles;
            _iRole = role;
        }

        // GET: CreateNewUser
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateNewUserViewModel createUserViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_iUserMaster.CheckEmailIdExists(createUserViewModel.EmailId))
                    {
                        TempData["MessageCreateUsersErrors"] = "EmailId Already Exists";
                    }
                    else if (_iUserMaster.CheckMobileNoExists(createUserViewModel.MobileNo))
                    {
                        TempData["MessageCreateUsersErrors"] = "MobileNo Already Exists";
                    }
                    else
                    {
                        string username = GenerateUserName(createUserViewModel);
                        Usermaster usermaster = new Usermaster();
                        usermaster.Status = true;
                        usermaster.CreateDate = DateTime.Now;
                        usermaster.UserId = 0;
                        usermaster.FirstName = createUserViewModel.FirstName;
                        usermaster.LastName = createUserViewModel.LastName;
                        usermaster.EmailId = createUserViewModel.EmailId;
                        usermaster.MobileNo = createUserViewModel.MobileNo;
                        usermaster.UserName = username;
                        usermaster.CreatedBy = Convert.ToInt32(_sessionHandler.UserId);
                        usermaster.Gender = createUserViewModel.Gender;
                        var salt = GenerateRandomNumbers.RandomNumbers(20);
                        var password = GeneratePassword(createUserViewModel, salt);
                        var userId = _iUserMaster.AddUser(usermaster, password, salt, Convert.ToInt16(StatusMain.Roles.User));
                        if (userId != -1)
                        {
                            TempData["MessageCreateUsers"] = $"User Created Successfully Username :- {usermaster.UserName}";
                        }
                        return RedirectToAction("Create", "CreateNewUser");
                    }

                    return View("Create", createUserViewModel);
                }
                else
                {
                    return View("Create", createUserViewModel);
                }
            }
            catch
            {
                throw;
            }
        }

        private string GenerateUserName(CreateNewUserViewModel createUserViewModel)
        {
            try
            {
                string mobileno = createUserViewModel.MobileNo;
                string username = string.Concat(createUserViewModel.FirstName, mobileno.Substring(6, 4)).Trim();
                if (CheckUsernameExists(username))
                {
                    username = string.Concat(createUserViewModel.LastName, mobileno.Substring(6, 4)).Trim();
                    if (CheckUsernameExists(username))
                    {
                        username = string.Concat(createUserViewModel.LastName, mobileno.Substring(3, 6)).Trim();
                        if (CheckUsernameExists(username))
                        {

                        }
                    }
                }
                return username;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool CheckUsernameExists(string username)
        {
            try
            {
                var isUser = _iUserMaster.CheckUsernameExists(username);
                return isUser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //tkts + onecharfirstName + onecharlastName + mobileno5 digit 
        private string GeneratePassword(CreateNewUserViewModel createUserViewModel, string salt)
        {
            try
            {
                string onecharfirstName = createUserViewModel.FirstName.Substring(0, 1);
                string onecharlastName = createUserViewModel.LastName.Substring(0, 1);
                string mobileno = createUserViewModel.MobileNo.Substring(5, 5);
                string password = string.Concat("tkts", onecharfirstName.ToLower(), onecharlastName.ToLower(), mobileno);
                var saltedpassword = GenerateHashSha512.Sha512(password, salt);
                return saltedpassword;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult Show()
        {
            return View();
        }

        [HttpPost]//Gets the todo Lists.  
        public JsonResult UserList(string username, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                var userCount = _iUserMaster.GetOnlyUserCount(username);
                var roles = _iUserMaster.GetOnlyUserList(username, jtStartIndex, jtPageSize, jtSorting);
                return Json(new { Result = "OK", Records = roles, TotalRecordCount = userCount });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult Edit(long? id)
        {
            try
            {
                if (id != null)
                {
                    var createUserViewModel = _iUserMaster.EditUserbyUserId(id);
                    return View(createUserViewModel);
                }
                else
                {
                    return RedirectToAction("Show", "AllUsers");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult Edit(EditUserViewModel editUserViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var isUser = _iUserMaster.CheckUserIdExists(editUserViewModel.UserId);
                    if (isUser)
                    {
                        var createUserViewModel = _iUserMaster.EditUserbyUserId(editUserViewModel.UserId);

                        if (createUserViewModel.EmailId != editUserViewModel.EmailId)
                        {
                            if (_iUserMaster.CheckEmailIdExists(editUserViewModel.EmailId))
                            {
                                ModelState.AddModelError("", "EmailId already exists");
                                return View(editUserViewModel);
                            }
                        }

                        if (createUserViewModel.MobileNo != editUserViewModel.MobileNo)
                        {
                            if (_iUserMaster.CheckMobileNoExists(editUserViewModel.MobileNo))
                            {
                                ModelState.AddModelError("", "MobileNo already exists");
                                return View(editUserViewModel);
                            }
                        }


                        var result = _iUserMaster.UpdateUserDetails(editUserViewModel);
                        TempData["MessageUpdateUsers"] = "User Details Updated Successfully";
                        return View(editUserViewModel);
                    }
                    else
                    {
                        TempData["MessageCreateUsersErrors"] = "User Details doesn't exist";
                        return View(editUserViewModel);
                    }
                }
                else
                {
                    return View("Edit", editUserViewModel);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}