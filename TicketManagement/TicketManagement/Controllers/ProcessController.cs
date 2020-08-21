using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketManagement.Filters;
using TicketManagement.Helpers;
using TicketManagement.Interface;

namespace TicketManagement.Controllers
{
    [AuthorizeAgent]
    public class ProcessController : Controller
    {
        private readonly IAgentCheckInStatus _agentCheckInStatus;
        readonly SessionHandler _sessionHandler = new SessionHandler();
        public ProcessController(IAgentCheckInStatus agentCheckInStatus)
        {
            _agentCheckInStatus = agentCheckInStatus;
        }

        // GET: Process
        [HttpPost]
        public ActionResult InProcess(string process)
        {
            try
            {
                _agentCheckInStatus.StatusCheckInCheckOut(Convert.ToInt64(_sessionHandler.UserId));
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult OutProcess(string process)
        {
            try
            {
                _agentCheckInStatus.StatusCheckInCheckOut(Convert.ToInt64(_sessionHandler.UserId));
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AutoOutProcess()
        {
            try
            {
                _agentCheckInStatus.StatusCheckInCheckOut(Convert.ToInt64(_sessionHandler.UserId));
                return RedirectToAction("Alerts", "CheckInAlert");
            }
            catch (Exception)
            {
                return RedirectToAction("Logout", "Login");
            }
        }

    }
}