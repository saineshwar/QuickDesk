using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Interface
{
    public interface ITickets
    {

        long AddTickets(long? userId, Tickets tickets, TicketDetails ticketDetails, List<Attachments> attachments, List<AttachmentDetails> attachmentDetails);
        int UpdateTickets(TicketsUserViewModel ticketsViewModel, List<Attachments> attachments, List<AttachmentDetails> attachmentDetails);

        int UpdateUserTickets(TicketsUserViewModel ticketsViewModel, List<Attachments> attachments, List<AttachmentDetails> attachmentDetails);
        //------------------------------------------------Change ----------------------------------------------//
        bool ChangeTicketPriority(ChangePriorityRequestModel changePriorityRequestModel);
        bool ChangeTicketStatus(ChangeStatusRequestModel changeStatusRequestModel);
        bool ChangeTicketCategory(ChangeCategoryRequestModel changeCategoryRequestModel);
        //------------------------------------------------Change ----------------------------------------------//

        TicketsUserViewModel EditTicketsByTicketId(string trackingId);
        TicketReplyViewModel EditTicketsByTicketReplyId(string trackingId, long? reply);
        int UpdateTicketReply(TicketReplyViewModel ticketsViewModel, List<ReplyAttachment> replyAttachment,
            List<ReplyAttachmentDetails> replyAttachmentDetails, long? ticketUser, long? systemUser);
        int UpdateAssignTickettoUser(long userId, long ticketId);

        //------------------------------------------------Dashboard ----------------------------------------------//

        // -- Admin


        int GetAllTicketsCount(string search, string searchin, int? prioritysearch, int? statussearch);
        List<ViewTicketModel> GetAllAdminTicketsbyList
            (string search, string searchin, int? prioritysearch, int? statussearch, int startIndex, int count, string sorting);
        // -- AgentAdmin
        int GetAllAgentAdminTicketsCount(string search, string searchin, int? prioritysearch, int? statussearch, int categoryId);
        List<ViewTicketModel> GetAllAgentAdminTicketsbyList(string search, string searchin, int categoryId,
            int? prioritysearch, int? statussearch, int startIndex, int count, string sorting);
        // -- Agent
        int GetTicketsAgentCount(long userId, string search, string searchin, int? prioritysearch, int? statussearch);
        List<ViewTicketModel> GetAllAgentTicketsbyUserId(long userId, string search, string searchin,
            int? prioritysearch, int? statussearch, int startIndex, int count, string sorting);
        // -- User
        int GetTicketsCount(long userId, string search, int? statusId, string searchin);

        List<ViewTicketModel> GetAllTicketsbyUserId(string search, string searchin, int? statusId, long userId, int startIndex,
            int count, string sorting);

        //------------------------------------------------Dashboard ----------------------------------------------//

        int DeleteTicket(long userId, long ticketId, short statusId);
        List<ViewTicketModel> GetAllDeleteTickets(string search, string searchin, int? prioritysearch, int? statussearch, int startIndex, int count, string sorting);

        int GetDeleteTicketsAgentsAdminCount(long userId, string categoryId);
        List<ViewTicketModel> GetAllAgentsAdminDeleteTickets(string search, string searchin, int categoryId, int? prioritysearch, int? statussearch, int startIndex, int count, string sorting);


        int GetDeleteTicketsAgentCount(long userId);
        int AssignTickettoAgent(long userId, long ticketId);
        int LockTicket(long? ticketId);
        int GetTicketLockStatus(long ticketId);
        int UnLockTicket(long? ticketId);

         List<ViewTicketModel> GetAllDeleteTicketsbyAgent(long userId, string search, string searchin,
            int? prioritysearch, int? statussearch, int startIndex, int count, string sorting);

         int RestoreTicket(long userId, long ticketId);
    }
}
