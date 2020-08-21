using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    [AuthorizeSuperAdmin]
    public class CreateAgentController : Controller
    {
        // GET: CreateAgent
        private readonly IUserMaster _iUserMaster;
        private readonly IPassword _iPassword;
        private readonly ISavedAssignedRoles _savedAssignedRoles;
        private readonly IRole _iRole;
        private readonly ICategory _category;
        readonly SessionHandler _sessionHandler = new SessionHandler();
        public CreateAgentController(ICategory category, IUserMaster userMaster, IPassword password, ISavedAssignedRoles savedAssignedRoles, IRole role)
        {
            _iUserMaster = userMaster;
            _iPassword = password;
            _savedAssignedRoles = savedAssignedRoles;
            _iRole = role;
            _category = category;
        }

        // GET: CreateUsers
        public ActionResult Create()
        {
            try
            {
                var createAgentViewModel = new CreateAgentViewModel()
                {
                    ListofCategory = _category.GetAllActiveSelectListItemCategory()
                };
                return View(createAgentViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult Create(CreateAgentViewModel createAgentViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var isUser = _iUserMaster.CheckUsernameExists(createAgentViewModel.UserName);

                    if (createAgentViewModel.CategoryId == null)
                    {
                        ModelState.AddModelError("", "Select Category");
                    }
                    else if (isUser)
                    {
                        ModelState.AddModelError("", "Username already exists");
                    }
                    else
                    {
                        var usermaster = AutoMapper.Mapper.Map<Usermaster>(createAgentViewModel);
                        usermaster.Status = true;
                        usermaster.CreateDate = DateTime.Now;
                        usermaster.UserId = 0;
                        usermaster.CreatedBy = Convert.ToInt32(_sessionHandler.UserId);
                        usermaster.IsFirstLogin = true;

                        var salt = GenerateRandomNumbers.RandomNumbers(20);
                        var saltedpassword = GenerateHashSha512.Sha512(createAgentViewModel.Password, salt);

                        var savedAssignedRoles = new SavedAssignedRoles()
                        {
                            RoleId = Convert.ToInt16(StatusMain.Roles.Agent),
                            AssignedRoleId = 0,
                            Status = true,
                            CreateDate = DateTime.Now
                        };

                        var result = _iUserMaster.AddAgent(usermaster, saltedpassword, savedAssignedRoles, createAgentViewModel.CategoryId,salt);

                        if (result > 0)
                        {
                            TempData["MessageCreateUsers"] = "Agent Created Successfully";
                            return RedirectToAction("Create", "CreateAgent");
                        }
                        else
                        {
                            return View(createAgentViewModel);
                        }
                    }

                    createAgentViewModel.ListofCategory = _category.GetAllActiveSelectListItemCategory();
                    return View("Create", createAgentViewModel);
                }
                else
                {
                    createAgentViewModel.ListofCategory = _category.GetAllActiveSelectListItemCategory();
                    return View("Create", createAgentViewModel);
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
                    var createUserViewModel = _iUserMaster.EditAgentbyUserId(id);
                    createUserViewModel.CategoryId = _category.GetCategoryIdsByUserId(createUserViewModel.UserId);
                    createUserViewModel.ListofCategory = _category.GetAllActiveSelectListItemCategory();
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
        public ActionResult Edit(EditAgentViewModel editAgentViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var isUser = _iUserMaster.CheckUserIdExists(editAgentViewModel.UserId);
                    if (isUser)
                    {

                        var createUserViewModel = _iUserMaster.EditUserbyUserId(editAgentViewModel.UserId);

                        if (createUserViewModel.EmailId != editAgentViewModel.EmailId)
                        {
                            if (_iUserMaster.CheckEmailIdExists(editAgentViewModel.EmailId))
                            {
                                ModelState.AddModelError("", "EmailId already exists");
                                return View(editAgentViewModel);
                            }
                        }

                        if (createUserViewModel.MobileNo != editAgentViewModel.MobileNo)
                        {
                            if (_iUserMaster.CheckMobileNoExists(editAgentViewModel.MobileNo))
                            {
                                ModelState.AddModelError("", "MobileNo already exists");
                                return View(editAgentViewModel);
                            }
                        }

                        var result = _iUserMaster.UpdateAgentDetails(editAgentViewModel);
                        TempData["MessageUpdateAgent"] = "Agent Details Updated Successfully";
                        editAgentViewModel.ListofCategory = _category.GetAllActiveSelectListItemCategory();
                        return View(editAgentViewModel);
                    }
                    else
                    {
                        editAgentViewModel.ListofCategory = _category.GetAllActiveSelectListItemCategory();
                        TempData["MessageCreateUsersErrors"] = "Agent Details doesn't exist";
                        return View(editAgentViewModel);
                    }
                }
                else
                {
                    editAgentViewModel.ListofCategory = _category.GetAllActiveSelectListItemCategory();
                    return View("Edit", editAgentViewModel);
                }
            }
            catch
            {
                throw;
            }
        }


        public ActionResult AllAgents()
        {
            return View();
        }


        [HttpPost]//Gets the todo Lists.  
        public JsonResult AgentList(string username, int? roleid, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                var agentCount = _iUserMaster.GetAgentCount(username, roleid);
                var listofagent = _iUserMaster.GetAgentList(username, roleid, jtStartIndex, jtPageSize, jtSorting);
                return Json(new { Result = "OK", Records = listofagent, TotalRecordCount = agentCount });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}