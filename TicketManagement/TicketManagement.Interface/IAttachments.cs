using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Models;

namespace TicketManagement.Interface
{
    public interface IAttachments
    {
        Attachments GetAttachmentsByAttachmentId(long ticketId);
        AttachmentDetails GetAttachmentDetailsByAttachmentId(long? attachmentId);
        ReplyAttachment GetReplyAttachmentsByAttachmentId(long? ticketId);
        ReplyAttachmentDetails GetReplyAttachmentDetailsByAttachmentId(long? replyAttachmentId);
        Attachments GetAttachments(long attachmentId);
        List<Attachments> GetListAttachmentsByAttachmentId(long? ticketId);
        List<AttachmentDetails> GetListAttachmentDetailsByAttachmentId(long? attachmentId);
        List<ReplyAttachment> GetListReplyAttachmentsByAttachmentId(long? ticketReplyId);
        List<ReplyAttachmentDetails> GetListReplyAttachmentDetailsByAttachmentId(long? replyAttachmentId);
        bool CheckAttachmentsExistbyTicketId(string trackingId);
        int DeleteAttachmentByAttachmentId(long? attachmentId);
        int DeleteReplyAttachmentByAttachmentId(long? ticketId);
        bool AttachmentsExistbyTicketId(long? ticketId);
        int DeleteReplyAttachmentByAttachmentIdAgent(long? replyAttachmentId);
    }
}
