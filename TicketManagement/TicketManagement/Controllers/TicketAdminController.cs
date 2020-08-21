using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CaptchaMvc.HtmlHelpers;
using TicketManagement.CommonData;
using TicketManagement.Filters;
using TicketManagement.Helpers;
using TicketManagement.Interface;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Controllers
{

    // TicketAdmin

    [AuthorizeAdmin]
    public class TicketAdminController : Controller
    {
        private readonly ICategory _category;
        private readonly IPriority _priority;
        private readonly ITickets _tickets;
        private readonly IAttachments _attachments;
        private readonly ITicketHistory _ticketHistory;
        private readonly IProfile _profile;
        private readonly IUserMaster _userMaster;
        readonly SessionHandler _sessionHandler = new SessionHandler();
        public TicketAdminController(ICategory category, IPriority priority, ITickets tickets, IAttachments attachments, ITicketHistory ticketHistory, IProfile profile, IUserMaster userMaster)
        {
            _category = category;
            _priority = priority;
            _tickets = tickets;
            _attachments = attachments;
            _ticketHistory = ticketHistory;
            _profile = profile;
            _userMaster = userMaster;
        }

        
        [HttpGet]
        public ActionResult Create()
        {
            try
            {
                TicketsViewModel ticketsViewModel = new TicketsViewModel()
                {
                    ListofCategory = _category.GetAllActiveSelectListItemCategory(),
                    ListofPriority = _priority.GetAllPrioritySelectListItem()
                };
                return View(ticketsViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(TicketsViewModel ticketsViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (ticketsViewModel.HiddenUserId == null)
                    {
                        ticketsViewModel.ListofCategory = _category.GetAllActiveSelectListItemCategory();
                        ticketsViewModel.ListofPriority = _priority.GetAllPrioritySelectListItem();
                        ticketsViewModel.Message = string.Empty;
                        ModelState.Remove("Message");
                        TempData["ErrorMessageTicket"] = "Name is AutoComplete field Don't Enter Name Choose.";
                        return View(ticketsViewModel);
                    }

                    var file = Request.Files;
                    var generate = new GenerateTicketNo();
                    var applicationNo = generate.ApplicationNo(_category.GetCategoryCodeByCategoryId(ticketsViewModel.CategoryId));
                    var userdetails = _userMaster.GetUserById(ticketsViewModel.HiddenUserId);

                    var tickets = AutoMapper.Mapper.Map<Tickets>(ticketsViewModel);
                    tickets.TicketId = 0;
                    tickets.CreatedDate = DateTime.Now;
                    tickets.TrackingId = applicationNo;
                    tickets.StatusAssigned = false;
                    tickets.Email = userdetails.EmailId;
                    tickets.Contact = userdetails.MobileNo;
                    tickets.InternalUserId = Convert.ToInt64(_sessionHandler.UserId);
                    var message = AppendSignature(ticketsViewModel.Message);
                    var ticketDetails = new TicketDetails()
                    {
                        Subject = ticketsViewModel.Subject,
                        Message = message,
                        TicketDetailsId = 0
                    };

                    var attachments = new Attachments();
                    var attachmentdetails = new AttachmentDetails();
                    // ReSharper disable once CollectionNeverQueried.Local
                    var listofattachments = new List<Attachments>();
                    // ReSharper disable once CollectionNeverQueried.Local
                    var listofattachmentdetails = new List<AttachmentDetails>();

                    for (int i = 0; i <= file.Count - 1; i++)
                    {
                        if (file[i] != null && file[i].ContentLength > 0)
                        {
                            string extension = Path.GetExtension(file[i].FileName);
                            attachments.UserId = Convert.ToInt64(_sessionHandler.UserId);
                            attachments.AttachmentName = file[i].FileName;
                            attachments.AttachmentType = extension;
                            attachments.CreatedDate = DateTime.Now;
                            var inputStream = file[i].InputStream;
                            if (inputStream != null)
                                using (var binaryReader = new BinaryReader(inputStream))
                                {
                                    byte[] fileSize = binaryReader.ReadBytes(count: file[i].ContentLength);
                                    attachmentdetails.AttachmentBytes = fileSize;
                                }

                            listofattachments.Add(attachments);
                            listofattachmentdetails.Add(attachmentdetails);
                        }
                    }

                    var ticketId = _tickets.AddTickets(Convert.ToInt64(ticketsViewModel.HiddenUserId), tickets, ticketDetails, listofattachments, listofattachmentdetails);

                    if (ticketId != -1)
                    {
                        TempData["MessageTicket"] = applicationNo + ' ' + CommonMessages.TicketSuccessMessages;

                        TicketHistoryHelper ticketHistoryHelper = new TicketHistoryHelper();
                        var ticketHistory = new TicketHistory();
                        ticketHistory.UserId = Convert.ToInt32(_sessionHandler.UserId);
                        ticketHistory.Message = ticketHistoryHelper.CreateMessage(tickets.PriorityId, tickets.CategoryId);
                        ticketHistory.CategoryId = tickets.CategoryId;
                        ticketHistory.PriorityId = tickets.PriorityId;
                        ticketHistory.StatusId = Convert.ToInt16(StatusMain.Status.Open);
                        ticketHistory.ProcessDate = DateTime.Now;
                        ticketHistory.TicketId = ticketId;
                        ticketHistory.ActivitiesId = Convert.ToInt16(StatusMain.Activities.Created);
                        _ticketHistory.TicketHistory(ticketHistory);
                    }
                    else
                    {
                        TempData["ErrorMessageTicket"] = CommonMessages.TicketErrorMessages;
                    }

                    return RedirectToAction("Create", "TicketAdmin");
                }
                else
                {
                    ticketsViewModel.ListofCategory = _category.GetAllActiveSelectListItemCategory();
                    ticketsViewModel.ListofPriority = _priority.GetAllPrioritySelectListItem();
                    ticketsViewModel.Message = string.Empty;
                    return View(ticketsViewModel);
                }
            }
            catch (Exception)
            {
                throw;
            }

        }


        [HttpGet]
        public ActionResult EditTicket(string trackingid)
        {
            try
            {
                var ticketsViewModel = _tickets.EditTicketsByTicketId(trackingid);
                ticketsViewModel.HasAttachment = _attachments.CheckAttachmentsExistbyTicketId(trackingid);
                ticketsViewModel.ListofCategory = _category.GetAllActiveSelectListItemCategory();
                ticketsViewModel.ListofPriority = _priority.GetAllPrioritySelectListItem();
                ticketsViewModel.ListofAttachments = _attachments.GetListAttachmentsByAttachmentId(ticketsViewModel.TicketId);
                return View(ticketsViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditTicket(TicketsUserViewModel ticketsViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (Request != null && (_attachments.AttachmentsExistbyTicketId(ticketsViewModel.TicketId) &&
                                            Request.Files["fileupload_1"].ContentLength != 0 &&
                                            Request.Files["fileupload_2"].ContentLength != 0 &&
                                            Request.Files["fileupload_3"].ContentLength != 0))
                    {
                        ModelState.AddModelError("", "Delete Pervious uploaded Attachments for Uploading New Attachments");
                        ticketsViewModel.HasAttachment =
                            _attachments.CheckAttachmentsExistbyTicketId(ticketsViewModel.TrackingId);
                        ticketsViewModel.ListofCategory = _category.GetAllActiveSelectListItemCategory();
                        ticketsViewModel.ListofPriority = _priority.GetAllPrioritySelectListItem();
                        ticketsViewModel.ListofAttachments =
                            _attachments.GetListAttachmentsByAttachmentId(ticketsViewModel.TicketId);
                        return View(ticketsViewModel);
                    }
                    else
                    {
                        var attachments = new Attachments();
                        var attachmentdetails = new AttachmentDetails();
                        // ReSharper disable once CollectionNeverQueried.Local
                        var listofattachments = new List<Attachments>();
                        // ReSharper disable once CollectionNeverQueried.Local
                        var listofattachmentdetails = new List<AttachmentDetails>();

                        if (Request != null && Request.Files.AllKeys.Any())
                        {
                            var file = Request.Files;

                            if (Request.Files["fileupload_1"].ContentLength != 0 ||
                                Request.Files["fileupload_2"].ContentLength != 0 ||
                                Request.Files["fileupload_3"].ContentLength != 0)
                            {

                                for (int i = 0; i <= file.Count - 1; i++)
                                {
                                    if (file[i] != null && file[i].ContentLength > 0)
                                    {
                                        string fileName = Path.GetFileName(file[i].FileName);
                                        string extension = Path.GetExtension(file[i].FileName);

                                        attachments.UserId = Convert.ToInt64(_sessionHandler.UserId);

                                        attachments.AttachmentName = fileName;
                                        attachments.AttachmentType = extension;
                                        attachments.CreatedDate = DateTime.Now;

                                        var inputStream = file[i]?.InputStream;
                                        if (inputStream != null)
                                            using (var binaryReader = new BinaryReader(inputStream))
                                            {
                                                byte[] fileSize = binaryReader.ReadBytes(count: file[i].ContentLength);
                                                attachmentdetails.AttachmentBytes = fileSize;
                                            }

                                        listofattachments.Add(attachments);
                                        listofattachmentdetails.Add(attachmentdetails);
                                    }
                                }

                                var message = AppendSignature(ticketsViewModel.Message);
                                ticketsViewModel.Message = message;
                                _tickets.UpdateTickets(ticketsViewModel, listofattachments, listofattachmentdetails);

                                TicketHistoryHelper ticketHistoryHelper = new TicketHistoryHelper();
                                TicketHistory ticketHistory = new TicketHistory
                                {
                                    UserId = Convert.ToInt32(_sessionHandler.UserId),
                                    Message = ticketHistoryHelper.EditTicket(),
                                    ProcessDate = DateTime.Now,
                                    TicketId = ticketsViewModel.TicketId,
                                    ActivitiesId = Convert.ToInt16(StatusMain.Activities.EditedTicket)
                                };
                                _ticketHistory.TicketHistory(ticketHistory);


                                TempData["TicketReplyMessage"] = CommonMessages.TicketUpdatedSuccessReplyMessages;
                            }
                            else
                            {
                                ticketsViewModel.Message = AppendSignature(ticketsViewModel.Message);
                                _tickets.UpdateTickets(ticketsViewModel, null, null);
                                TicketHistoryHelper ticketHistoryHelper = new TicketHistoryHelper();
                                TicketHistory ticketHistory = new TicketHistory
                                {
                                    UserId = Convert.ToInt32(_sessionHandler.UserId),
                                    Message = ticketHistoryHelper.EditTicket(),
                                    ProcessDate = DateTime.Now,
                                    TicketId = ticketsViewModel.TicketId,
                                    ActivitiesId = Convert.ToInt16(StatusMain.Activities.EditedTicket)
                                };
                                _ticketHistory.TicketHistory(ticketHistory);

                                TempData["TicketReplyMessage"] = CommonMessages.TicketUpdatedSuccessReplyMessages;
                            }
                        }


                    }
                }
                return RedirectToAction("Details", "TicketDetailsAdmin",
                    new { trackingId = ticketsViewModel.TrackingId });
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult EditReply(string tracking, long ticketreply)
        {
            try
            {
                var ticketsViewModel = _tickets.EditTicketsByTicketReplyId(tracking, ticketreply);
                return View(ticketsViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditReply(TicketReplyViewModel ticketReplyViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int result;
                    var decodemessage = DecodeHtml(ticketReplyViewModel.Message);
                    ticketReplyViewModel.Message = decodemessage;

                    var file = Request.Files;
                    ReplyAttachment replyAttachment = new ReplyAttachment();
                    ReplyAttachmentDetails replyAttachmentDetails = new ReplyAttachmentDetails();

                    // ReSharper disable once CollectionNeverQueried.Local
                    var listofattachments = new List<ReplyAttachment>();
                    // ReSharper disable once CollectionNeverQueried.Local
                    var listofattachmentdetails = new List<ReplyAttachmentDetails>();

                    if (file.Count > 0)
                    {
                        if (Request.Files["fileupload_1"].ContentLength != 0 ||
                            Request.Files["fileupload_2"].ContentLength != 0 ||
                            Request.Files["fileupload_3"].ContentLength != 0)
                        {

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

                            result = _tickets.UpdateTicketReply(ticketReplyViewModel, listofattachments,
                                listofattachmentdetails,null, Convert.ToInt32(_sessionHandler.UserId));
                        }

                        else
                        {
                            result = _tickets.UpdateTicketReply(ticketReplyViewModel, null, null,
                                null, Convert.ToInt32(_sessionHandler.UserId));
                        }

                    }
                    else
                    {
                        result = _tickets.UpdateTicketReply(ticketReplyViewModel, null, null,
                            null, Convert.ToInt32(_sessionHandler.UserId));
                    }

                    TicketHistoryHelper ticketHistoryHelper = new TicketHistoryHelper();
                    TicketHistory ticketHistory = new TicketHistory
                    {
                        UserId = Convert.ToInt32(_sessionHandler.UserId),
                        Message = ticketHistoryHelper.EditReplyTicket(),
                        ProcessDate = DateTime.Now,
                        TicketId = ticketReplyViewModel.TicketId,
                        ActivitiesId = Convert.ToInt16(StatusMain.Activities.EditedTicketReply)
                    };
                    _ticketHistory.TicketHistory(ticketHistory);

                    if (result > 0)
                    {
                        TempData["TicketReplyMessage"] = CommonMessages.TicketUpdatedSuccessReplyMessages;
                    }
                    return RedirectToAction("Details", "TicketDetailsAdmin", new { trackingId = ticketReplyViewModel.TrackingId });
                }
                else
                {
                    var ticketsViewModel = _tickets.EditTicketsByTicketReplyId(ticketReplyViewModel.TrackingId, ticketReplyViewModel.TicketReplyId);
                    return View(ticketsViewModel);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult GetAllUsers(string usernames)
        {
            var userlist = _userMaster.GetAutoCompleteUserName(usernames, Convert.ToInt32(StatusMain.Roles.User));
            return Json(userlist, JsonRequestBehavior.AllowGet);
        }

        private string AppendSignature(string message)
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
        private string DecodeHtml(string message)
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
    }
}