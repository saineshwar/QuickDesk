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
    [AuthorizeAgentsAdmin]
    public class TicketDetailsAgentsAdminController : Controller
    {
        // GET: TicketDetailsAgentsAdmin
        private readonly IDisplayTickets _displayTickets;
        private readonly IPriority _priority;
        private readonly IStatus _status;
        private readonly ITicketsReply _ticketsReply;
        private readonly IAttachments _attachments;
        private readonly ITicketHistory _ticketHistory;
        private readonly ITickets _tickets;
        private readonly IProfile _profile;
        private readonly ICategory _category;
        private readonly IUserMaster _userMaster;

        SessionHandler _sessionHandler = new SessionHandler();
        public TicketDetailsAgentsAdminController(IDisplayTickets displayTickets,
            IPriority priority,
            IStatus status,
            ITicketsReply ticketsReply,
            IAttachments attachments,
            ITicketHistory ticketHistory,
            ITickets tickets,
            IProfile profile,
            ICategory category,
            IUserMaster userMaster)
        {
            _displayTickets = displayTickets;
            _priority = priority;
            _status = status;
            _ticketsReply = ticketsReply;
            _attachments = attachments;
            _ticketHistory = ticketHistory;
            _tickets = tickets;
            _profile = profile;
            _category = category;
            _userMaster = userMaster;
        }
        // GET: TicketDetailsAgentsAdmin
        public ActionResult Details(string trackingId)
        {
            try
            {
                if (!string.IsNullOrEmpty(trackingId))
                {
                    if (!_displayTickets.CheckTrackingIdExists(trackingId))
                    {
                        return RedirectToAction("Dashboard", "AgentAdminDashboard");
                    }

                    var ticket = _displayTickets.TicketsDetailsbyticketId(trackingId);
                    ticket.EscalatedUser = _userMaster.GetTicketEscalatedToUserNames(ticket.TicketId);
                    ticket.ListofPriority = _priority.GetAllPrioritySelectListItem();
                    ticket.ListofStatus = _status.GetAllStatusWithoutOverdueandEscalationSelectListItem();
                    ticket.ListofCategory = _category.GetAllActiveSelectListItemCategory();
                    ticket.TicketLockStatus = _tickets.GetTicketLockStatus(ticket.TicketId);
                    ticket.CategoryId = ticket.CategoryId;
                    ticket.TicketReply = new TicketReplyModel()
                    {
                        Message = string.Empty,
                        TicketId = ticket.TicketId,
                        TrackingId = ticket.TrackingId,
                    };
                    var listofTicketreply = _ticketsReply.ListofHistoryTicketReplies(trackingId);
                    if (listofTicketreply != null)
                    {
                        ticket.ViewMainModel = new ViewTicketReplyMainModel()
                        {
                            ListofReplyAttachment = _attachments.GetListReplyAttachmentsByAttachmentId(ticket.TicketId),
                            ListofTicketreply = listofTicketreply
                        };
                        ticket.ListofAttachments = ticket.ListofAttachments = _attachments.GetListAttachmentsByAttachmentId(ticket.TicketId);
                    }
                    else
                    {
                        ticket.ViewMainModel = null;
                    }
                    return View(ticket);
                }

                return RedirectToAction("Dashboard", "AgentAdminDashboard");
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult Details(TicketReplyModel ticketReplyModel, string resolved, string inprogress, string onHold, string replied)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var file = Request.Files;
                    var statusId = 0;

                    if (!string.IsNullOrEmpty(resolved))
                    {
                        statusId = (int)StatusMain.Status.Resolved;
                    }
                    else if (!string.IsNullOrEmpty(inprogress))
                    {
                        statusId = (int)StatusMain.Status.InProgress;
                    }
                    else if (!string.IsNullOrEmpty(onHold))
                    {
                        statusId = (int)StatusMain.Status.OnHold;
                    }
                    else if (!string.IsNullOrEmpty(replied))
                    {
                        statusId = (int)StatusMain.Status.Replied;
                    }

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
                            replyAttachment.SystemUser = Convert.ToInt64(_sessionHandler.UserId);
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

                    _ticketsReply.ReplyTicket(ticketReplyModel, listofattachments, listofattachmentdetails, null, userId, statusId);

                    var ticket = _displayTickets.TicketsDetailsbyticketId(ticketReplyModel.TrackingId);
                    ticket.ListofPriority = _priority.GetAllPrioritySelectListItem();
                    ticket.ListofStatus = _status.GetAllStatusWithoutOverdueandEscalationSelectListItem();
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
                            ListofReplyAttachment = _attachments.GetListReplyAttachmentsByAttachmentId(ticketReplyModel.TicketId)
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

                    TicketHistory(ticketReplyModel, resolved, inprogress, onHold, replied);
                    TempData["TicketReplyMessage"] = CommonMessages.TicketSuccessReplyMessages;

                    return RedirectToAction("Details", "TicketDetailsAgentsAdmin", new { trackingId = ticket.TrackingId });

                }
                else
                {
                    var ticket = _displayTickets.TicketsDetailsbyticketId(ticketReplyModel.TrackingId);
                    ticket.ListofPriority = _priority.GetAllPrioritySelectListItem();
                    ticket.ListofStatus = _status.GetAllStatusWithoutOverdueandEscalationSelectListItem();
                    ticket.EscalatedUser = _userMaster.GetTicketEscalatedToUserNames(ticket.TicketId);
                    ticket.ListofCategory = _category.GetAllActiveSelectListItemCategory();
                    ticket.TicketReply = new TicketReplyModel()
                    {
                        Message = string.Empty,
                        TicketId = ticket.TicketId,
                        TrackingId = ticket.TrackingId
                    };
                    var listofTicketreply = _ticketsReply.ListofHistoryTicketReplies(ticketReplyModel.TrackingId);

                    if (listofTicketreply.Count > 0)
                    {
                        ticket.ListofAttachments = _attachments.GetListAttachmentsByAttachmentId(ticketReplyModel.TicketId) ??
                                                   new List<Attachments>();

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

                    return RedirectToAction("Dashboard", "AgentAdminDashboard");
                }
                else
                {
                    return RedirectToAction("Dashboard", "AgentAdminDashboard");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void TicketHistory(TicketReplyModel ticketReplyModel, string resolved, string inprogress, string onHold, string replied)
        {
            try
            {
                string message = string.Empty;

                TicketHistoryHelper ticketHistoryHelper = new TicketHistoryHelper();
                var ticketHistory = new TicketHistory();
                if (!string.IsNullOrEmpty(resolved))
                {
                    message = ticketHistoryHelper.ReplyMessage(Convert.ToInt16(StatusMain.Status.Resolved));
                    ticketHistory.StatusId = Convert.ToInt16(StatusMain.Status.Resolved);
                }
                else if (!string.IsNullOrEmpty(inprogress))
                {
                    message = ticketHistoryHelper.ReplyMessage(Convert.ToInt16(StatusMain.Status.InProgress));
                    ticketHistory.StatusId = Convert.ToInt16(StatusMain.Status.InProgress);
                }
                else if (!string.IsNullOrEmpty(onHold))
                {
                    message = ticketHistoryHelper.ReplyMessage(Convert.ToInt16(StatusMain.Status.OnHold));
                    ticketHistory.StatusId = Convert.ToInt16(StatusMain.Status.OnHold);
                }
                else if (!string.IsNullOrEmpty(replied))
                {
                    message = ticketHistoryHelper.ReplyMessage(Convert.ToInt16(StatusMain.Status.Replied));
                    ticketHistory.StatusId = Convert.ToInt16(StatusMain.Status.Replied);
                }

                TempData["TicketReplyMessage"] = CommonMessages.TicketSuccessReplyMessages;

                ticketHistory.UserId = Convert.ToInt32(_sessionHandler.UserId);
                ticketHistory.Message = message;
                ticketHistory.ProcessDate = DateTime.Now;
                ticketHistory.TicketId = ticketReplyModel.TicketId;
                ticketHistory.ActivitiesId = Convert.ToInt16(StatusMain.Activities.RepliedandChangedStatus);
                _ticketHistory.TicketHistory(ticketHistory);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public JsonResult ChangePriority(ChangePriorityRequestModel changePriorityRequestModel)
        {
            try
            {
                var result = _tickets.ChangeTicketPriority(changePriorityRequestModel);
                if (result)
                {
                    TicketHistoryHelper ticketHistoryHelper = new TicketHistoryHelper();
                    TicketHistory ticketHistory = new TicketHistory
                    {
                        UserId = Convert.ToInt32(_sessionHandler.UserId),
                        Message = ticketHistoryHelper.PriorityMessage(changePriorityRequestModel.PriorityId),
                        ProcessDate = DateTime.Now,
                        TicketId = changePriorityRequestModel.TicketId,
                        PriorityId = changePriorityRequestModel.PriorityId,
                        ActivitiesId = Convert.ToInt16(StatusMain.Activities.PriorityChanged)
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

        public JsonResult ChangeStatus(ChangeStatusRequestModel changeStatusRequestModel)
        {
            try
            {
                var result = _tickets.ChangeTicketStatus(changeStatusRequestModel);
                if (result)
                {
                    TicketHistoryHelper ticketHistoryHelper = new TicketHistoryHelper();
                    TicketHistory ticketHistory = new TicketHistory
                    {
                        UserId = Convert.ToInt32(_sessionHandler.UserId),
                        Message = ticketHistoryHelper.StatusMessage(changeStatusRequestModel.StatusId),
                        ProcessDate = DateTime.Now,
                        TicketId = changeStatusRequestModel.TicketId,
                        StatusId = changeStatusRequestModel.StatusId,
                        ActivitiesId = Convert.ToInt16(StatusMain.Activities.StatusChanged)
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

        public JsonResult ChangeCategory(ChangeCategoryRequestModel changeStatusRequestModel)
        {
            try
            {
                var result = _tickets.ChangeTicketCategory(changeStatusRequestModel);
                if (result)
                {
                    TicketHistoryHelper ticketHistoryHelper = new TicketHistoryHelper();
                    TicketHistory ticketHistory = new TicketHistory
                    {
                        UserId = Convert.ToInt32(_sessionHandler.UserId),
                        Message = ticketHistoryHelper.CategoryMessage(changeStatusRequestModel.CategoryId),
                        ProcessDate = DateTime.Now,
                        TicketId = changeStatusRequestModel.TicketId,
                        CategoryId = changeStatusRequestModel.CategoryId,
                        ActivitiesId = Convert.ToInt16(StatusMain.Activities.CategoryChanged)
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

        private string AppendString(string message)
        {
            try
            {
                var appendedmessage = WebUtility.HtmlDecode(message)
                                         + Environment.NewLine +
                                         Environment.NewLine +
                                         _profile.GetSignature(Convert.ToInt64(_sessionHandler.UserId));
                return appendedmessage;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult Delete(RequestTicketReplyDeleteViewModel model)
        {
            try
            {
                if (model.TicketId != null && model.TicketReplyId != null)
                {
                    var result = _ticketsReply.DeleteTicketReply(model.TicketReplyId, model.TicketId, Convert.ToInt32(_sessionHandler.UserId));

                    if (result > 0)
                    {
                        TicketHistoryHelper ticketHistoryHelper = new TicketHistoryHelper();
                        var ticketHistory = new TicketHistory();
                        ticketHistory.UserId = Convert.ToInt32(_sessionHandler.UserId);
                        ticketHistory.Message = ticketHistoryHelper.DeleteTicketReplyMessage();
                        ticketHistory.ProcessDate = DateTime.Now;
                        ticketHistory.TicketId = model.TicketId;
                        ticketHistory.TicketReplyId = model.TicketReplyId;
                        ticketHistory.ActivitiesId = Convert.ToInt16(StatusMain.Activities.DeleteTicketReply);
                        _ticketHistory.TicketHistory(ticketHistory);
                    }

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

        public ActionResult GetDocumentList(long? ticketId)
        {
            try
            {
                var listofattachment = _attachments.GetListAttachmentsByAttachmentId(ticketId);
                return Json(listofattachment, JsonRequestBehavior.AllowGet);
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

                    return RedirectToAction("Dashboard", "AgentAdminDashboard");
                }
                else
                {
                    return RedirectToAction("Dashboard", "AgentAdminDashboard");
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

        public ActionResult DeleteReplyAttachment(RequestAttachments requestAttachments)
        {
            try
            {
                var result = _attachments.DeleteReplyAttachmentByAttachmentIdAgent(requestAttachments.AttachmentsId);
                if (result > 0)
                {
                    TicketHistoryHelper ticketHistoryHelper = new TicketHistoryHelper();
                    TicketHistory ticketHistory = new TicketHistory
                    {
                        UserId = Convert.ToInt32(_sessionHandler.UserId),
                        Message = ticketHistoryHelper.DeleteTicketReplyAttachment(),
                        ProcessDate = DateTime.Now,
                        TicketId = requestAttachments.TicketId,
                        ActivitiesId = Convert.ToInt16(StatusMain.Activities.DeleteReplyAttachment)
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

        public ActionResult ProcessLockTicket(RequestLockTicket requestLockTicket)
        {
            try
            {
                if (requestLockTicket.TicketId != null)
                {
                    var result = _tickets.LockTicket(requestLockTicket.TicketId);
                    if (result > 0)
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
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

        public ActionResult ProcessUnLockTicket(RequestLockTicket requestLockTicket)
        {
            try
            {
                if (requestLockTicket.TicketId != null)
                {
                    var result = _tickets.UnLockTicket(requestLockTicket.TicketId);
                    if (result > 0)
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
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
    }
}