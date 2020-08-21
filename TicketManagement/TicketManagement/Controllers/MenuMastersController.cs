using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TicketManagement.CommonData;
using TicketManagement.Filters;
using TicketManagement.Helpers;
using TicketManagement.Interface;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Controllers
{
    [AuthorizeSuperAdminAttribute]
    public class MenuMastersController : Controller
    {
        private readonly IMenu _iMenu;
        private readonly IRole _role;
      
        private readonly IMenuCategory _menuCategory;
        readonly SessionHandler _sessionHandler = new SessionHandler();
        public MenuMastersController(IMenu menu, IRole role, IMenuCategory menuCategory)
        {
            _iMenu = menu;
            _role = role;
            _menuCategory = menuCategory;
        }

        // GET: MenuMasters
        public ActionResult Index()
        {
            return View();
        }

        // GET: MenuMasters/Create
        public ActionResult Create()
        {
            MenuMasterViewModel menuMasterViewModel = new MenuMasterViewModel()
            {
                AllActiveRoles = _role.GetAllActiveRoles(),
                ListofMenuCategory = new List<SelectListItem>()
                {
                    new SelectListItem()
                    {
                        Value = "",
                        Text = "-----Select-----"
                    }
                }
            };
            return View(menuMasterViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MenuMasterViewModel menuvm)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (_iMenu.CheckMenuNameExists(menuvm.MenuName, menuvm.RoleId, menuvm.MenuCategoryId))
                    {
                        ModelState.AddModelError("", CommonMessages.MenuNameAlreadyExistsMessages);
                        menuvm.AllActiveRoles = _role.GetAllActiveRoles();
                        menuvm.ListofMenuCategory = new List<SelectListItem>()
                        {
                            new SelectListItem()
                            {
                                Value = "",
                                Text = "-----Select-----"
                            }
                        };
                        return View(menuvm);
                    }

                    MenuMaster menuMaster = new MenuMaster()
                    {
                        UserId = Convert.ToInt64(_sessionHandler.UserId),
                        MenuName = menuvm.MenuName,
                        Status = menuvm.Status,
                        ActionMethod = menuvm.ActionMethod,
                        ControllerName = menuvm.ControllerName,
                        MenuId = 0,
                        CategoryId = menuvm.MenuCategoryId,
                        RoleId = menuvm.RoleId,
                        CreateDate = DateTime.Now
                    };

                    _iMenu.AddMenu(menuMaster);
                    TempData["MenuSuccessMessages"] = CommonMessages.MenuSuccessMessages;
                    return RedirectToAction("Create");
                }

                menuvm.AllActiveRoles = _role.GetAllActiveRoles();
                menuvm.ListofMenuCategory = new List<SelectListItem>()
                {
                    new SelectListItem()
                    {
                        Value = "",
                        Text = "-----Select-----"
                    }
                };
                return View(menuvm);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: MenuMasters/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                var menuMaster = _iMenu.GetMenuById(id);
                menuMaster.AllActiveRoles = _role.GetAllActiveRoles();
                menuMaster.ListofMenuCategory = new List<SelectListItem>()
                {
                    new SelectListItem()
                    {
                        Value = "",
                        Text = "-----Select-----"
                    }
                };
                return View(menuMaster);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditMenuMasterViewModel editmenuMaster)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_iMenu.EditValidationCheck(editmenuMaster.MenuId, editmenuMaster))
                    {
                        MenuMaster menuMaster = new MenuMaster()
                        {
                            UserId = Convert.ToInt64(_sessionHandler.UserId),
                            MenuName = editmenuMaster.MenuName,
                            Status = editmenuMaster.Status,
                            ActionMethod = editmenuMaster.ActionMethod,
                            ControllerName = editmenuMaster.ControllerName,
                            MenuId = editmenuMaster.MenuId,
                            CategoryId = editmenuMaster.MenuCategoryId,
                            RoleId = editmenuMaster.RoleId,
                            ModifiedDate = DateTime.Now
                        };

                        _iMenu.UpdateMenu(menuMaster);

                        TempData["MenuUpdateMessages"] = CommonMessages.MenuUpdateMessages;
                    }
                    else if (_iMenu.CheckMenuNameExists(editmenuMaster.MenuName, editmenuMaster.RoleId, editmenuMaster.MenuCategoryId))
                    {

                        ModelState.AddModelError("", CommonMessages.MenuNameAlreadyExistsMessages);
                        editmenuMaster.AllActiveRoles = _role.GetAllActiveRoles();
                        editmenuMaster.ListofMenuCategory = new List<SelectListItem>()
                        {
                            new SelectListItem()
                            {
                                Value = "",
                                Text = "-----Select-----"
                            }
                        };
                        return View(editmenuMaster);
                    }
                    else
                    {
                        MenuMaster menuMaster = new MenuMaster()
                        {
                            UserId = Convert.ToInt64(_sessionHandler.UserId),
                            MenuName = editmenuMaster.MenuName,
                            Status = editmenuMaster.Status,
                            ActionMethod = editmenuMaster.ActionMethod,
                            ControllerName = editmenuMaster.ControllerName,
                            MenuId = editmenuMaster.MenuId,
                            CategoryId = editmenuMaster.MenuCategoryId,
                            RoleId = editmenuMaster.RoleId,
                            ModifiedDate = DateTime.Now
                        };

                        _iMenu.UpdateMenu(menuMaster);

                        TempData["MenuUpdateMessages"] = CommonMessages.MenuUpdateMessages;
                    }
                    editmenuMaster.AllActiveRoles = _role.GetAllActiveRoles();
                    editmenuMaster.ListofMenuCategory = new List<SelectListItem>()
                    {
                        new SelectListItem()
                        {
                            Value = "",
                            Text = "-----Select-----"
                        }
                    };
                    return View(editmenuMaster);
                }
                editmenuMaster.AllActiveRoles = _role.GetAllActiveRoles();
                editmenuMaster.ListofMenuCategory = new List<SelectListItem>()
                {
                    new SelectListItem()
                    {
                        Value = "",
                        Text = "-----Select-----"
                    }
                };
                return View(editmenuMaster);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: MenuMasters/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                MenuViewModel menuMaster = _iMenu.GetMenutoDeleteById(id);
                if (menuMaster == null)
                {
                    return HttpNotFound();
                }
                return View(menuMaster);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // POST: MenuMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                _iMenu.DeleteMenu(id);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }
        }


        public ActionResult LoadallMenus()
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

                var rolesData = _iMenu.ShowAllMenus(sortColumn, sortColumnDir, searchValue);
                recordsTotal = rolesData.Count();
                var data = rolesData.Skip(skip).Take(pageSize).ToList();

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult GetCategory(RequestCategory requestCategory)
        {
            var listofCategory = _menuCategory.GetCategorybyRoleId(requestCategory.RoleID);
            return Json(listofCategory, JsonRequestBehavior.AllowGet);
        }
    }
}
