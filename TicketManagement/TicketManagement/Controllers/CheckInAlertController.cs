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
    public class CheckInAlertController : Controller
    {
        private readonly IAgentCheckInStatus _agentCheckInStatus;
        readonly SessionHandler _sessionHandler = new SessionHandler();
        public CheckInAlertController(IAgentCheckInStatus staffCheckInStatus)
        {
            _agentCheckInStatus = staffCheckInStatus;
        }
        // GET: CheckInAlert
        public ActionResult Alerts()
        {
            try
            {

                if (!_agentCheckInStatus.CheckIsCategoryAssignedtoAgent(Convert.ToInt64(_sessionHandler.UserId)))
                {
                    TempData["DeskMessage"] = "Category is not Assigned, Please contact your administrator";
                }

                ViewBag.Data = _agentCheckInStatus.AgentDailyActivity(Convert.ToInt64(_sessionHandler.UserId));
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}