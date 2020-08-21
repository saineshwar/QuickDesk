using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketManagement.Common;
using TicketManagement.Filters;
using TicketManagement.Helpers;
using TicketManagement.Interface;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Controllers
{
    [AuthorizeSuperAdminAttribute]
    public class CreateUsersController : Controller
    {
        private readonly IUserMaster _iUserMaster;
        private readonly IPassword _iPassword;
        private readonly ISavedAssignedRoles _savedAssignedRoles;
        private readonly IRole _iRole;
        readonly SessionHandler _sessionHandler = new SessionHandler();
        public CreateUsersController(IUserMaster userMaster, IPassword password, ISavedAssignedRoles savedAssignedRoles, IRole role)
        {
            _iUserMaster = userMaster;
            _iPassword = password;
            _savedAssignedRoles = savedAssignedRoles;
            _iRole = role;
        }

        // GET: CreateUsers
        public ActionResult Create()
        {
            try
            {
                var createUserViewModel = new CreateUserViewModel()
                {
                    ListRole = _iRole.GetAllActiveRolesNotAgent()
                };
                return View(createUserViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult Create(CreateUserViewModel createUserViewModel)
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
                    else if (_iUserMaster.CheckUsernameExists(createUserViewModel.UserName))
                    {
                        ModelState.AddModelError("", "Username already exists");
                    }
                    else
                    {
                        createUserViewModel.FirstName = UppercaseFirst(createUserViewModel.FirstName);
                        createUserViewModel.LastName = UppercaseFirst(createUserViewModel.LastName);
                        var usermaster = AutoMapper.Mapper.Map<Usermaster>(createUserViewModel);
                        usermaster.Status = true;
                        usermaster.CreateDate = DateTime.Now;
                        usermaster.UserId = 0;
                        usermaster.CreatedBy = Convert.ToInt32(_sessionHandler.UserId);
                        var salt = GenerateRandomNumbers.RandomNumbers(20);
                        var saltedpassword = GenerateHashSha512.Sha512(createUserViewModel.Password, salt);

                        var userId = _iUserMaster.AddUser(usermaster, saltedpassword, salt, createUserViewModel.RoleId);
                        if (userId != -1)
                        {
                            TempData["MessageCreateUsers"] = "User Created Successfully";
                        }

                        return RedirectToAction("Create", "CreateUsers");
                    }

                    createUserViewModel.ListRole = _iRole.GetAllActiveRolesNotAgent();
                    return View("Create", createUserViewModel);
                }
                else
                {
                    createUserViewModel.ListRole = _iRole.GetAllActiveRolesNotAgent();
                    return View("Create", createUserViewModel);
                }
            }
            catch
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

        static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

    }
}