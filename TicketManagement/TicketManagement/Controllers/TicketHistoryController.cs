using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using TicketManagement.Filters;
using TicketManagement.Interface;
using TicketManagement.ViewModels;

namespace TicketManagement.Controllers
{
    [AuthorizeCommonFormUsersAttribute]
    public class TicketHistoryController : Controller
    {
        private readonly ITicketHistory _ticketHistory;
        public TicketHistoryController(ITicketHistory ticketHistory)
        {
            _ticketHistory = ticketHistory;
        }

        //TicketHistory 
        public ActionResult Activities(TicketHistoryRequest ticketHistoryRequest)
        {
            if (!string.IsNullOrEmpty(ticketHistoryRequest.Ticketid))
            {
                var data = _ticketHistory.ListofTicketHistorybyTicket(Convert.ToInt64(ticketHistoryRequest.Ticketid));
                return PartialView("_TicketHistory", data);
            }
            else
            {
                return null;
            }
        }
    }
}