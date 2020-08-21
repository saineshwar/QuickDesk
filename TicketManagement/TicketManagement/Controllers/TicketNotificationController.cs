using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketManagement.Filters;
using TicketManagement.Helpers;
using TicketManagement.Interface;
using TicketManagement.ViewModels;

namespace TicketManagement.Controllers
{
    [AuthorizeAgent]
    [AgentCheckIn]
    public class TicketNotificationController : Controller
    {
        private readonly ITicketNotification _iTicketNotification;
        readonly SessionHandler _sessionHandler = new SessionHandler();
        public TicketNotificationController(ITicketNotification ticketNotification)
        {
            _iTicketNotification = ticketNotification;
        }

        [HttpPost]
        // GET: TicketNotification
        public ActionResult UpdateNotification(RequestNotificationViewModel request)
        {
            _iTicketNotification.UpdateTicketNotificationasRead(Convert.ToInt64(_sessionHandler.UserId), request.NotificationId);
            return Json(new { status = "Success" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Notification()
        {
            var notificationList = _iTicketNotification.ListofNotification(Convert.ToInt64(_sessionHandler.UserId));
            return View(notificationList);
        }
    }
}