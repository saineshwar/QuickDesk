using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketManagement.Interface;

namespace TicketManagement.Controllers
{
    public class DeleteAttachmentController : Controller
    {
        private readonly IAttachments _attachments;
        public DeleteAttachmentController(IAttachments attachments)
        {
            _attachments = attachments;
        }
        // GET: DeleteAttachment
        public ActionResult Delete(string trackingId, long? ticketId)
        {
            if (ticketId != null && !string.IsNullOrEmpty(trackingId))
            {
                _attachments.DeleteAttachmentByAttachmentId(ticketId);
                return RedirectToAction("Details", "TicketDetails", new { TrackingId = trackingId });
            }
            else
            {
                return RedirectToAction("Dashboard", "UserDashboard");
            }
        }
    }
}