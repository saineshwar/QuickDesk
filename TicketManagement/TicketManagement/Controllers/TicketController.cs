using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CaptchaMvc.HtmlHelpers;
using TicketManagement.Common.Emails;
using TicketManagement.CommonData;
using TicketManagement.Filters;
using TicketManagement.Helpers;
using TicketManagement.Interface;
using TicketManagement.ViewModels;
using TicketManagement.Models;

namespace TicketManagement.Controllers
{
    [AuthorizeUser]
    public class TicketController : Controller
    {
        private readonly ICategory _category;
        private readonly IPriority _priority;
        private readonly ITickets _tickets;
        private readonly IAttachments _attachments;
        private readonly ITicketHistory _ticketHistory;
        private readonly IProfile _profile;
        readonly SessionHandler _sessionHandler = new SessionHandler();
        public TicketController(ICategory category, IPriority priority, ITickets tickets, IAttachments attachments,
            ITicketHistory ticketHistory, IProfile profile)
        {
            _category = category;
            _priority = priority;
            _tickets = tickets;
            _attachments = attachments;
            _ticketHistory = ticketHistory;
            _profile = profile;
        }

        // GET: Tickets
        [HttpGet]
        public ActionResult Create()
        {
            try
            {
                TicketsUserViewModel ticketsViewModel = new TicketsUserViewModel()
                {
                    ListofCategory = _category.GetAllActiveSelectListItemCategory(),
                    ListofPriority = _priority.GetAllPrioritySelectListItem()
                };
                //  MockInsert();
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
        public async Task<ActionResult> Create(TicketsUserViewModel ticketsViewModel)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var file = Request.Files;
                    var generate = new GenerateTicketNo();
                    var applicationNo =
                        generate.ApplicationNo(_category.GetCategoryCodeByCategoryId(ticketsViewModel.CategoryId));

                    var tickets = AutoMapper.Mapper.Map<Tickets>(ticketsViewModel);
                    tickets.TicketId = 0;
                    tickets.CreatedDate = DateTime.Now;
                    tickets.TrackingId = applicationNo;
                    tickets.StatusAssigned = false;
                    tickets.UserId = Convert.ToInt32(_sessionHandler.UserId);

                    var message = AppendString(ticketsViewModel.Message);
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

                    var ticketId = _tickets.AddTickets(Convert.ToInt64(_sessionHandler.UserId), tickets, ticketDetails, listofattachments,
                        listofattachmentdetails);

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

                        SendingEmailhelper sendingEmailhelper = new SendingEmailhelper();
                        Task task1 = sendingEmailhelper.SendEmailasync(applicationNo);
                        await Task.WhenAll(task1);

                    }
                    else
                    {
                        TempData["ErrorMessageTicket"] = CommonMessages.TicketErrorMessages;
                    }

                    return RedirectToAction("Create", "Ticket");
                }
                else
                {
                    ticketsViewModel.ListofCategory = _category.GetAllActiveSelectListItemCategory();
                    ticketsViewModel.ListofPriority = _priority.GetAllPrioritySelectListItem();
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
                ticketsViewModel.ListofAttachments =
                    _attachments.GetListAttachmentsByAttachmentId(ticketsViewModel.TicketId);
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


                                var message = AppendString(ticketsViewModel.Message);
                                ticketsViewModel.Message = message;

                                _tickets.UpdateUserTickets(ticketsViewModel, listofattachments, listofattachmentdetails);

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
                                ticketsViewModel.Message = AppendString(ticketsViewModel.Message);
                                _tickets.UpdateUserTickets(ticketsViewModel, null, null);

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

                return RedirectToAction("Details", "TicketDetails", new { trackingId = ticketsViewModel.TrackingId });
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
                    var decodemessage = AppendStringwithoutSigntaure(ticketReplyViewModel.Message);
                    ticketReplyViewModel.Message = decodemessage;

                    int result;
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

                            result = _tickets.UpdateTicketReply(ticketReplyViewModel, listofattachments,
                                listofattachmentdetails, Convert.ToInt32(_sessionHandler.UserId), null);
                        }
                        else
                        {
                            result = _tickets.UpdateTicketReply(ticketReplyViewModel, null, null,
                                Convert.ToInt32(_sessionHandler.UserId), null);
                        }
                    }
                    else
                    {
                        result = _tickets.UpdateTicketReply(ticketReplyViewModel, null, null,
                            Convert.ToInt32(_sessionHandler.UserId), null);
                    }


                    if (result > 0)
                    {
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

                        TempData["TicketReplyMessage"] = CommonMessages.TicketUpdatedSuccessReplyMessages;
                    }
                    return RedirectToAction("Details", "TicketDetails", new { trackingId = ticketReplyViewModel.TrackingId });
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
        private string AppendStringwithoutSigntaure(string message)
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
        public void MockInsert()
        {

            for (int i = 0; i < 100000; i++)
            {
                var generate = new GenerateTicketNo();
                var applicationNo =
                    generate.ApplicationNo(_category.GetCategoryCodeByCategoryId(1));

                var tickets = new Tickets();
                tickets.TicketId = 0;
                tickets.CreatedDate = DateTime.Now;
                tickets.TrackingId = applicationNo;
                tickets.StatusAssigned = false;
                tickets.Contact = "9999999999";
                tickets.Email = "saihacksoft@gmail.com";
                tickets.PriorityId = 3;
                tickets.CategoryId = 1;
                tickets.Name = "Joejoe";
                tickets.UserId = Convert.ToInt32(_sessionHandler.UserId);

                var message = @"<p>Lorem Ipsum&nbsp;is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry&#39;s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.</p>
            Thanks & Regards,
            IT Analyst
            ";

                var ticketDetails = new TicketDetails()
                {
                    Subject = "Lorem Ipsum is simply dummy text of the printing and typesetting indus",
                    Message = message,
                    TicketDetailsId = 0
                };

                var ticketId = _tickets.AddTickets(Convert.ToInt64(_sessionHandler.UserId), tickets, ticketDetails, null, null);
            }
        }


    }
}