using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketManagement.Filters;
using TicketManagement.Interface;

namespace TicketManagement.Controllers
{
    [AuthorizeSuperAdminAttribute]
    public class AllRolesController : Controller
    {
        private readonly IRole _role;
        // GET: AllRoles

        public AllRolesController(IRole role)
        {
            _role = role;
        }

        public ActionResult GetAllRoles()
        {
            try
            {
                var result = new SelectList(_role.GetAllActiveRoles(), "Value", "Text");
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult GetAllActiveRolesNotAgentRole()
        {
            try
            {
                var result = new SelectList(_role.GetAllActiveRolesNotAgent(), "Value", "Text");
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult GetAllActiveRolesAgent()
        {
            try
            {
                var result = new SelectList(_role.GetAllActiveRolesAgent(), "Value", "Text");
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}