using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketManagement.Filters;
using TicketManagement.Interface;
using TicketManagement.ViewModels;

namespace TicketManagement.Controllers
{
    [SessionCheck]
    public class PrintController : Controller
    {
        private readonly IDisplayTickets _displayTickets;
        private readonly ITicketsReply _ticketsReply;

        public PrintController(IDisplayTickets displayTickets, ITicketsReply ticketsReply)
        {
            _displayTickets = displayTickets;
            _ticketsReply = ticketsReply;
        }

        // GET: Print
        public ActionResult Ticket(string trackingId)
        {
            try
            {
                if (!string.IsNullOrEmpty(trackingId))
                {
                    TempData["Print_TrackingId"] = trackingId;
                    return RedirectToAction("Print", "Print");
                }
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult Print()
        {
            try
            {
                if (!string.IsNullOrEmpty(Convert.ToString(TempData["Print_TrackingId"])))
                {
                    var ticket = _displayTickets.TicketsDetailsbyticketId(Convert.ToString(TempData["Print_TrackingId"]));
                    ticket.TicketReply = new TicketReplyModel();
                    var listofTicketreply = _ticketsReply.ListofHistoryTicketReplies(Convert.ToString(TempData["Print_TrackingId"]));
                    if (listofTicketreply != null)
                    {
                        ticket.ViewMainModel = new ViewTicketReplyMainModel();
                        ticket.ViewMainModel.ListofTicketreply = listofTicketreply;
                    }
                    else
                    {
                        ticket.ViewMainModel = null;
                    }

                    return View(ticket);
                }
                else
                {
                    return View(new DisplayTicketViewModel() { TrackingId = string.Empty });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
