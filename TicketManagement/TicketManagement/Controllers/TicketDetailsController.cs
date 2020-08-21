using System;
using System.Collections.Generic;
using System.IO;
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
    [AuthorizeUser]
    public class TicketDetailsController : Controller
    {
        private readonly IDisplayTickets _displayTickets;
        private readonly IPriority _priority;
        private readonly IStatus _status;
        private readonly ITicketsReply _ticketsReply;
        private readonly IAttachments _attachments;
        private readonly ITickets _tickets;
        private readonly ITicketHistory _ticketHistory;
        SessionHandler _sessionHandler = new SessionHandler();
        public TicketDetailsController(IDisplayTickets displayTickets, IPriority priority, IStatus status, ITicketsReply ticketsReply, IAttachments attachments, ITickets tickets, ITicketHistory ticketHistory)
        {
            _displayTickets = displayTickets;
            _priority = priority;
            _status = status;
            _ticketsReply = ticketsReply;
            _attachments = attachments;
            _tickets = tickets;
            _ticketHistory = ticketHistory;
        }


        // GET: TicketDetails
        public ActionResult Details(string trackingId)
        {
            try
            {
                if (!string.IsNullOrEmpty(trackingId))
                {
                    if (!_displayTickets.CheckTrackingIdExists(trackingId))
                    {
                        return RedirectToAction("Dashboard", "UserDashboard");
                    }

                    var ticket = _displayTickets.TicketsDetailsbyticketId(trackingId);
                    ticket.ListofPriority = _priority.GetAllPrioritySelectListItem();
                    ticket.ListofStatus = _status.GetAllStatusSelectListItem();
                    ticket.TicketLockStatus = _tickets.GetTicketLockStatus(ticket.TicketId);
                    ticket.TicketReply = new TicketReplyModel()
                    {
                        Message = string.Empty,
                        TicketId = ticket.TicketId,
                        TrackingId = ticket.TrackingId,
                    };
                    var listofTicketreply = _ticketsReply.ListofHistoryTicketReplies(trackingId);
                    if (listofTicketreply != null)
                    {
                        ticket.ViewMainModel = new ViewTicketReplyMainModel();
                        ticket.ViewMainModel.ListofReplyAttachment = _attachments.GetListReplyAttachmentsByAttachmentId(ticket.TicketId);
                        ticket.ViewMainModel.ListofTicketreply = listofTicketreply;
                        ticket.ListofAttachments = ticket.ListofAttachments = _attachments.GetListAttachmentsByAttachmentId(ticket.TicketId);
                    }
                    else
                    {
                        ticket.ViewMainModel = null;
                    }
                    return View(ticket);
                }

                return RedirectToAction("Dashboard", "UserDashboard");
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult Details(TicketReplyModel ticketReplyModel, string replied)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    var statusId = 0;

                    if (!string.IsNullOrEmpty(replied))
                    {
                        statusId = (int)StatusMain.Status.Replied;
                    }



                    var file = Request.Files;
                    ReplyAttachment replyAttachment = new ReplyAttachment();
                    ReplyAttachmentDetails replyAttachmentDetails = new ReplyAttachmentDetails();

                    // ReSharper disable once CollectionNeverQueried.Local
                    var listofattachments = new List<ReplyAttachment>();
                    // ReSharper disable once CollectionNeverQueried.Local
                    var listofattachmentdetails = new List<ReplyAttachmentDetails>();

                    for (int i = 0; i <= file.Count - 1; i++)
                    {
                        if (file[i] != null && file[i].ContentLength > 0)
                        {
                            string extension = Path.GetExtension(file[i].FileName);
                            replyAttachment.TicketUser = Convert.ToInt64(_sessionHandler.UserId);
                            replyAttachment.AttachmentName = file[i].FileName;
                            replyAttachment.AttachmentType = extension;
                            replyAttachment.CreatedDate = DateTime.Now;
                            var inputStream = file[i].InputStream;
                            if (inputStream != null)
                                using (var binaryReader = new BinaryReader(inputStream))
                                {
                                    byte[] fileSize = binaryReader.ReadBytes(count: file[i].ContentLength);
                                    replyAttachmentDetails.AttachmentBytes = fileSize;
                                }

                            listofattachments.Add(replyAttachment);
                            listofattachmentdetails.Add(replyAttachmentDetails);
                        }
                    }




                    var userId = Convert.ToInt64(_sessionHandler.UserId);

                    var message = AppendString(ticketReplyModel.Message);
                    ticketReplyModel.Message = message;

                    _ticketsReply.ReplyTicket(ticketReplyModel, listofattachments, listofattachmentdetails, userId,null, statusId);

                    var ticket = _displayTickets.TicketsDetailsbyticketId(ticketReplyModel.TrackingId);
                    ticket.ListofPriority = _priority.GetAllPrioritySelectListItem();
                    ticket.ListofStatus = _status.GetAllStatusSelectListItem();
                    ticket.TicketReply = new TicketReplyModel()
                    {
                        Message = string.Empty,
                        TicketId = ticket.TicketId,
                        TrackingId = ticket.TrackingId
                    };

                    var listofTicketreply = _ticketsReply.ListofHistoryTicketReplies(ticketReplyModel.TrackingId);

                    if (listofTicketreply != null)
                    {
                        ticket.ListofAttachments = _attachments.GetListAttachmentsByAttachmentId(ticketReplyModel.TicketId);
                        ticket.ViewMainModel = new ViewTicketReplyMainModel()
                        {
                            ListofReplyAttachment = _attachments.GetListReplyAttachmentsByAttachmentId(ticketReplyModel.TicketId),
                            ListofTicketreply = listofTicketreply
                        };
                    }
                    else
                    {
                        ticket.ListofAttachments = _attachments.GetListAttachmentsByAttachmentId(ticketReplyModel.TicketId) ??
                                                   new List<Attachments>();

                        ticket.ViewMainModel = new ViewTicketReplyMainModel()
                        {
                            ListofTicketreply = new List<ViewTicketReplyHistoryModel>(),
                            ListofReplyAttachment = new List<ReplyAttachment>()
                        };
                    }

                    TicketHistory(ticketReplyModel, replied);
                    TempData["TicketReplyMessage"] = CommonMessages.TicketSuccessReplyMessages;

                    return RedirectToAction("Details", "TicketDetails", new { trackingId = ticket.TrackingId });

                }
                else
                {
                    var ticket = _displayTickets.TicketsDetailsbyticketId(ticketReplyModel.TrackingId);
                    ticket.ListofPriority = _priority.GetAllPrioritySelectListItem();
                    ticket.ListofStatus = _status.GetAllStatusSelectListItem();
                    ticket.ListofAttachments = _attachments.GetListAttachmentsByAttachmentId(ticketReplyModel.TicketId) ??
                                               new List<Attachments>();
                    ticket.TicketReply = new TicketReplyModel()
                    {
                        Message = string.Empty,
                        TicketId = ticket.TicketId,
                        TrackingId = ticket.TrackingId
                    };
                    var listofTicketreply = _ticketsReply.ListofHistoryTicketReplies(ticketReplyModel.TrackingId);

                    if (listofTicketreply != null)
                    {
                        ticket.ViewMainModel = new ViewTicketReplyMainModel()
                        {
                            ListofReplyAttachment = _attachments.GetListReplyAttachmentsByAttachmentId(ticketReplyModel.TicketId),
                            ListofTicketreply = listofTicketreply
                        };
                    }
                    else
                    {
                        ticket.ViewMainModel = null;
                    }

                    TempData["TicketReplyMessage"] = CommonMessages.TicketErrorReplyMessages;
                    return View(ticket);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
     
        public ActionResult DownloadReplyAttachMent(long ticketReplyId)
        {
            try
            {
                if (!string.IsNullOrEmpty(Convert.ToString(ticketReplyId)))
                {
                    var document = _attachments.GetReplyAttachmentsByAttachmentId(ticketReplyId);
                    if (document != null)
                    {
                        var documentdetail = _attachments.GetReplyAttachmentDetailsByAttachmentId(document.ReplyAttachmentId);
                        return File(documentdetail.AttachmentBytes, System.Net.Mime.MediaTypeNames.Application.Octet, document.AttachmentName);
                    }

                    return RedirectToAction("Dashboard", "UserDashboard");
                }
                else
                {
                    return RedirectToAction("Dashboard", "UserDashboard");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private string AppendString(string message)
        {
            try
            {
                var appendedmessage = WebUtility.HtmlDecode(message);
                return appendedmessage;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult DownloadAttachMent(long attachmentId)
        {
            try
            {
                if (!string.IsNullOrEmpty(Convert.ToString(attachmentId)))
                {
                    var document = _attachments.GetAttachments(attachmentId);
                    if (document != null)
                    {
                        var documentdetail = _attachments.GetAttachmentDetailsByAttachmentId(document.AttachmentId);
                        return File(documentdetail.AttachmentBytes, System.Net.Mime.MediaTypeNames.Application.Octet, document.AttachmentName);
                    }

                    return RedirectToAction("Dashboard", "UserDashboard");
                }
                else
                {
                    return RedirectToAction("Dashboard", "UserDashboard");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult DeleteAttachment(RequestAttachments requestAttachments)
        {
            try
            {
                var result = _attachments.DeleteAttachmentByAttachmentId(requestAttachments.AttachmentsId);
                if (result > 0)
                {
                    TicketHistoryHelper ticketHistoryHelper = new TicketHistoryHelper();
                    TicketHistory ticketHistory = new TicketHistory
                    {
                        UserId = Convert.ToInt32(_sessionHandler.UserId),
                        Message = ticketHistoryHelper.DeleteTicketAttachment(),
                        ProcessDate = DateTime.Now,
                        TicketId = requestAttachments.TicketId,
                        ActivitiesId = Convert.ToInt16(StatusMain.Activities.DeletedAttachment)
                    };
                    _ticketHistory.TicketHistory(ticketHistory);

                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void TicketHistory(TicketReplyModel ticketReplyModel, string replied)
        {
            try
            {
                string message = string.Empty;

                TicketHistoryHelper ticketHistoryHelper = new TicketHistoryHelper();
                var ticketHistory = new TicketHistory();
                if (!string.IsNullOrEmpty(replied))
                {
                    message = ticketHistoryHelper.ReplyMessage(Convert.ToInt16(StatusMain.Status.Replied));
                    ticketHistory.StatusId = Convert.ToInt16(StatusMain.Status.Replied);
                }

                TempData["TicketReplyMessage"] = CommonMessages.TicketSuccessReplyMessages;

                ticketHistory.UserId = Convert.ToInt32(_sessionHandler.UserId);
                ticketHistory.Message = message;
                ticketHistory.ProcessDate = DateTime.Now;
                ticketHistory.TicketId = ticketReplyModel.TicketId;
                ticketHistory.ActivitiesId = Convert.ToInt16(StatusMain.Activities.Replied);
                _ticketHistory.TicketHistory(ticketHistory);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}