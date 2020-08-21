using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicketManagement.Concrete;
using TicketManagement.Models;

namespace TicketManagement.CommonData
{
    public class TicketReplyAttachment
    {
        private readonly DatabaseContext _context;
        public TicketReplyAttachment()
        {
            _context = new DatabaseContext();
        }
        public List<ReplyAttachment> GetListReplyAttachmentsByAttachmentId(long? ticketReplyId)
        {
            var attachmentsinfo = (from attachments in _context.ReplyAttachment
                where attachments.TicketReplyId == ticketReplyId
                                   select attachments).ToList();
            return attachmentsinfo;
        }
    }
}