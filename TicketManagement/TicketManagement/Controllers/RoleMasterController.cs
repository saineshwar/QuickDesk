using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TicketManagement.Filters;
using TicketManagement.Helpers;
using TicketManagement.Interface;
using TicketManagement.Models;


namespace TicketManagement.Controllers
{
    [AuthorizeSuperAdminAttribute]
    public class RoleMasterController : Controller
    {
        private readonly IRole _iRole;
        SessionHandler _sessionHandler = new SessionHandler();
        public RoleMasterController(IRole IRole)
        {
            _iRole = IRole;
        }


        // GET: RoleMaster
        public ActionResult Index()
        {
            try
            {
                return View(_iRole.GetAllRoleMaster());
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: RoleMaster/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleMaster roleMaster = _iRole.GetRoleMasterById(id);
            if (roleMaster == null)
            {
                return HttpNotFound();
            }
            return View(roleMaster);
        }

        // GET: RoleMaster/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoleMaster/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoleId,RoleName,Status")] RoleMaster roleMaster)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    roleMaster.UserId = Convert.ToInt32(_sessionHandler.UserId);
                    _iRole.AddRoleMaster(roleMaster);
                    return RedirectToAction("Index");
                }

                return View(roleMaster);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: RoleMaster/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                RoleMaster roleMaster = _iRole.GetRoleMasterById(id);
                if (roleMaster == null)
                {
                    return HttpNotFound();
                }
                return View(roleMaster);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // POST: RoleMaster/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoleId,RoleName,Status,CreateDate")] RoleMaster roleMaster)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    roleMaster.UserId = Convert.ToInt32(_sessionHandler.UserId);
                    _iRole.UpdateRoleMaster(roleMaster);
                    return RedirectToAction("Index");
                }
                return View(roleMaster);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: RoleMaster/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                RoleMaster roleMaster = _iRole.GetRoleMasterById(id);
                if (roleMaster == null)
                {
                    return HttpNotFound();
                }
                return View(roleMaster);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // POST: RoleMaster/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                _iRole.DeleteRoleMaster(id);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult LoadallRoles()
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

                var rolesData = _iRole.ShowAllRoleMaster(sortColumn, sortColumnDir, searchValue);
                recordsTotal = rolesData.Count();
                var data = rolesData.Skip(skip).Take(pageSize).ToList();

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception)
            {
                throw;
            }
        }
        public JsonResult CheckRoleName(string rolename)
        {
            try
            {
                var result = _iRole.CheckRoleMasterNameExists(rolename);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
