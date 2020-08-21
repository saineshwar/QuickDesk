using System.Collections.Generic;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Interface
{
    public interface ITicketsReply
    {
        void ReplyTicket(TicketReplyModel ticketReplyModel, List<ReplyAttachment> replyAttachment,
            List<ReplyAttachmentDetails> replyAttachmentDetails,
            long? fromuser, long? systemuser, int? status);
        List<ViewTicketReplyHistoryModel> ListofHistoryTicketReplies(string trackingId);
        int DeleteTicketReply(long? ticketReplyId, long? ticketId, long userId);
        int RestoreTicketReply(long? ticketReplyId, long? ticketId, long userId);
    }
}