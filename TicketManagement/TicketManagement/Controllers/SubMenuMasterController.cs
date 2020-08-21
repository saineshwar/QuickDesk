using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using AutoMapper;
using TicketManagement.CommonData;
using TicketManagement.Filters;
using TicketManagement.Helpers;
using TicketManagement.Interface;
using TicketManagement.Models;
using TicketManagement.ViewModels;


namespace TicketManagement.Controllers
{
    [AuthorizeSuperAdminAttribute]
    public class SubMenuMasterController : Controller
    {
        private readonly ISubMenu _subMenu;
        private readonly IMenu _menu;
        private readonly IRole _role;
        readonly SessionHandler _sessionHandler = new SessionHandler();
        public SubMenuMasterController(ISubMenu subMenu, IMenu menu, IRole role)
        {
            _subMenu = subMenu;
            _menu = menu;
            _role = role;
        }

        // GET: SubMenuMaster
        public ActionResult Index()
        {
            return View();
        }

        // GET: SubMenuMaster/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var subMenu = _subMenu.GetSubMenuById(id);
                var subMenuMaster = AutoMapper.Mapper.Map<SubMenuMasterViewModel>(subMenu);
                var menuMaster = _menu.GetMenuById(subMenuMaster.MenuId);
                subMenuMaster.MenuName = menuMaster.MenuName;
                return View(subMenuMaster);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: SubMenuMaster/Create
        public ActionResult Create()
        {
            try
            {
                SubMenuMasterCreate subMenu = new SubMenuMasterCreate();
                subMenu.MenuList = new List<SelectListItem>()
                {
                    new SelectListItem()
                    {
                        Value = "",
                        Text = "-----Select-----"
                    }
                };
                subMenu.ListofMenuCategory = new List<SelectListItem>()
                {
                    new SelectListItem()
                    {
                        Value = "",
                        Text = "-----Select-----"
                    }
                };
                subMenu.ListofRoles = _role.GetAllActiveRoles();

                return View(subMenu);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SubMenuMasterCreate subMenuMasterVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    SubMenuMaster subMenuMaster = new SubMenuMaster()
                    {
                        SubMenuId = 0,
                        RoleId = subMenuMasterVm.RoleID,
                        CategoryId = subMenuMasterVm.MenuCategoryId,
                        MenuId = subMenuMasterVm.MenuId,
                        Status = subMenuMasterVm.Status,
                        ActionMethod = subMenuMasterVm.ActionMethod,
                        ControllerName = subMenuMasterVm.ControllerName,
                        SubMenuName = subMenuMasterVm.SubMenuName,
                        CreateDate = DateTime.Now
                    };
                    subMenuMaster.UserId = Convert.ToInt32(_sessionHandler.UserId);
                    _subMenu.AddSubMenu(subMenuMaster);
                    return RedirectToAction("Index");
                }

                subMenuMasterVm.MenuList = new List<SelectListItem>()
                {
                    new SelectListItem()
                    {
                        Value = "",
                        Text = "-----Select-----"
                    }
                };

                subMenuMasterVm.ListofMenuCategory = new List<SelectListItem>()
                {
                    new SelectListItem()
                    {
                        Value = "",
                        Text = "-----Select-----"
                    }
                };

                subMenuMasterVm.ListofRoles = _role.GetAllActiveRoles();

                return View(subMenuMasterVm);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: SubMenuMaster/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                var subMenuMaster = _subMenu.GetSubMenuById(id);
                subMenuMaster.MenuList = new List<SelectListItem>()
                {
                    new SelectListItem()
                    {
                        Value = "",
                        Text = "-----Select-----"
                    }
                };
                subMenuMaster.ListofMenuCategory = new List<SelectListItem>()
                {
                    new SelectListItem()
                    {
                        Value = "",
                        Text = "-----Select-----"
                    }
                };
                subMenuMaster.ListofRoles = _role.GetAllActiveRoles();
                return View(subMenuMaster);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // POST: SubMenuMaster/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditSubMenuMaster subMenuMasterVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_subMenu.EditValidationCheck(subMenuMasterVm.SubMenuId, subMenuMasterVm))
                    {
                        SubMenuMaster subMenuMaster = new SubMenuMaster()
                        {
                            SubMenuId = subMenuMasterVm.SubMenuId,
                            RoleId = subMenuMasterVm.RoleID,
                            CategoryId = subMenuMasterVm.MenuCategoryId,
                            MenuId = subMenuMasterVm.MenuId,
                            Status = subMenuMasterVm.Status,
                            ActionMethod = subMenuMasterVm.ActionMethod,
                            ControllerName = subMenuMasterVm.ControllerName,
                            SubMenuName = subMenuMasterVm.SubMenuName,
                            CreateDate = DateTime.Now
                        };
                        subMenuMaster.UserId = Convert.ToInt32(_sessionHandler.UserId);
                        _subMenu.UpdateSubMenu(subMenuMaster);

                        TempData["MenuUpdateMessages"] = CommonMessages.MenuUpdateMessages;
                    }
                    else if (_subMenu.CheckSubMenuNameExists(subMenuMasterVm.SubMenuName, subMenuMasterVm.MenuId,
                        subMenuMasterVm.RoleID, subMenuMasterVm.MenuCategoryId))
                    {
                        ModelState.AddModelError("", CommonMessages.MenuNameAlreadyExistsMessages);
                        subMenuMasterVm.ListofRoles = _role.GetAllActiveRoles();
                        subMenuMasterVm.ListofMenuCategory = new List<SelectListItem>()
                        {
                            new SelectListItem()
                            {
                                Value = "",
                                Text = "-----Select-----"
                            }
                        };
                        subMenuMasterVm.MenuList = new List<SelectListItem>()
                        {
                            new SelectListItem()
                            {
                                Value = "",
                                Text = "-----Select-----"
                            }
                        };
                        return View(subMenuMasterVm);
                    }
                    else
                    {
                        SubMenuMaster subMenuMaster = new SubMenuMaster()
                        {
                            SubMenuId = subMenuMasterVm.SubMenuId,
                            RoleId = subMenuMasterVm.RoleID,
                            CategoryId = subMenuMasterVm.MenuCategoryId,
                            MenuId = subMenuMasterVm.MenuId,
                            Status = subMenuMasterVm.Status,
                            ActionMethod = subMenuMasterVm.ActionMethod,
                            ControllerName = subMenuMasterVm.ControllerName,
                            SubMenuName = subMenuMasterVm.SubMenuName,
                            CreateDate = DateTime.Now
                        };
                        subMenuMaster.UserId = Convert.ToInt32(_sessionHandler.UserId);
                        _subMenu.UpdateSubMenu(subMenuMaster);

                        TempData["MenuUpdateMessages"] = CommonMessages.MenuUpdateMessages;
                    }

                    return RedirectToAction("Index");
                }

                subMenuMasterVm.ListofRoles = _role.GetAllActiveRoles();
                subMenuMasterVm.ListofMenuCategory = new List<SelectListItem>()
                {
                    new SelectListItem()
                    {
                        Value = "",
                        Text = "-----Select-----"
                    }
                };
                subMenuMasterVm.MenuList = new List<SelectListItem>()
                {
                    new SelectListItem()
                    {
                        Value = "",
                        Text = "-----Select-----"
                    }
                };
                return View(subMenuMasterVm);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult CheckSubMenuName(string menuName, int menuId)
        {
            try
            {
                var result = _subMenu.CheckSubMenuNameExists(menuName, menuId);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult LoadallSubMenus()
        {
            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;

                int recordsTotal = 0;

                var rolesData = _subMenu.ShowAllSubMenus(sortColumn, sortColumnDir, searchValue);
                recordsTotal = rolesData.Count();
                var data = rolesData.Skip(skip).Take(pageSize).ToList();

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult GetMenus(RequestMenus requestMenus)
        {
            var listofMenus = _menu.ListofMenubyRoleId(requestMenus);
            return Json(listofMenus, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteSubMenu(RequestDeleteSubMenu request)
        {
            try
            {
                var result = _subMenu.DeleteSubMenu(request.SubMenuId);
                return Json(new { Result = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Result = "failed", Message = "Cannot Delete" });
            }
        }
    }
}
